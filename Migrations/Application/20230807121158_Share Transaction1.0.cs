using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations.Application
{
    /// <inheritdoc />
    public partial class ShareTransaction10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositAccountTransactions_BankSetups_BankDetailId",
                table: "DepositAccountTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareAccount_Clients_ClientId",
                table: "ShareAccount");

            migrationBuilder.DropIndex(
                name: "IX_DepositAccountTransactions_BankDetailId",
                table: "DepositAccountTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareAccount",
                table: "ShareAccount");

            migrationBuilder.DropColumn(
                name: "BankChequeNumber",
                table: "DepositAccountTransactions");

            migrationBuilder.DropColumn(
                name: "BankDetailId",
                table: "DepositAccountTransactions");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "DepositAccountTransactions");

            migrationBuilder.RenameTable(
                name: "ShareAccount",
                newName: "ShareAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_ShareAccount_ClientId",
                table: "ShareAccounts",
                newName: "IX_ShareAccounts_ClientId");

            migrationBuilder.AddColumn<string>(
                name: "BankChequeNumber",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankDetailId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId1",
                table: "ShareAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareAccounts",
                table: "ShareAccounts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ShareKittas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceOfOneKitta = table.Column<int>(type: "int", nullable: false),
                    CurrentKitta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareKittas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShareTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShareTransactionType = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    ShareCertificateNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    ShareAccountId = table.Column<int>(type: "int", nullable: false),
                    TransferToDepositAccountId = table.Column<int>(type: "int", nullable: true),
                    PaymentDepositAccountId = table.Column<int>(type: "int", nullable: true),
                    BalanceAfterTransaction = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShareTransactions_DepositAccounts_PaymentDepositAccountId",
                        column: x => x.PaymentDepositAccountId,
                        principalTable: "DepositAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShareTransactions_DepositAccounts_TransferToDepositAccountId",
                        column: x => x.TransferToDepositAccountId,
                        principalTable: "DepositAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShareTransactions_ShareAccounts_ShareAccountId",
                        column: x => x.ShareAccountId,
                        principalTable: "ShareAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShareTransactions_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BankDetailId",
                table: "Transactions",
                column: "BankDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAccounts_ClientId1",
                table: "ShareAccounts",
                column: "ClientId1",
                unique: true,
                filter: "[ClientId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ShareTransactions_PaymentDepositAccountId",
                table: "ShareTransactions",
                column: "PaymentDepositAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareTransactions_ShareAccountId",
                table: "ShareTransactions",
                column: "ShareAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareTransactions_TransactionId",
                table: "ShareTransactions",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShareTransactions_TransferToDepositAccountId",
                table: "ShareTransactions",
                column: "TransferToDepositAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareAccounts_Clients_ClientId",
                table: "ShareAccounts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareAccounts_Clients_ClientId1",
                table: "ShareAccounts",
                column: "ClientId1",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankSetups_BankDetailId",
                table: "Transactions",
                column: "BankDetailId",
                principalTable: "BankSetups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareAccounts_Clients_ClientId",
                table: "ShareAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareAccounts_Clients_ClientId1",
                table: "ShareAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankSetups_BankDetailId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "ShareKittas");

            migrationBuilder.DropTable(
                name: "ShareTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BankDetailId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareAccounts",
                table: "ShareAccounts");

            migrationBuilder.DropIndex(
                name: "IX_ShareAccounts_ClientId1",
                table: "ShareAccounts");

            migrationBuilder.DropColumn(
                name: "BankChequeNumber",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BankDetailId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ClientId1",
                table: "ShareAccounts");

            migrationBuilder.RenameTable(
                name: "ShareAccounts",
                newName: "ShareAccount");

            migrationBuilder.RenameIndex(
                name: "IX_ShareAccounts_ClientId",
                table: "ShareAccount",
                newName: "IX_ShareAccount_ClientId");

            migrationBuilder.AddColumn<string>(
                name: "BankChequeNumber",
                table: "DepositAccountTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankDetailId",
                table: "DepositAccountTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "DepositAccountTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareAccount",
                table: "ShareAccount",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccountTransactions_BankDetailId",
                table: "DepositAccountTransactions",
                column: "BankDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepositAccountTransactions_BankSetups_BankDetailId",
                table: "DepositAccountTransactions",
                column: "BankDetailId",
                principalTable: "BankSetups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareAccount_Clients_ClientId",
                table: "ShareAccount",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
