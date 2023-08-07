using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
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
                    CompanyValidityStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyValidityEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogoFileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LogoFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoFileType = table.Column<int>(type: "int", nullable: true),
                    CurrentTax = table.Column<decimal>(type: "decimal(2,2)", precision: 2, scale: 2, nullable: false)
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
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfJoining = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenderCode = table.Column<int>(type: "int", nullable: true),
                    PFAllowed = table.Column<bool>(type: "bit", nullable: true),
                    SalaryPostingAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProvidentPostingAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalaryAmount = table.Column<double>(type: "float", nullable: true),
                    Tax = table.Column<float>(type: "real", nullable: true),
                    Facilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherFacilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PANNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePicFileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ProfilePicFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePicFileType = table.Column<int>(type: "int", nullable: true),
                    CitizenShipFileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CitizenShipFileType = table.Column<int>(type: "int", nullable: true),
                    CitizenShipFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureFileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    SignatureFileType = table.Column<int>(type: "int", nullable: true),
                    SignatureFileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepositLimit = table.Column<double>(type: "float", nullable: true),
                    LoanLimit = table.Column<double>(type: "float", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
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
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
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
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
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
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
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
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AmountInWords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: true),
                    BankDetailId = table.Column<int>(type: "int", nullable: true),
                    BankChequeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RealWorldCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyCalendarCreationDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierBranchCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealWorldModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyCalendarModificationDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_BankSetups_BankDetailId",
                        column: x => x.BankDetailId,
                        principalTable: "BankSetups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShareAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CurrentShareBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShareAccounts_Clients_ClientId",
                        column: x => x.ClientId,
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
                    CompanyCalendarCreationDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierBranchCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealWorldModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyCalendarModificationDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "DepositAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositSchemeId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OpeningDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: false),
                    PeriodType = table.Column<int>(type: "int", nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    MatureDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    SignatureFileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    SignatureFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureFileType = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    InterestPostingAccountNumberId = table.Column<int>(type: "int", nullable: true),
                    MatureInterestPostingAccountNumberId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RealWorldCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyCalendarCreationDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierBranchCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealWorldModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyCalendarModificationDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    CollectedByEmployeeId = table.Column<int>(type: "int", nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BalanceAfterTransaction = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositAccountTransactions", x => x.Id);
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
                name: "JointAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositAccountId = table.Column<int>(type: "int", nullable: false),
                    JointClientId = table.Column<int>(type: "int", nullable: false),
                    RealWorldStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RealWorldEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyCalendarStartDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyCalendarEndDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_AccountTypes_Name",
                table: "AccountTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeeId",
                table: "AspNetUsers",
                column: "EmployeeId",
                unique: true,
                filter: "[EmployeeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

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
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
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
                name: "IX_LedgerTransactions_LedgerId",
                table: "LedgerTransactions",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerTransactions_TransactionId",
                table: "LedgerTransactions",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShareAccounts_ClientId",
                table: "ShareAccounts",
                column: "ClientId",
                unique: true);

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
                name: "IX_SubLedgerTransactions_SubLedgerId",
                table: "SubLedgerTransactions",
                column: "SubLedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_SubLedgerTransactions_TransactionId",
                table: "SubLedgerTransactions",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BankDetailId",
                table: "Transactions",
                column: "BankDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_VoucherNumber",
                table: "Transactions",
                column: "VoucherNumber",
                unique: true,
                filter: "[VoucherNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

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
                name: "DepositAccountTransactions");

            migrationBuilder.DropTable(
                name: "DepositAccountTypes");

            migrationBuilder.DropTable(
                name: "DepositPostingSchemes");

            migrationBuilder.DropTable(
                name: "DepositSchemeCalculationTypes");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "FlexibleInterestRates");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "JointAccounts");

            migrationBuilder.DropTable(
                name: "LedgerTransactions");

            migrationBuilder.DropTable(
                name: "MaritalStatuses");

            migrationBuilder.DropTable(
                name: "ShareKittas");

            migrationBuilder.DropTable(
                name: "ShareTransactions");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "SubLedgerTransactions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DepositAccounts");

            migrationBuilder.DropTable(
                name: "ShareAccounts");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "DepositSchemes");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "BankSetups");

            migrationBuilder.DropTable(
                name: "SubLedgers");

            migrationBuilder.DropTable(
                name: "ClientGroups");

            migrationBuilder.DropTable(
                name: "ClientKYMTypes");

            migrationBuilder.DropTable(
                name: "ClientTypes");

            migrationBuilder.DropTable(
                name: "ClientUnits");

            migrationBuilder.DropTable(
                name: "BankTypes");

            migrationBuilder.DropTable(
                name: "Ledgers");

            migrationBuilder.DropTable(
                name: "GroupTypes");

            migrationBuilder.DropTable(
                name: "AccountTypes");
        }
    }
}
