using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroFinance.Migrations.Application
{
    /// <inheritdoc />
    public partial class BranchCode : Migration
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
                    Id = table.Column<int>(type: "int", nullable: false),
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
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    MonthName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfDay = table.Column<int>(type: "int", nullable: false),
                    RunningDay = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Casts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    NepaliName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Casts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientKYMTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientKYMTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyNameNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAddressNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PANNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstablishedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LogoFileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LogoFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoFileType = table.Column<int>(type: "int", nullable: true)
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
                name: "DepositAccountStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositAccountStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepositAccountTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositAccountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepositPostingSchemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositPostingSchemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepositSchemeCalculationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositSchemeCalculationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    NepaliName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    NepaliName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaritalStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    NepaliName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaritalStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    NepaliName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
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
                    CharKhataNumber = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                name: "Ledgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LedgerCode = table.Column<int>(type: "int", nullable: true),
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
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsKYMUpdated = table.Column<bool>(type: "bit", nullable: false),
                    IsShareAllowed = table.Column<bool>(type: "bit", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PermanentVdcMunicipality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentVdcMunicipalityNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentToleVillage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentToleVillageNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentWardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentWardNumberNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentDistrictCode = table.Column<int>(type: "int", nullable: true),
                    PermanentStateCode = table.Column<int>(type: "int", nullable: true),
                    TemporaryVdcMunicipality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporaryVdcMunicipalityNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporaryToleVillage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporaryToleVillageNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporaryWardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporaryWardNumberNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporaryDistrictCode = table.Column<int>(type: "int", nullable: true),
                    TemporaryStateCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientMobileNumber1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientMobileNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientTelephoneNumber1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientTelephoneNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientWebsite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientMotherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientMotherNameNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientFatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientFatherNameNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientGrandFatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientGrandFatherNameNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientSpouseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientSpouseOccupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientNameOfSons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientNameOfDaughters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientFatherInLaw = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientMotherInLaw = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientMiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientNepaliName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientCastCode = table.Column<int>(type: "int", nullable: true),
                    ClientGenderCode = table.Column<int>(type: "int", nullable: true),
                    ClientDateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientOccupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientCitizenshipNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientCitizenShipIssueDistrict = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientCitizenShipIssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientNationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientPanNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientEducationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientMartialStatusCode = table.Column<int>(type: "int", nullable: true),
                    ClientNationalityIdStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientVotingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientOtherInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientOtherInfo2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientIncomeSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientAccountPurposeNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientIfMemberOfOtherParty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomineeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomineeNepaliName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomineeRelation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomineeNepaliRelation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomineeIntroducedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomineeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomineeCitizenshipNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomineeContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientPhotoFileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ClientPhotoFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientPhotoFileType = table.Column<int>(type: "int", nullable: true),
                    ClientCitizenshipFileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ClientCitizenshipFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientCitizenshipFileType = table.Column<int>(type: "int", nullable: true),
                    ClientSignatureFileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ClientSignatureFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientSignatureFileType = table.Column<int>(type: "int", nullable: true),
                    NomineePhotoFileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    NomineePhotoFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomineePhotoFileType = table.Column<int>(type: "int", nullable: true),
                    ClientTypeId = table.Column<int>(type: "int", nullable: false),
                    ClientShareTypeInfoId = table.Column<int>(type: "int", nullable: true),
                    ClientGroupId = table.Column<int>(type: "int", nullable: true),
                    ClientUnitId = table.Column<int>(type: "int", nullable: true),
                    KYMTypeId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsModified = table.Column<bool>(type: "bit", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_ClientGroups_ClientGroupId",
                        column: x => x.ClientGroupId,
                        principalTable: "ClientGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clients_ClientKYMTypes_KYMTypeId",
                        column: x => x.KYMTypeId,
                        principalTable: "ClientKYMTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clients_ClientTypes_ClientTypeId",
                        column: x => x.ClientTypeId,
                        principalTable: "ClientTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clients_ClientUnits_ClientUnitId",
                        column: x => x.ClientUnitId,
                        principalTable: "ClientUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clients_Ledgers_ClientShareTypeInfoId",
                        column: x => x.ClientShareTypeInfoId,
                        principalTable: "Ledgers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubLedgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubLedgerCode = table.Column<int>(type: "int", nullable: true),
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
                name: "ShareAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<int>(type: "int", nullable: false),
                    CurrentShareBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentNumberOfKitta = table.Column<int>(type: "int", nullable: false),
                    StartOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShareAccounts_Clients_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DepositSchemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchemeName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchemeNameNepali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchemeTypeId = table.Column<int>(type: "int", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MinimumBalance = table.Column<int>(type: "int", nullable: false),
                    InterestRateOnMinimumBalance = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    MinimumInterestRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    MaximumInterestRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Calculation = table.Column<int>(type: "int", nullable: false),
                    PostingScheme = table.Column<int>(type: "int", nullable: false),
                    InterestSubLedgerId = table.Column<int>(type: "int", nullable: false),
                    DepositSubledgerId = table.Column<int>(type: "int", nullable: false),
                    TaxSubledgerId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RealWorldCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyCalendarCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierBranchCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealWorldModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyCalendarModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositSchemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositSchemes_Ledgers_SchemeTypeId",
                        column: x => x.SchemeTypeId,
                        principalTable: "Ledgers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepositSchemes_SubLedgers_DepositSubledgerId",
                        column: x => x.DepositSubledgerId,
                        principalTable: "SubLedgers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepositSchemes_SubLedgers_InterestSubLedgerId",
                        column: x => x.InterestSubLedgerId,
                        principalTable: "SubLedgers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepositSchemes_SubLedgers_TaxSubledgerId",
                        column: x => x.TaxSubledgerId,
                        principalTable: "SubLedgers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DepositAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositSchemeId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: false),
                    PeriodType = table.Column<int>(type: "int", nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    MatureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    PrincipalAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    InterestAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ReferredByEmployeeId = table.Column<int>(type: "int", nullable: false),
                    Relation = table.Column<int>(type: "int", nullable: true),
                    NomineeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsSMSServiceActive = table.Column<bool>(type: "bit", nullable: false),
                    ExpectedDailyDepositAmount = table.Column<int>(type: "int", nullable: true),
                    ExpectedTotalDepositDay = table.Column<int>(type: "int", nullable: true),
                    ExpectedTotalDepositAmount = table.Column<int>(type: "int", nullable: true),
                    ExpectedTotalReturnAmount = table.Column<int>(type: "int", nullable: true),
                    ExpectedTotalInterestAmount = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    InterestPostingAccountNumberId = table.Column<int>(type: "int", nullable: true),
                    MatureInterestPostingAccountNumberId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RealWorldCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyCalendarCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierBranchCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealWorldModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyCalendarModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                        name: "FK_DepositAccounts_DepositAccounts_InterestPostingAccountNumberId",
                        column: x => x.InterestPostingAccountNumberId,
                        principalTable: "DepositAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepositAccounts_DepositAccounts_MatureInterestPostingAccountNumberId",
                        column: x => x.MatureInterestPostingAccountNumberId,
                        principalTable: "DepositAccounts",
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
                name: "JointAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositAccountId = table.Column<int>(type: "int", nullable: false),
                    JointClientId = table.Column<int>(type: "int", nullable: false),
                    RealWorldStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RealWorldEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyCalendarStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyCalendarEndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JointAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JointAccounts_Clients_JointClientId",
                        column: x => x.JointClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JointAccounts_DepositAccounts_DepositAccountId",
                        column: x => x.DepositAccountId,
                        principalTable: "DepositAccounts",
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_Calendars_Year_Month",
                table: "Calendars",
                columns: new[] { "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientGroups_Code",
                table: "ClientGroups",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientKYMTypes_Type",
                table: "ClientKYMTypes",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientGroupId",
                table: "Clients",
                column: "ClientGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientId",
                table: "Clients",
                column: "ClientId",
                unique: true,
                filter: "[ClientId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientShareTypeInfoId",
                table: "Clients",
                column: "ClientShareTypeInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientTypeId",
                table: "Clients",
                column: "ClientTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientUnitId",
                table: "Clients",
                column: "ClientUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_KYMTypeId",
                table: "Clients",
                column: "KYMTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTypes_Type",
                table: "ClientTypes",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientUnits_Code",
                table: "ClientUnits",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccounts_AccountNumber",
                table: "DepositAccounts",
                column: "AccountNumber",
                unique: true,
                filter: "[AccountNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccounts_ClientId",
                table: "DepositAccounts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccounts_DepositSchemeId",
                table: "DepositAccounts",
                column: "DepositSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccounts_InterestPostingAccountNumberId",
                table: "DepositAccounts",
                column: "InterestPostingAccountNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccounts_MatureInterestPostingAccountNumberId",
                table: "DepositAccounts",
                column: "MatureInterestPostingAccountNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositSchemes_DepositSubledgerId",
                table: "DepositSchemes",
                column: "DepositSubledgerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositSchemes_InterestSubLedgerId",
                table: "DepositSchemes",
                column: "InterestSubLedgerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositSchemes_SchemeName",
                table: "DepositSchemes",
                column: "SchemeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositSchemes_SchemeTypeId",
                table: "DepositSchemes",
                column: "SchemeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositSchemes_Symbol",
                table: "DepositSchemes",
                column: "Symbol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositSchemes_TaxSubledgerId",
                table: "DepositSchemes",
                column: "TaxSubledgerId",
                unique: true);

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
                name: "IX_GroupTypes_CharKhataNumber",
                table: "GroupTypes",
                column: "CharKhataNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JointAccounts_DepositAccountId",
                table: "JointAccounts",
                column: "DepositAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JointAccounts_JointClientId",
                table: "JointAccounts",
                column: "JointClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_GroupTypeId_Name",
                table: "Ledgers",
                columns: new[] { "GroupTypeId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_LedgerCode",
                table: "Ledgers",
                column: "LedgerCode",
                unique: true,
                filter: "[LedgerCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAccounts_AccountNumber",
                table: "ShareAccounts",
                column: "AccountNumber");

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
                name: "IX_SubLedgers_SubLedgerCode",
                table: "SubLedgers",
                column: "SubLedgerCode",
                unique: true,
                filter: "[SubLedgerCode] IS NOT NULL");

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
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "Casts");

            migrationBuilder.DropTable(
                name: "CompanyDetails");

            migrationBuilder.DropTable(
                name: "DebitOrCredits");

            migrationBuilder.DropTable(
                name: "DepositAccountStatuses");

            migrationBuilder.DropTable(
                name: "DepositAccountTypes");

            migrationBuilder.DropTable(
                name: "DepositPostingSchemes");

            migrationBuilder.DropTable(
                name: "DepositSchemeCalculationTypes");

            migrationBuilder.DropTable(
                name: "DepositTransaction");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "FlexibleInterestRates");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "JointAccounts");

            migrationBuilder.DropTable(
                name: "MaritalStatuses");

            migrationBuilder.DropTable(
                name: "ShareAccounts");

            migrationBuilder.DropTable(
                name: "States");

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
                name: "ClientGroups");

            migrationBuilder.DropTable(
                name: "ClientKYMTypes");

            migrationBuilder.DropTable(
                name: "ClientTypes");

            migrationBuilder.DropTable(
                name: "ClientUnits");

            migrationBuilder.DropTable(
                name: "SubLedgers");

            migrationBuilder.DropTable(
                name: "Ledgers");

            migrationBuilder.DropTable(
                name: "GroupTypes");

            migrationBuilder.DropTable(
                name: "AccountTypes");
        }
    }
}
