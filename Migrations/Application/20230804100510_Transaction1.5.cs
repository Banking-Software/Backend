using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations.Application
{
    /// <inheritdoc />
    public partial class Transaction15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "SignatureFileData",
                table: "DepositAccounts",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignatureFileName",
                table: "DepositAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SignatureFileType",
                table: "DepositAccounts",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignatureFileData",
                table: "DepositAccounts");

            migrationBuilder.DropColumn(
                name: "SignatureFileName",
                table: "DepositAccounts");

            migrationBuilder.DropColumn(
                name: "SignatureFileType",
                table: "DepositAccounts");
        }
    }
}
