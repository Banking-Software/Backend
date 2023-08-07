using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations.Application
{
    /// <inheritdoc />
    public partial class ShareTransaction11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareAccounts_Clients_ClientId1",
                table: "ShareAccounts");

            migrationBuilder.DropIndex(
                name: "IX_ShareAccounts_ClientId",
                table: "ShareAccounts");

            migrationBuilder.DropIndex(
                name: "IX_ShareAccounts_ClientId1",
                table: "ShareAccounts");

            migrationBuilder.DropColumn(
                name: "ClientId1",
                table: "ShareAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAccounts_ClientId",
                table: "ShareAccounts",
                column: "ClientId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShareAccounts_ClientId",
                table: "ShareAccounts");

            migrationBuilder.AddColumn<int>(
                name: "ClientId1",
                table: "ShareAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShareAccounts_ClientId",
                table: "ShareAccounts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAccounts_ClientId1",
                table: "ShareAccounts",
                column: "ClientId1",
                unique: true,
                filter: "[ClientId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareAccounts_Clients_ClientId1",
                table: "ShareAccounts",
                column: "ClientId1",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
