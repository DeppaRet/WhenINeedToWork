using Microsoft.EntityFrameworkCore.Migrations;

namespace WhenINeedToWork.Services.Migrations
{
    public partial class EventType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTemp",
                table: "Event",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTemp",
                table: "Event");
        }
    }
}
