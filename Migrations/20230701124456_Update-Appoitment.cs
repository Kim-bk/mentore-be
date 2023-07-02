using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class UpdateAppoitment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Appointment",
                newName: "VerifiedCode");

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Appointment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LinkGoogleMeet",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MenteeId",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "LinkGoogleMeet",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "MenteeId",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "VerifiedCode",
                table: "Appointment",
                newName: "AccountId");
        }
    }
}
