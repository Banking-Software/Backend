using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations.Application
{
    /// <inheritdoc />
    public partial class Transaction11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectedBy",
                table: "DepositAccountTransactions");

            migrationBuilder.AddColumn<int>(
                name: "CollectedByEmployeeId",
                table: "DepositAccountTransactions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectedByEmployeeId",
                table: "DepositAccountTransactions");

            migrationBuilder.AddColumn<string>(
                name: "CollectedBy",
                table: "DepositAccountTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
