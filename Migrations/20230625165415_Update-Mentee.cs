using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class UpdateMentee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mentor_Account_AccountId",
                table: "Mentor");

            migrationBuilder.DropForeignKey(
                name: "FK_Mentor_Location_LocationId1",
                table: "Mentor");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "BankType");

            migrationBuilder.DropIndex(
                name: "IX_Mentor_AccountId",
                table: "Mentor");

            migrationBuilder.DropIndex(
                name: "IX_Mentor_LocationId1",
                table: "Mentor");

            migrationBuilder.DropColumn(
                name: "LocationId1",
                table: "Mentor");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Wallet",
                table: "Account");

            migrationBuilder.AddColumn<bool>(
                name: "IsActived",
                table: "UserWorkshop",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "LocationId",
                table: "Mentor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Mentor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Mentee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Mentee",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActived",
                table: "UserWorkshop");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Mentee");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Mentee");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Mentor",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Mentor",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationId1",
                table: "Mentor",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Wallet",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BankType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BankTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    StartedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bank_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bank_BankType_BankTypeId",
                        column: x => x.BankTypeId,
                        principalTable: "BankType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mentor_AccountId",
                table: "Mentor",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Mentor_LocationId1",
                table: "Mentor",
                column: "LocationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Bank_AccountId",
                table: "Bank",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Bank_BankTypeId",
                table: "Bank",
                column: "BankTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mentor_Account_AccountId",
                table: "Mentor",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mentor_Location_LocationId1",
                table: "Mentor",
                column: "LocationId1",
                principalTable: "Location",
                principalColumn: "Id");
        }
    }
}
