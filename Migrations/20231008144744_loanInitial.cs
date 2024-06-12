using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations
{
    /// <inheritdoc />
    public partial class loanInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWithDrawalAllowed",
                table: "DepositAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "LoanSchemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NepaliName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AliasCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    MinimumInterestRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    MaximumInterestRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    IsRevolving = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PenalInterest = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    InterestOnInterest = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    LoanInterestReceivable = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    OverDueInterest = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    AssetsAccountLedgerId = table.Column<int>(type: "int", nullable: false),
                    InterestAccountLedgerId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_LoanSchemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanSchemes_Ledgers_AssetsAccountLedgerId",
                        column: x => x.AssetsAccountLedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LoanSchemes_Ledgers_InterestAccountLedgerId",
                        column: x => x.InterestAccountLedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanSchemes_AliasCode",
                table: "LoanSchemes",
                column: "AliasCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanSchemes_AssetsAccountLedgerId",
                table: "LoanSchemes",
                column: "AssetsAccountLedgerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanSchemes_InterestAccountLedgerId",
                table: "LoanSchemes",
                column: "InterestAccountLedgerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanSchemes_Name",
                table: "LoanSchemes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanSchemes");

            migrationBuilder.DropColumn(
                name: "IsWithDrawalAllowed",
                table: "DepositAccounts");
        }
    }
}
