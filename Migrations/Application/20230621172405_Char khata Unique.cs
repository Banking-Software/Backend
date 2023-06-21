using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations.Application
{
    /// <inheritdoc />
    public partial class CharkhataUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupTypes_AccountTypeId_Name",
                table: "GroupTypes");

            migrationBuilder.AlterColumn<string>(
                name: "CharKhataNumber",
                table: "GroupTypes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTypes_AccountTypeId_Name_CharKhataNumber",
                table: "GroupTypes",
                columns: new[] { "AccountTypeId", "Name", "CharKhataNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupTypes_AccountTypeId_Name_CharKhataNumber",
                table: "GroupTypes");

            migrationBuilder.AlterColumn<string>(
                name: "CharKhataNumber",
                table: "GroupTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTypes_AccountTypeId_Name",
                table: "GroupTypes",
                columns: new[] { "AccountTypeId", "Name" },
                unique: true);
        }
    }
}
