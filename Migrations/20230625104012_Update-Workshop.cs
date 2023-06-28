using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class UpdateWorkshop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Anttendees",
                table: "Workshop",
                newName: "Participated");

            migrationBuilder.AddColumn<int>(
                name: "Attendees",
                table: "Workshop",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attendees",
                table: "Workshop");

            migrationBuilder.RenameColumn(
                name: "Participated",
                table: "Workshop",
                newName: "Anttendees");
        }
    }
}
