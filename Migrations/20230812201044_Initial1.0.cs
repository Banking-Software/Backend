using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MicroFinance.Migrations
{
    /// <inheritdoc />
    public partial class Initial10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "025fed80-e073-4d0d-b7d6-fdcfc8d8eea1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3aac356f-292c-42fd-a26d-e3cdda262b0e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4acbdd75-a4b9-4444-8f78-86c713da3054");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81faa567-90a6-402d-898a-877ecb623a12");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cfa33577-db62-455f-a52a-d55e4bdc9f93");

            migrationBuilder.AlterColumn<string>(
                name: "NepaliModificationDate",
                table: "DepositSchemes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NepaliModificationDate",
                table: "DepositAccounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "27fc9915-b334-43ee-a2d5-20de61f1b783", "7212f470-8f08-4db2-bfcc-d9bc29ee054a", "SuperAdmin", "SUPERADMIN" },
                    { "589d7fa1-85d0-4cf0-90f9-9aae22abcefd", "b372d4e9-c909-461b-9743-91076e7bf8d0", "Marketing", "MARKETING" },
                    { "58c2f618-3224-43a2-9592-8777f1cea718", "f825e1a8-e4fb-4648-9ec5-1cb40643e4fe", "SeniorAssistant", "SENIORASSISTANT" },
                    { "bbd82b69-3c4d-459d-89e1-33ed516d4c4d", "be6e7b9c-75af-4121-a579-b49aa80984a5", "Officer", "OFFICER" },
                    { "f9efd1c0-1208-4d5b-a6ac-19ad01656f7b", "b3204ae1-ef64-490a-a2b2-96f10fd814f6", "Assistant", "ASSISTANT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27fc9915-b334-43ee-a2d5-20de61f1b783");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "589d7fa1-85d0-4cf0-90f9-9aae22abcefd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58c2f618-3224-43a2-9592-8777f1cea718");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbd82b69-3c4d-459d-89e1-33ed516d4c4d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9efd1c0-1208-4d5b-a6ac-19ad01656f7b");

            migrationBuilder.AlterColumn<string>(
                name: "NepaliModificationDate",
                table: "DepositSchemes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NepaliModificationDate",
                table: "DepositAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "025fed80-e073-4d0d-b7d6-fdcfc8d8eea1", "d8d796cd-0877-4928-8268-c823f6acd133", "SuperAdmin", "SUPERADMIN" },
                    { "3aac356f-292c-42fd-a26d-e3cdda262b0e", "1ef524bf-6a97-4890-a4ef-67eb99d017c0", "SeniorAssistant", "SENIORASSISTANT" },
                    { "4acbdd75-a4b9-4444-8f78-86c713da3054", "2ce7bcf3-6cff-4a57-8131-401f98a958a8", "Officer", "OFFICER" },
                    { "81faa567-90a6-402d-898a-877ecb623a12", "54a01895-abc1-46dd-97c4-7c4b199a3cf7", "Assistant", "ASSISTANT" },
                    { "cfa33577-db62-455f-a52a-d55e4bdc9f93", "0b16d4f7-27c4-461f-89f1-995bccbe9012", "Marketing", "MARKETING" }
                });
        }
    }
}
