using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations.Application
{
    /// <inheritdoc />
    public partial class CompanyProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyLogo",
                table: "CompanyDetails",
                newName: "LogoFileName");

            migrationBuilder.AddColumn<byte[]>(
                name: "LogoFileData",
                table: "CompanyDetails",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LogoFileType",
                table: "CompanyDetails",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoFileData",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "LogoFileType",
                table: "CompanyDetails");

            migrationBuilder.RenameColumn(
                name: "LogoFileName",
                table: "CompanyDetails",
                newName: "CompanyLogo");
        }
    }
}
