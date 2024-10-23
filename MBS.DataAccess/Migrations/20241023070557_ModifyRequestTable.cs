using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MBS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_CalendarEvents_CalendarEventId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "CalendarEventId",
                table: "Requests",
                newName: "MentorId");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_CalendarEventId",
                table: "Requests",
                newName: "IX_Requests_MentorId");

            migrationBuilder.AddColumn<DateTime>(
                name: "End",
                table: "Requests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Start",
                table: "Requests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Mentors_MentorId",
                table: "Requests",
                column: "MentorId",
                principalTable: "Mentors",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Mentors_MentorId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "End",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "MentorId",
                table: "Requests",
                newName: "CalendarEventId");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_MentorId",
                table: "Requests",
                newName: "IX_Requests_CalendarEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_CalendarEvents_CalendarEventId",
                table: "Requests",
                column: "CalendarEventId",
                principalTable: "CalendarEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
