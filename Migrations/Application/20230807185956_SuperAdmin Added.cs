using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MicroFinance.Migrations.Application
{
    /// <inheritdoc />
    public partial class SuperAdminAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "71eab024-cf51-4c0b-93d9-237a75d09c83");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a155c930-7463-45f0-a59f-0b2d191cd524");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea4dbc99-cbec-41a7-8f38-d09b4f72906e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec63ff51-f9a5-4af7-8d3d-ebd10f7ae6a5");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "71eab024-cf51-4c0b-93d9-237a75d09c83", "e79a0af6-b059-4ad6-82ad-20b572d857d2", "Marketing", "MARKETING" },
                    { "a155c930-7463-45f0-a59f-0b2d191cd524", "ff5bed60-5c81-4fd0-a185-228452d20401", "Assistant", "ASSISTANT" },
                    { "ea4dbc99-cbec-41a7-8f38-d09b4f72906e", "0b8629a7-3136-4dae-b194-f8db37b52063", "SeniorAssistant", "SENIORASSISTANT" },
                    { "ec63ff51-f9a5-4af7-8d3d-ebd10f7ae6a5", "148744c1-db60-4594-b395-7975de7b13cc", "Officer", "OFFICER" }
                });
        }
    }
}
