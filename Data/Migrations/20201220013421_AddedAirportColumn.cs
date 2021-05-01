using Microsoft.EntityFrameworkCore.Migrations;

namespace HardstyleFamily.Data.Migrations
{
    public partial class AddedAirportColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Airport",
                table: "Events",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Airport",
                table: "Events");
        }
    }
}
