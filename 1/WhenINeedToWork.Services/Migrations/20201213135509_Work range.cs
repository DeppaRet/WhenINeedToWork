using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WhenINeedToWork.Services.Migrations
{
    public partial class Workrange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "firsWorkDay",
                table: "Calendars",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "flexAmount",
                table: "Calendars",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "workAmount",
                table: "Calendars",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "firsWorkDay",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "flexAmount",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "workAmount",
                table: "Calendars");
        }
    }
}
