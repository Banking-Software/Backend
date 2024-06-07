using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations
{
    /// <inheritdoc />
    public partial class loanAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoanAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanSchemeId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Sector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    InterestType = table.Column<int>(type: "int", nullable: false),
                    PeriodType = table.Column<int>(type: "int", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: false),
                    MatureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    LoanLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReferredByEmployeeId = table.Column<int>(type: "int", nullable: false),
                    WithDrawalBlockedDepositAccountIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Shakshi1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Shakshi2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Jamani1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Jamani2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurjaNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KittaNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RokkaNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VechileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxPaidDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LandArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RokkaMiti = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VatNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PanNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PolicyNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedDocument = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UploadedDocumentType = table.Column<int>(type: "int", nullable: true),
                    UploadedDocumentFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduleType = table.Column<int>(type: "int", nullable: true),
                    InterestPaymentType = table.Column<int>(type: "int", nullable: true),
                    GracePeriod = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnglishCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NepaliCreationDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RealWorldCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierBranchCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NepaliModificationDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealWorldModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanAccounts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LoanAccounts_LoanSchemes_LoanSchemeId",
                        column: x => x.LoanSchemeId,
                        principalTable: "LoanSchemes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanAccounts_AccountNumber",
                table: "LoanAccounts",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanAccounts_ClientId",
                table: "LoanAccounts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanAccounts_LoanSchemeId",
                table: "LoanAccounts",
                column: "LoanSchemeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanAccounts");
        }
    }
}
