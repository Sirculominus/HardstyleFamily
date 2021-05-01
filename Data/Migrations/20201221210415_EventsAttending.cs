using Microsoft.EntityFrameworkCore.Migrations;

namespace HardstyleFamily.Data.Migrations
{
    public partial class EventsAttending : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventsAttending",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventsAttending",
                table: "AspNetUsers");
        }
    }
}
