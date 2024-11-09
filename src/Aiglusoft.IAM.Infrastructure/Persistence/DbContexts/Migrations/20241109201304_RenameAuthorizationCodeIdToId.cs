using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aiglusoft.IAM.Infrastructure.Persistence.DbContexts.Migrations
{
    /// <inheritdoc />
    public partial class RenameAuthorizationCodeIdToId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Client_ClientId",
                table: "Client");

            migrationBuilder.RenameColumn(
                name: "AuthorizationCodeId",
                table: "AuthorizationCode",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AuthorizationCode",
                newName: "AuthorizationCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_ClientId",
                table: "Client",
                column: "ClientId");
        }
    }
}
