﻿// <auto-generated />
using System;
using Aiglusoft.IAM.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Aiglusoft.IAM.Infrastructure.Persistence.DbContexts.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240618194010_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.AuthorizationCode", b =>
                {
                    b.Property<string>("AuthorizationCodeId")
                        .HasColumnType("text");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Expiry")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RedirectUri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Scopes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AuthorizationCodeId");

                    b.HasIndex("ClientId");

                    b.HasIndex("UserId");

                    b.ToTable("AuthorizationCodes");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.Client", b =>
                {
                    b.Property<string>("ClientId")
                        .HasColumnType("text");

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ClientSecret")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ClientId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.ClientGrantType", b =>
                {
                    b.Property<string>("ClientGrantTypeId")
                        .HasColumnType("text");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("GrantType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ClientGrantTypeId");

                    b.HasIndex("ClientId");

                    b.ToTable("ClientGrantTypes");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.ClientRedirectUri", b =>
                {
                    b.Property<string>("ClientRedirectUriId")
                        .HasColumnType("text");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RedirectUri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ClientRedirectUriId");

                    b.HasIndex("ClientId");

                    b.ToTable("ClientRedirectUris");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.ClientScope", b =>
                {
                    b.Property<string>("ClientScopeId")
                        .HasColumnType("text");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Scope")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ClientScopeId");

                    b.HasIndex("ClientId");

                    b.ToTable("ClientScopes");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.Token", b =>
                {
                    b.Property<string>("TokenId")
                        .HasColumnType("text");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Expiry")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TokenType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("TokenId");

                    b.HasIndex("ClientId");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("EmailVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.UserClaim", b =>
                {
                    b.Property<string>("UserClaimId")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserClaimId");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.AuthorizationCode", b =>
                {
                    b.HasOne("Aiglusoft.IAM.Domain.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Aiglusoft.IAM.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.ClientGrantType", b =>
                {
                    b.HasOne("Aiglusoft.IAM.Domain.Entities.Client", "Client")
                        .WithMany("GrantTypes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.ClientRedirectUri", b =>
                {
                    b.HasOne("Aiglusoft.IAM.Domain.Entities.Client", "Client")
                        .WithMany("RedirectUris")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.ClientScope", b =>
                {
                    b.HasOne("Aiglusoft.IAM.Domain.Entities.Client", "Client")
                        .WithMany("Scopes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.Token", b =>
                {
                    b.HasOne("Aiglusoft.IAM.Domain.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Aiglusoft.IAM.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.UserClaim", b =>
                {
                    b.HasOne("Aiglusoft.IAM.Domain.Entities.User", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.Client", b =>
                {
                    b.Navigation("GrantTypes");

                    b.Navigation("RedirectUris");

                    b.Navigation("Scopes");
                });

            modelBuilder.Entity("Aiglusoft.IAM.Domain.Entities.User", b =>
                {
                    b.Navigation("Claims");
                });
#pragma warning restore 612, 618
        }
    }
}
