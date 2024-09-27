using Aiglusoft.IAM.Application.Queries;
using Aiglusoft.IAM.Domain.Model;
using Aiglusoft.IAM.Domain.Model.AuthorizationAggregates;
using Aiglusoft.IAM.Domain.Model.ClientAggregates;
using Aiglusoft.IAM.Domain.Model.CodeValidators;
using Aiglusoft.IAM.Domain.Model.TokenAggregate;
using Aiglusoft.IAM.Domain.Model.UserAggregates;
using Aiglusoft.IAM.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Aiglusoft.IAM.Infrastructure.Persistence.DbContexts
{
  public class AppDbContext : DbContext, IQueryContext, IUnitOfWork, IDataProtectionKeyContext
  {
    private readonly IMediator _mediator;
    private IDbContextTransaction _currentTransaction;


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator) : base(options)
    {
      _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


      System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
    }

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
    public IQueryable<User> Users => Set<User>().AsNoTracking();
    public IQueryable<UserClaim> UserClaims => Set<UserClaim>().AsNoTracking();
    public IQueryable<Client> Clients => Set<Client>().AsNoTracking();
    public IQueryable<ClientRedirectUri> ClientRedirectUris => Set<ClientRedirectUri>().AsNoTracking();
    public IQueryable<ClientScope> ClientScopes => Set<ClientScope>().AsNoTracking();
    public IQueryable<ClientGrantType> ClientGrantTypes => Set<ClientGrantType>().AsNoTracking();
    public IQueryable<AuthorizationCode> AuthorizationCodes => Set<AuthorizationCode>().AsNoTracking();
    public IQueryable<Token> Tokens => Set<Token>().AsNoTracking();
    public IQueryable<CodeValidator> CodeValidators => Set<CodeValidator>().AsNoTracking();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

      // Configure Client entity
      modelBuilder.Entity<Client>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.ClientId);
        entity.HasMany(e => e.RedirectUris).WithOne(r => r.Client).HasForeignKey(r => r.ClientId);
        entity.HasMany(e => e.Scopes).WithOne(s => s.Client).HasForeignKey(s => s.ClientId);
        entity.HasMany(e => e.GrantTypes).WithOne(g => g.Client).HasForeignKey(g => g.ClientId);
      });

      // Configure ClientRedirectUri entity
      modelBuilder.Entity<ClientRedirectUri>(entity =>
      {
        entity.HasKey(e => e.ClientRedirectUriId);
      });

      // Configure ClientScope entity
      modelBuilder.Entity<ClientScope>(entity =>
      {
        entity.HasKey(e => e.ClientScopeId);
      });

      // Configure ClientGrantType entity
      modelBuilder.Entity<ClientGrantType>(entity =>
      {
        entity.HasKey(e => e.ClientGrantTypeId);
      });

      // Configure AuthorizationCode entity
      modelBuilder.Entity<AuthorizationCode>(entity =>
      {
        entity.HasKey(e => e.AuthorizationCodeId);
        entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
        entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
      });

      // Configure Token entity
      modelBuilder.Entity<Token>(entity =>
      {
        entity.HasKey(e => e.TokenId);
        entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
        entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
      });

    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
      // Dispatch Domain Events collection. 
      // Choices:
      // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
      // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
      // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
      // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
      await _mediator.DispatchDomainEventsAsync(this);

      // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
      // performed through the DbContext will be committed
      _ = await SaveChangesAsync(cancellationToken);

      return true;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
      if (_currentTransaction != null) return null;

      _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

      return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
      if (transaction == null) throw new ArgumentNullException(nameof(transaction));
      if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

      try
      {
        await SaveChangesAsync();
        await transaction.CommitAsync();
      }
      catch
      {
        RollbackTransaction();
        throw;
      }
      finally
      {
        if (_currentTransaction != null)
        {
          _currentTransaction.Dispose();
          _currentTransaction = null;
        }
      }
    }

    public void RollbackTransaction()
    {
      try
      {
        _currentTransaction?.Rollback();
      }
      finally
      {
        if (_currentTransaction != null)
        {
          _currentTransaction.Dispose();
          _currentTransaction = null;
        }
      }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      UpdateTimestamps();
      return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
      var entries = ChangeTracker.Entries()
          .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

      foreach (var entry in entries)
      {
        if (entry.Metadata.FindProperty("createdAt") != null)
        {
          if (entry.State == EntityState.Added)
          {
            entry.Property("createdAt").CurrentValue = DateTime.UtcNow;
          }
        }
        if (entry.Metadata.FindProperty("createdAt") != null && entry.Metadata.FindProperty("updatedAt") != null)
        {
          if (entry.State == EntityState.Modified)
          {
            entry.Property("updatedAt").CurrentValue = DateTime.UtcNow;
          }
        }
      }
    }

    public IQueryable<T> GetQueryable<T>() where T : class
      => Set<T>().AsNoTracking();
  }

  static class MediatorExtension
  {
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, AppDbContext ctx)
    {
      var domainEntities = ctx.ChangeTracker
          .Entries<IEntity>()
          .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

      var domainEvents = domainEntities
          .SelectMany(x => x.Entity.DomainEvents)
          .ToList();

      domainEntities.ToList()
          .ForEach(entity => entity.Entity.ClearDomainEvents());

      foreach (var domainEvent in domainEvents)
        await mediator.Publish(domainEvent);
    }
  }
}

