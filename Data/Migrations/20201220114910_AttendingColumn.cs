using Microsoft.EntityFrameworkCore.Migrations;

namespace HardstyleFamily.Data.Migrations
{
    public partial class AttendingColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttendingTotal",
                table: "Events",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendingTotal",
                table: "Events");
        }
    }
}
