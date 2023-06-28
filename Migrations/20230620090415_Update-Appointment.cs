using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class UpdateAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "accountId",
                table: "Appointment",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Appointment",
                newName: "DateStart");

            migrationBuilder.AlterColumn<string>(
                name: "MentorId",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Detail",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeStart",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Detail",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "TimeStart",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Appointment",
                newName: "accountId");

            migrationBuilder.RenameColumn(
                name: "DateStart",
                table: "Appointment",
                newName: "StartTime");

            migrationBuilder.AlterColumn<int>(
                name: "MentorId",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Appointment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
