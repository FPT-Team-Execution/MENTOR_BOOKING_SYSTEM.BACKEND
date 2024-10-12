using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MBS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Testaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Majors",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Name", "ParentId", "Status", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { new Guid("71577eaf-ebf1-4b23-a48d-cf8561b1c7db"), null, null, "SS", null, "Activated", null, null },
                    { new Guid("903b6085-4cc3-47f3-bbdd-0f8319e5aabb"), null, null, "SE", null, "Activated", null, null },
                    { new Guid("dfdb83a4-18e0-447e-9ec8-7c8b39ee6f3a"), null, null, "SA", null, "Activated", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Majors",
                keyColumn: "Id",
                keyValue: new Guid("71577eaf-ebf1-4b23-a48d-cf8561b1c7db"));

            migrationBuilder.DeleteData(
                table: "Majors",
                keyColumn: "Id",
                keyValue: new Guid("903b6085-4cc3-47f3-bbdd-0f8319e5aabb"));

            migrationBuilder.DeleteData(
                table: "Majors",
                keyColumn: "Id",
                keyValue: new Guid("dfdb83a4-18e0-447e-9ec8-7c8b39ee6f3a"));
        }
    }
}
