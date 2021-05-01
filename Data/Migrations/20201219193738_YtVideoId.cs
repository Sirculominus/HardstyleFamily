using Microsoft.EntityFrameworkCore.Migrations;

namespace HardstyleFamily.Data.Migrations
{
    public partial class YtVideoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YtVideoId",
                table: "Music",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YtVideoId",
                table: "Music");
        }
    }
}
