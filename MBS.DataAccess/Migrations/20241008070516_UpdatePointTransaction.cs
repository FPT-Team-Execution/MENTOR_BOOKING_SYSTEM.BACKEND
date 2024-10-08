using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MBS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePointTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PointTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PointTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "PointTransactions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "PointTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kind",
                table: "PointTransactions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RemainBalance",
                table: "PointTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "PointTransactions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kind",
                table: "PointTransactions");

            migrationBuilder.DropColumn(
                name: "RemainBalance",
                table: "PointTransactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PointTransactions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "PointTransactions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PointTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PointTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "PointTransactions",
                type: "datetime2",
                nullable: true);
        }
    }
}
