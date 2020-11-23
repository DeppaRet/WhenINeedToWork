using Microsoft.EntityFrameworkCore.Migrations;

namespace WhenINeedToWork.Services.Migrations
{
    public partial class FK_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calendars_Users_Userid",
                table: "Calendars");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Calendars_Calendarid",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_Calendarid",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Calendars_Userid",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "Calendarid",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Userid",
                table: "Calendars");

            migrationBuilder.AlterColumn<int>(
                name: "Calendar_id",
                table: "Event",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "User_id",
                table: "Calendars",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Calendar_id",
                table: "Event",
                column: "Calendar_id");

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_User_id",
                table: "Calendars",
                column: "User_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Calendars_Users_User_id",
                table: "Calendars",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Calendars_Calendar_id",
                table: "Event",
                column: "Calendar_id",
                principalTable: "Calendars",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calendars_Users_User_id",
                table: "Calendars");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Calendars_Calendar_id",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_Calendar_id",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Calendars_User_id",
                table: "Calendars");

            migrationBuilder.AlterColumn<int>(
                name: "Calendar_id",
                table: "Event",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Calendarid",
                table: "Event",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "User_id",
                table: "Calendars",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Userid",
                table: "Calendars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_Calendarid",
                table: "Event",
                column: "Calendarid");

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_Userid",
                table: "Calendars",
                column: "Userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Calendars_Users_Userid",
                table: "Calendars",
                column: "Userid",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Calendars_Calendarid",
                table: "Event",
                column: "Calendarid",
                principalTable: "Calendars",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
