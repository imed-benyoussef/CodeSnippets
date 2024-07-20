
namespace Aiglusoft.IAM.Infrastructure.Persistence.DbContexts.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Aiglusoft.IAM.Domain.Model.UserAggregates;

    public class UserConfiguration : IEntityTypeConfiguration<User>
        ,IEntityTypeConfiguration<UserClaim>
        , IEntityTypeConfiguration<UserRole>

    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Configuration de la table
            builder.ToTable("Users");

            // Configuration de la clé primaire
            builder.HasKey(u => u.Id);

            // Configuration des propriétés
            builder.Property(u => u.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .HasMaxLength(50);

            builder.Property(u => u.Birthdate)
                .HasColumnType("date"); // Utilise le type de colonne SQL 'date' pour la propriété Birthdate

            builder.Property(u => u.Gender)
                .HasMaxLength(10);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.EmailVerificationHash)
                .IsRequired(false);

            builder.Property(u => u.EmailVerified);

            builder.Property(u => u.PasswordHash)
                .IsRequired(false);

            builder.Property(u => u.SecurityStamp)
                .IsRequired(false);

            builder.Property(u => u.IsActive)
                .IsRequired();

            builder.Property("createdAt")
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property("updatedAt")
                .HasColumnName("UpdatedAt");

            // Configuration des relations si nécessaire
            // Par exemple, si User a une relation avec UserClaim et UserRole
            builder.HasMany(u => u.Claims)
                .WithOne()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Roles)
                .WithOne()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("UserClaims");

            builder.HasKey(uc => uc.Id);

            builder.Property(uc => uc.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(uc => uc.UserId)
                .IsRequired();

            builder.Property(uc => uc.ClaimType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(uc => uc.ClaimValue)
                .IsRequired()
                .HasMaxLength(50);
        }

        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder.HasKey(ur => ur.Id);

            builder.Property(ur => ur.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(ur => ur.UserId)
                .IsRequired();

            builder.Property(ur => ur.Role)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
