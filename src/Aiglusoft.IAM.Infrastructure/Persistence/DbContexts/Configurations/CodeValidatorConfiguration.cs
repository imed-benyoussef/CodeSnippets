

namespace Aiglusoft.IAM.Infrastructure.Persistence.DbContexts.Configurations
{
    using Aiglusoft.IAM.Domain.Model.CodeValidators;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CodeValidatorConfiguration : IEntityTypeConfiguration<CodeValidator>
    {
        public void Configure(EntityTypeBuilder<CodeValidator> builder)
        {
            // Définir le nom de la table
            builder.ToTable("CodeValidators");

            // Clé primaire
            builder.HasKey(cv => cv.Id);

            // Noms des colonnes
            builder.Property(cv => cv.Id)
                   .HasColumnName("Id");  // Nom de la colonne pour Id

            // Propriétés

            builder.Property(cv => cv.Code)
                  .IsRequired()
                  .HasMaxLength(6);

            builder.Property("createdAt")
                   .IsRequired()
                   .HasColumnName("CreatedAt");

            builder.Property<DateTime>("expiresAt")
               .IsRequired()  // Date d'expiration doit être spécifiée
               .HasColumnName("ExpiresAt");  // Nom de la colonne pour ExpiresAt

            builder.Property(cv => cv.Code)
                   .IsRequired()  // Code ne doit pas être null
                   .HasMaxLength(6)  // Longueur maximale pour le code
                   .HasColumnName("VerificationCode");  // Nom de la colonne pour Code


            builder.Property(cv => cv.Target)
                   .IsRequired()  // Cible du code (email ou téléphone)
                   .HasMaxLength(256)  // Taille maximale pour l'adresse email ou le numéro de téléphone
                   .HasColumnName("Target");  // Nom de la colonne pour Target

            builder.Property(cv => cv.Type)
                   .IsRequired()
                   .HasConversion(
                       v => v.ToString(),  // Convertir de l'enum vers une string lors de l'enregistrement dans la BD
                       v => (VerificationType)Enum.Parse(typeof(VerificationType), v))  // Convertir de string vers enum lors de la lecture de la BD
                   .HasColumnName("VerificationType");  // Nom de la colonne pour Type

            builder.Property(cv => cv.Status)
              .IsRequired()
              .HasConversion(
                  v => v.ToString(),
                  v => (CodeStatus)Enum.Parse(typeof(CodeStatus), v)
              )
              .HasColumnName("Status");

            // Index
            builder.HasIndex(cv => new { cv.Target, cv.Code })  // Un index sur la cible et le code pour des recherches rapides
                  .IsUnique()  // Assure que la combinaison de cible et code est unique
                  .HasDatabaseName("IX_CodeValidators_Target_Code");  // Nom de l'index
        }
    }


}
