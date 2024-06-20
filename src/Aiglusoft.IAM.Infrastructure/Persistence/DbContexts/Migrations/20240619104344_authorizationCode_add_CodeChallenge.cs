using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aiglusoft.IAM.Infrastructure.Persistence.DbContexts.Migrations
{
    /// <inheritdoc />
    public partial class authorizationCode_add_CodeChallenge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeChallenge",
                table: "AuthorizationCodes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodeChallengeMethod",
                table: "AuthorizationCodes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeChallenge",
                table: "AuthorizationCodes");

            migrationBuilder.DropColumn(
                name: "CodeChallengeMethod",
                table: "AuthorizationCodes");
        }
    }
}
