using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aiglusoft.IAM.Infrastructure.Persistence.DbContexts.Migrations
{
    /// <inheritdoc />
    public partial class User_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_updatedAt",
                table: "Users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "_createdAt",
                table: "Users",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Users",
                newName: "_updatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "_createdAt");
        }
    }
}
