using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PathFinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Profile_Picture_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePhoto",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "665376b2-6d04-42d6-95a8-4a14e1819649",
                column: "ConcurrencyStamp",
                value: "9/20/2025 7:08:35 AM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b35f5379-539e-413c-8ebc-e407fdf705c2",
                column: "ConcurrencyStamp",
                value: "9/20/2025 7:08:35 AM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6a2221f-9e15-4474-9264-73a76447849e",
                column: "ConcurrencyStamp",
                value: "9/20/2025 7:08:35 AM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePhoto",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "665376b2-6d04-42d6-95a8-4a14e1819649",
                column: "ConcurrencyStamp",
                value: "9/19/2025 10:23:22 PM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b35f5379-539e-413c-8ebc-e407fdf705c2",
                column: "ConcurrencyStamp",
                value: "9/19/2025 10:23:22 PM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6a2221f-9e15-4474-9264-73a76447849e",
                column: "ConcurrencyStamp",
                value: "9/19/2025 10:23:22 PM");
        }
    }
}
