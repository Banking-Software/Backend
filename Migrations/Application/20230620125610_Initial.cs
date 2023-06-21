using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations.Application
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientAccountTypeInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAccountTypeInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientAddressInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermanentVdcMunicipality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermanentVdcMunicipalityNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentToleVillage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentToleVillageNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentWardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermanentWardNumberNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentDistrict = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermanentDistrictNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemporaryVdcMunicipality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporaryVdcMunicipalityNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporaryToleVillage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporaryToleVillageNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporaryWardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporaryWardNumberNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporaryDistrict = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporaryDistrictNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporaryState = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAddressInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientContactInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MobileNumber1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelephoneNumber1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelephoneNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientContactInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientFamilyInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MotherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotherNameNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherNameNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrandFatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrandFatherNameNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpouseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpouseOccupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameOfSons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameOfDaughters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherInLaw = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherInLaw = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFamilyInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NepaliName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cast = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CitizenshipNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssueDistrict = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PanNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalityIdStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VotingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherInfo2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncomeSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountPurposeNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IfMemberOfOtherParty = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientKYMTypeInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientKYMTypeInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientNomineeInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NepaliName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Relation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NepaliRelation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntroducedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientNomineeInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientShareTypeInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientShareTypeInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientTypeInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTypeInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DebitOrCredits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebitOrCredits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostingSchemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostingSchemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NepaliName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Schedule = table.Column<int>(type: "int", nullable: true),
                    CharKhataNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupTypes_AccountTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsModified = table.Column<bool>(type: "bit", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationCount = table.Column<int>(type: "int", nullable: true),
                    ClientInfoId = table.Column<int>(type: "int", nullable: false),
                    ClientAddressInfoId = table.Column<int>(type: "int", nullable: false),
                    ClientFamilyInfoId = table.Column<int>(type: "int", nullable: false),
                    ClientContactInfoId = table.Column<int>(type: "int", nullable: true),
                    ClientNomineeInfoId = table.Column<int>(type: "int", nullable: true),
                    ClientKYMTypeInfoId = table.Column<int>(type: "int", nullable: true),
                    ClientAccountTypeInfoId = table.Column<int>(type: "int", nullable: false),
                    ClientTypeInfoId = table.Column<int>(type: "int", nullable: false),
                    ClientShareTypeInfoId = table.Column<int>(type: "int", nullable: true),
                    IsKYMUpdated = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_ClientAddressInfos_ClientAddressInfoId",
                        column: x => x.ClientAddressInfoId,
                        principalTable: "ClientAddressInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clients_ClientContactInfos_ClientContactInfoId",
                        column: x => x.ClientContactInfoId,
                        principalTable: "ClientContactInfos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clients_ClientFamilyInfos_ClientFamilyInfoId",
                        column: x => x.ClientFamilyInfoId,
                        principalTable: "ClientFamilyInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clients_ClientInfos_ClientInfoId",
                        column: x => x.ClientInfoId,
                        principalTable: "ClientInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clients_ClientNomineeInfos_ClientNomineeInfoId",
                        column: x => x.ClientNomineeInfoId,
                        principalTable: "ClientNomineeInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Ledgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    GroupTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NepaliName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepreciationRate = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    HisabNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSubLedgerActive = table.Column<bool>(type: "bit", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    IsBank = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ledgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ledgers_GroupTypes_GroupTypeId",
                        column: x => x.GroupTypeId,
                        principalTable: "GroupTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankSetups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LedgerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NepaliName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankBranch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankTypeId = table.Column<int>(type: "int", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalInterestBalance = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankSetups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankSetups_BankTypes_BankTypeId",
                        column: x => x.BankTypeId,
                        principalTable: "BankTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankSetups_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepositSchemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinimumBalance = table.Column<int>(type: "int", nullable: false),
                    InterestRateOnMinimumBalance = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MinimumInterestRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MaximumInterestRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Calculation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostingSchemeId = table.Column<int>(type: "int", nullable: false),
                    ClosingCharge = table.Column<int>(type: "int", nullable: true),
                    LedgerAsLiabilityAccountId = table.Column<int>(type: "int", nullable: false),
                    LedgerAsInterestAccountId = table.Column<int>(type: "int", nullable: false),
                    FineAmount = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositSchemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositSchemes_Ledgers_LedgerAsInterestAccountId",
                        column: x => x.LedgerAsInterestAccountId,
                        principalTable: "Ledgers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepositSchemes_Ledgers_LedgerAsLiabilityAccountId",
                        column: x => x.LedgerAsLiabilityAccountId,
                        principalTable: "Ledgers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepositSchemes_PostingSchemes_PostingSchemeId",
                        column: x => x.PostingSchemeId,
                        principalTable: "PostingSchemes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubLedgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LedgerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubLedgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubLedgers_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepositAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositSchemeId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: true),
                    PeriodType = table.Column<int>(type: "int", nullable: true),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    JointClientId = table.Column<int>(type: "int", nullable: true),
                    MatureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MinimumBalance = table.Column<int>(type: "int", nullable: false),
                    PrincipalAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    InterestAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ReferredBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InterestPostingAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatureInterestPostingAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsSMSServiceActive = table.Column<bool>(type: "bit", nullable: false),
                    DailyDepositAmount = table.Column<int>(type: "int", nullable: true),
                    TotalDepositDay = table.Column<int>(type: "int", nullable: true),
                    TotalDepositAmount = table.Column<int>(type: "int", nullable: true),
                    TotalReturnAmount = table.Column<int>(type: "int", nullable: true),
                    TotalInterestAmount = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositAccounts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepositAccounts_Clients_JointClientId",
                        column: x => x.JointClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepositAccounts_DepositSchemes_DepositSchemeId",
                        column: x => x.DepositSchemeId,
                        principalTable: "DepositSchemes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FlexibleInterestRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositSchemeId = table.Column<int>(type: "int", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromAmount = table.Column<int>(type: "int", nullable: false),
                    ToAmount = table.Column<int>(type: "int", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlexibleInterestRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlexibleInterestRates_DepositSchemes_DepositSchemeId",
                        column: x => x.DepositSchemeId,
                        principalTable: "DepositSchemes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositAccountId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_DepositAccounts_DepositAccountId",
                        column: x => x.DepositAccountId,
                        principalTable: "DepositAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DepositTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    TransactionAmount = table.Column<int>(type: "int", nullable: false),
                    TotalAmountAfterTransaction = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Employee = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningCharge = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_AccountTypes_Name",
                table: "AccountTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankSetups_BankTypeId",
                table: "BankSetups",
                column: "BankTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BankSetups_LedgerId",
                table: "BankSetups",
                column: "LedgerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankSetups_LedgerId_Name",
                table: "BankSetups",
                columns: new[] { "LedgerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_BranchCode",
                table: "Branches",
                column: "BranchCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientAccountTypeInfos_Type",
                table: "ClientAccountTypeInfos",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientKYMTypeInfos_Type",
                table: "ClientKYMTypeInfos",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientAddressInfoId",
                table: "Clients",
                column: "ClientAddressInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientContactInfoId",
                table: "Clients",
                column: "ClientContactInfoId",
                unique: true,
                filter: "[ClientContactInfoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientFamilyInfoId",
                table: "Clients",
                column: "ClientFamilyInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientInfoId",
                table: "Clients",
                column: "ClientInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientNomineeInfoId",
                table: "Clients",
                column: "ClientNomineeInfoId",
                unique: true,
                filter: "[ClientNomineeInfoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClientShareTypeInfos_Type",
                table: "ClientShareTypeInfos",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientTypeInfos_Type",
                table: "ClientTypeInfos",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccounts_AccountNumber",
                table: "DepositAccounts",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccounts_ClientId",
                table: "DepositAccounts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccounts_DepositSchemeId",
                table: "DepositAccounts",
                column: "DepositSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccounts_JointClientId",
                table: "DepositAccounts",
                column: "JointClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositSchemes_LedgerAsInterestAccountId",
                table: "DepositSchemes",
                column: "LedgerAsInterestAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositSchemes_LedgerAsLiabilityAccountId",
                table: "DepositSchemes",
                column: "LedgerAsLiabilityAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositSchemes_Name",
                table: "DepositSchemes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositSchemes_PostingSchemeId",
                table: "DepositSchemes",
                column: "PostingSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositTransaction_TransactionId",
                table: "DepositTransaction",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlexibleInterestRates_DepositSchemeId",
                table: "FlexibleInterestRates",
                column: "DepositSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTypes_AccountTypeId_Name",
                table: "GroupTypes",
                columns: new[] { "AccountTypeId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_GroupTypeId_Name",
                table: "Ledgers",
                columns: new[] { "GroupTypeId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostingSchemes_Name",
                table: "PostingSchemes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubLedgers_LedgerId",
                table: "SubLedgers",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_SubLedgers_Name_LedgerId",
                table: "SubLedgers",
                columns: new[] { "Name", "LedgerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DepositAccountId",
                table: "Transaction",
                column: "DepositAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankSetups");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "ClientAccountTypeInfos");

            migrationBuilder.DropTable(
                name: "ClientKYMTypeInfos");

            migrationBuilder.DropTable(
                name: "ClientShareTypeInfos");

            migrationBuilder.DropTable(
                name: "ClientTypeInfos");

            migrationBuilder.DropTable(
                name: "CompanyDetails");

            migrationBuilder.DropTable(
                name: "DebitOrCredits");

            migrationBuilder.DropTable(
                name: "DepositTransaction");

            migrationBuilder.DropTable(
                name: "FlexibleInterestRates");

            migrationBuilder.DropTable(
                name: "SubLedgers");

            migrationBuilder.DropTable(
                name: "BankTypes");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "DepositAccounts");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "DepositSchemes");

            migrationBuilder.DropTable(
                name: "ClientAddressInfos");

            migrationBuilder.DropTable(
                name: "ClientContactInfos");

            migrationBuilder.DropTable(
                name: "ClientFamilyInfos");

            migrationBuilder.DropTable(
                name: "ClientInfos");

            migrationBuilder.DropTable(
                name: "ClientNomineeInfos");

            migrationBuilder.DropTable(
                name: "Ledgers");

            migrationBuilder.DropTable(
                name: "PostingSchemes");

            migrationBuilder.DropTable(
                name: "GroupTypes");

            migrationBuilder.DropTable(
                name: "AccountTypes");
        }
    }
}
