using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PathFinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Removed_Address_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Talents");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "665376b2-6d04-42d6-95a8-4a14e1819649",
                column: "ConcurrencyStamp",
                value: "9/21/2025 11:12:23 AM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b35f5379-539e-413c-8ebc-e407fdf705c2",
                column: "ConcurrencyStamp",
                value: "9/21/2025 11:12:23 AM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6a2221f-9e15-4474-9264-73a76447849e",
                column: "ConcurrencyStamp",
                value: "9/21/2025 11:12:23 AM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Talents",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "665376b2-6d04-42d6-95a8-4a14e1819649",
                column: "ConcurrencyStamp",
                value: "9/21/2025 9:39:49 AM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b35f5379-539e-413c-8ebc-e407fdf705c2",
                column: "ConcurrencyStamp",
                value: "9/21/2025 9:39:49 AM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6a2221f-9e15-4474-9264-73a76447849e",
                column: "ConcurrencyStamp",
                value: "9/21/2025 9:39:49 AM");
        }
    }
}
