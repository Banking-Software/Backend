using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MicroFinance.Migrations
{
    /// <inheritdoc />
    public partial class Initial11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "RealWorldModificationDate",
                table: "DepositSchemes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EnglishModificationDate",
                table: "DepositSchemes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RealWorldModificationDate",
                table: "DepositAccounts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EnglishModificationDate",
                table: "DepositAccounts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4ed7a3b5-32d0-49f5-8d8b-b3f4a0767db6", "68ee3cf1-052c-4512-a47a-3613d5be7f46", "SeniorAssistant", "SENIORASSISTANT" },
                    { "64c30543-93e4-4f77-b6ad-2b51207e16e5", "1f06c358-e0ca-4284-a772-9f5cd96ba045", "Marketing", "MARKETING" },
                    { "870b93e3-b883-43b4-a65d-3c95887253fb", "68060219-3e03-45e1-be74-362c1f00f4b6", "SuperAdmin", "SUPERADMIN" },
                    { "d28a4610-6c63-4f26-8601-224a6a8f2f15", "ef5baa18-68fd-4137-b4d8-f64a7da0bcec", "Assistant", "ASSISTANT" },
                    { "f14fd079-da32-4824-b5a4-6d95f4fd5324", "dd8c76fe-cdb3-4548-a977-5caa7e1a3f00", "Officer", "OFFICER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ed7a3b5-32d0-49f5-8d8b-b3f4a0767db6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64c30543-93e4-4f77-b6ad-2b51207e16e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "870b93e3-b883-43b4-a65d-3c95887253fb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d28a4610-6c63-4f26-8601-224a6a8f2f15");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f14fd079-da32-4824-b5a4-6d95f4fd5324");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RealWorldModificationDate",
                table: "DepositSchemes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EnglishModificationDate",
                table: "DepositSchemes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RealWorldModificationDate",
                table: "DepositAccounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EnglishModificationDate",
                table: "DepositAccounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

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
    }
}
