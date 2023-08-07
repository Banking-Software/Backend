using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MicroFinance.Migrations.Application
{
    /// <inheritdoc />
    public partial class SuperAdmin1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20d8ab57-09af-4b0a-a1f8-0acdbe96317d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f00db07-7542-432a-9a92-50cd7bd2720b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41db8274-a229-464b-a5fd-4cd587c9494f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96fa0a56-8860-4e7c-89f8-7e73ae17c551");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca92a923-ce52-483a-81a3-303f52e6884c");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentTax",
                table: "CompanyDetails",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,2)",
                oldPrecision: 2,
                oldScale: 2);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0250582a-5487-4cae-b739-e5593ddc3c63", "8e1b5ce1-0259-4204-b0fb-0452f73714c1", "SeniorAssistant", "SENIORASSISTANT" },
                    { "6633f3a7-71ad-4e46-88e4-2bbbb3056912", "700ee777-e506-4eb9-90f1-e183ef676b3e", "SuperAdmin", "SUPERADMIN" },
                    { "81160b3e-5cd6-4cf2-98a8-c44cfd1fc110", "cdb8cee1-ad38-4cc0-b14a-304284f30346", "Marketing", "MARKETING" },
                    { "9450ceda-6e29-4cbd-bd4d-9cf84a410809", "74929d42-01de-4bbf-9c11-5dd7844c4c40", "Assistant", "ASSISTANT" },
                    { "d75e2211-2f63-4957-887b-6defb5be787f", "7592edb9-964e-47a3-a058-026c79e5d2f4", "Officer", "OFFICER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0250582a-5487-4cae-b739-e5593ddc3c63");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6633f3a7-71ad-4e46-88e4-2bbbb3056912");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81160b3e-5cd6-4cf2-98a8-c44cfd1fc110");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9450ceda-6e29-4cbd-bd4d-9cf84a410809");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d75e2211-2f63-4957-887b-6defb5be787f");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentTax",
                table: "CompanyDetails",
                type: "decimal(2,2)",
                precision: 2,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "20d8ab57-09af-4b0a-a1f8-0acdbe96317d", "8042a823-f335-4204-be8e-12af735a03b2", "SuperAdmin", "SUPERADMIN" },
                    { "2f00db07-7542-432a-9a92-50cd7bd2720b", "4f5fe61f-1eb1-4a70-acae-0a32a5037cef", "Assistant", "ASSISTANT" },
                    { "41db8274-a229-464b-a5fd-4cd587c9494f", "4ad0430b-b360-4f88-b301-e81696246095", "Marketing", "MARKETING" },
                    { "96fa0a56-8860-4e7c-89f8-7e73ae17c551", "365ed5c7-c921-43d7-be78-a66eb71dc8cf", "SeniorAssistant", "SENIORASSISTANT" },
                    { "ca92a923-ce52-483a-81a3-303f52e6884c", "9d5e729e-9c5c-4e09-981e-de075b3cdcfc", "Officer", "OFFICER" }
                });
        }
    }
}
