using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class DeleteUnused : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_UserGroup_UserGroupId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityField_Field_FieldId",
                table: "EntityField");

            migrationBuilder.DropTable(
                name: "Counter");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropIndex(
                name: "IX_EntityField_FieldId",
                table: "EntityField");

            migrationBuilder.DropIndex(
                name: "IX_Account_UserGroupId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "EntityField");

            migrationBuilder.AlterColumn<string>(
                name: "UserGroupId",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FieldId",
                table: "EntityField",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserGroupId",
                table: "Account",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Counter",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityField_FieldId",
                table: "EntityField",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserGroupId",
                table: "Account",
                column: "UserGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_UserGroup_UserGroupId",
                table: "Account",
                column: "UserGroupId",
                principalTable: "UserGroup",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EntityField_Field_FieldId",
                table: "EntityField",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "Id");
        }
    }
}
