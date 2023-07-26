using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations.Application
{
    /// <inheritdoc />
    public partial class Transaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_DepositAccounts_DepositAccountId",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "DepositTransaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_DepositAccountId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "DepositAccountId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transactions");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Transactions",
                newName: "RealWorldCreationDate");

            migrationBuilder.AddColumn<string>(
                name: "AmountInWords",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BranchCode",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompanyCalendarCreationDate",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CompanyCalendarModificationDate",
                table: "Transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifierBranchCode",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifierId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RealWorldModificationDate",
                table: "Transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TransactionAmount",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "VoucherNumber",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DepositAccountTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    DepositAccountId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<int>(type: "int", precision: 18, scale: 4, nullable: false),
                    WithDrawalType = table.Column<int>(type: "int", nullable: true),
                    WithDrawalChequeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    BankDetailId = table.Column<int>(type: "int", nullable: true),
                    BankChequeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BalanceAfterTransaction = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositAccountTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositAccountTransactions_BankSetups_BankDetailId",
                        column: x => x.BankDetailId,
                        principalTable: "BankSetups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepositAccountTransactions_DepositAccounts_DepositAccountId",
                        column: x => x.DepositAccountId,
                        principalTable: "DepositAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepositAccountTransactions_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LedgerTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    LedgerId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BalanceAfterTransaction = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LedgerTransactions_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LedgerTransactions_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubLedgerTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    SubLedgerId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BalanceAfterTransaction = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubLedgerTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubLedgerTransactions_SubLedgers_SubLedgerId",
                        column: x => x.SubLedgerId,
                        principalTable: "SubLedgers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubLedgerTransactions_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_VoucherNumber",
                table: "Transactions",
                column: "VoucherNumber",
                unique: true,
                filter: "[VoucherNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccountTransactions_BankDetailId",
                table: "DepositAccountTransactions",
                column: "BankDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccountTransactions_DepositAccountId",
                table: "DepositAccountTransactions",
                column: "DepositAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccountTransactions_Id",
                table: "DepositAccountTransactions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccountTransactions_TransactionId",
                table: "DepositAccountTransactions",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LedgerTransactions_LedgerId",
                table: "LedgerTransactions",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerTransactions_TransactionId",
                table: "LedgerTransactions",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubLedgerTransactions_SubLedgerId",
                table: "SubLedgerTransactions",
                column: "SubLedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_SubLedgerTransactions_TransactionId",
                table: "SubLedgerTransactions",
                column: "TransactionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepositAccountTransactions");

            migrationBuilder.DropTable(
                name: "LedgerTransactions");

            migrationBuilder.DropTable(
                name: "SubLedgerTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_VoucherNumber",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AmountInWords",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BranchCode",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CompanyCalendarCreationDate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CompanyCalendarModificationDate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ModifierBranchCode",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RealWorldModificationDate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionAmount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "VoucherNumber",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transaction");

            migrationBuilder.RenameColumn(
                name: "RealWorldCreationDate",
                table: "Transaction",
                newName: "CreatedOn");

            migrationBuilder.AddColumn<int>(
                name: "DepositAccountId",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DepositTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    Employee = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningCharge = table.Column<int>(type: "int", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmountAfterTransaction = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TransactionAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositTransaction_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transaction",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DepositAccountId",
                table: "Transaction",
                column: "DepositAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositTransaction_TransactionId",
                table: "DepositTransaction",
                column: "TransactionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_DepositAccounts_DepositAccountId",
                table: "Transaction",
                column: "DepositAccountId",
                principalTable: "DepositAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
