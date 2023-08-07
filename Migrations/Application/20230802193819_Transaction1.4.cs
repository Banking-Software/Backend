using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations.Application
{
    /// <inheritdoc />
    public partial class Transaction14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareAccounts_Clients_AccountNumber",
                table: "ShareAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareAccounts",
                table: "ShareAccounts");

            migrationBuilder.DropIndex(
                name: "IX_ShareAccounts_AccountNumber",
                table: "ShareAccounts");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "ShareAccounts");

            migrationBuilder.DropColumn(
                name: "EndOn",
                table: "ShareAccounts");

            migrationBuilder.DropColumn(
                name: "StartOn",
                table: "ShareAccounts");

            migrationBuilder.RenameTable(
                name: "ShareAccounts",
                newName: "ShareAccount");

            migrationBuilder.RenameColumn(
                name: "CurrentNumberOfKitta",
                table: "ShareAccount",
                newName: "ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareAccount",
                table: "ShareAccount",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAccount_ClientId",
                table: "ShareAccount",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareAccount_Clients_ClientId",
                table: "ShareAccount",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareAccount_Clients_ClientId",
                table: "ShareAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareAccount",
                table: "ShareAccount");

            migrationBuilder.DropIndex(
                name: "IX_ShareAccount_ClientId",
                table: "ShareAccount");

            migrationBuilder.RenameTable(
                name: "ShareAccount",
                newName: "ShareAccounts");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ShareAccounts",
                newName: "CurrentNumberOfKitta");

            migrationBuilder.AddColumn<int>(
                name: "AccountNumber",
                table: "ShareAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndOn",
                table: "ShareAccounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartOn",
                table: "ShareAccounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareAccounts",
                table: "ShareAccounts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAccounts_AccountNumber",
                table: "ShareAccounts",
                column: "AccountNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareAccounts_Clients_AccountNumber",
                table: "ShareAccounts",
                column: "AccountNumber",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
