using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations.Application
{
    /// <inheritdoc />
    public partial class Client : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "States");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "MaritalStatuses");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Genders");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "ClientCast",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ClientGender",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ClientMaritalStatus",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PermanentDistrict",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PermanentDistrictNepali",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Casts");

            migrationBuilder.RenameColumn(
                name: "TemporaryState",
                table: "Clients",
                newName: "NomineePhotoFileName");

            migrationBuilder.RenameColumn(
                name: "TemporaryDistrictNepali",
                table: "Clients",
                newName: "ClientSignatureFileName");

            migrationBuilder.RenameColumn(
                name: "TemporaryDistrict",
                table: "Clients",
                newName: "ClientPhotoFileName");

            migrationBuilder.RenameColumn(
                name: "PermanentState",
                table: "Clients",
                newName: "ClientCitizenshipFileName");

            migrationBuilder.AddColumn<string>(
                name: "EnglishName",
                table: "States",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NepaliName",
                table: "States",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishName",
                table: "MaritalStatuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NepaliName",
                table: "MaritalStatuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishName",
                table: "Genders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NepaliName",
                table: "Genders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishName",
                table: "Districts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NepaliName",
                table: "Districts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TemporaryStateCode",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ClientCitizenshipFileData",
                table: "Clients",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientCitizenshipFileType",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ClientPhotoFileData",
                table: "Clients",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientPhotoFileType",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ClientSignatureFileData",
                table: "Clients",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientSignatureFileType",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "NomineePhotoFileData",
                table: "Clients",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NomineePhotoFileType",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishName",
                table: "Casts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NepaliName",
                table: "Casts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnglishName",
                table: "States");

            migrationBuilder.DropColumn(
                name: "NepaliName",
                table: "States");

            migrationBuilder.DropColumn(
                name: "EnglishName",
                table: "MaritalStatuses");

            migrationBuilder.DropColumn(
                name: "NepaliName",
                table: "MaritalStatuses");

            migrationBuilder.DropColumn(
                name: "EnglishName",
                table: "Genders");

            migrationBuilder.DropColumn(
                name: "NepaliName",
                table: "Genders");

            migrationBuilder.DropColumn(
                name: "EnglishName",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "NepaliName",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "ClientCitizenshipFileData",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ClientCitizenshipFileType",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ClientPhotoFileData",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ClientPhotoFileType",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ClientSignatureFileData",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ClientSignatureFileType",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "NomineePhotoFileData",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "NomineePhotoFileType",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "EnglishName",
                table: "Casts");

            migrationBuilder.DropColumn(
                name: "NepaliName",
                table: "Casts");

            migrationBuilder.RenameColumn(
                name: "NomineePhotoFileName",
                table: "Clients",
                newName: "TemporaryState");

            migrationBuilder.RenameColumn(
                name: "ClientSignatureFileName",
                table: "Clients",
                newName: "TemporaryDistrictNepali");

            migrationBuilder.RenameColumn(
                name: "ClientPhotoFileName",
                table: "Clients",
                newName: "TemporaryDistrict");

            migrationBuilder.RenameColumn(
                name: "ClientCitizenshipFileName",
                table: "Clients",
                newName: "PermanentState");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "States",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "MaritalStatuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Genders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Districts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "TemporaryStateCode",
                table: "Clients",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientCast",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientGender",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientMaritalStatus",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentDistrict",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentDistrictNepali",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Casts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
