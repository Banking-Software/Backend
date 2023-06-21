using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations.Application
{
    /// <inheritdoc />
    public partial class Charkhata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupTypes_AccountTypeId_Name_CharKhataNumber",
                table: "GroupTypes");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTypes_AccountTypeId_Name",
                table: "GroupTypes",
                columns: new[] { "AccountTypeId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupTypes_CharKhataNumber",
                table: "GroupTypes",
                column: "CharKhataNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupTypes_AccountTypeId_Name",
                table: "GroupTypes");

            migrationBuilder.DropIndex(
                name: "IX_GroupTypes_CharKhataNumber",
                table: "GroupTypes");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTypes_AccountTypeId_Name_CharKhataNumber",
                table: "GroupTypes",
                columns: new[] { "AccountTypeId", "Name", "CharKhataNumber" },
                unique: true);
        }
    }
}
