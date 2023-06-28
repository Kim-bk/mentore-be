using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class UpdateWorkshopPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Invitation",
                table: "Workshop");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "Workshop");

            migrationBuilder.AddColumn<string>(
                name: "InvitationCode",
                table: "UserWorkshop",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitationCode",
                table: "UserWorkshop");

            migrationBuilder.AddColumn<string>(
                name: "Invitation",
                table: "Workshop",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "Workshop",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
