using Microsoft.EntityFrameworkCore.Migrations;

namespace WhenINeedToWork.Services.Migrations
{
    public partial class EventLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Event",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Event");
        }
    }
}
