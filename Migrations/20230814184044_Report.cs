using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations
{
    /// <inheritdoc />
    public partial class Report : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Narration",
                table: "SubLedgerTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Narration",
                table: "LedgerTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Narration",
                table: "SubLedgerTransactions");

            migrationBuilder.DropColumn(
                name: "Narration",
                table: "LedgerTransactions");
        }
    }
}
