using Aiglusoft.IAM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aiglusoft.IAM.Infrastructure.Persistence.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
        public DbSet<ClientScope> ClientScopes { get; set; }
        public DbSet<ClientGrantType> ClientGrantTypes { get; set; }
        public DbSet<AuthorizationCode> AuthorizationCodes { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.HasMany(e => e.Claims).WithOne(c => c.User).HasForeignKey(c => c.UserId);
            });

            // Configure UserClaim entity
            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.HasKey(e => e.UserClaimId);
            });

            // Configure Client entity
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.ClientId);
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
    }
    }

