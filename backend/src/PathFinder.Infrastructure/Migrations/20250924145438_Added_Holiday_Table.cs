using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PathFinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Holiday_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "text", nullable: true),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "665376b2-6d04-42d6-95a8-4a14e1819649",
                column: "ConcurrencyStamp",
                value: "9/24/2025 2:54:38 PM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b35f5379-539e-413c-8ebc-e407fdf705c2",
                column: "ConcurrencyStamp",
                value: "9/24/2025 2:54:38 PM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6a2221f-9e15-4474-9264-73a76447849e",
                column: "ConcurrencyStamp",
                value: "9/24/2025 2:54:38 PM");

            migrationBuilder.InsertData(
                table: "Holidays",
                columns: new[] { "Id", "Country", "CreatedAt", "Date", "IsDeprecated", "IsRecurring", "ModifiedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("9e41a73c-3ad9-4efd-98e7-74b3120babad"), "Global", new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8990), new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), false, true, new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8990), "Christmas Day" },
                    { new Guid("9e41a73c-3ad9-ef4d-98e7-74b3120babad"), "Nigeria", new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8997), new DateTime(2025, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), false, true, new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8997), "Boxing Day" },
                    { new Guid("9e41a7c3-3da9-4efd-98e7-74b3120babad"), "Global", new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8958), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, true, new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8959), "New Year's Day" },
                    { new Guid("9e41a7c3-3da9-4efd-98e7-74b3120badab"), "Nigeria", new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8978), new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, true, new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8979), "Worker's Day" },
                    { new Guid("9e4a17c3-3da9-4efd-98e7-74b3120babad"), "Nigeria", new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8987), new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, true, new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8987), "Independence Day" },
                    { new Guid("e941a7c3-3da9-4efd-98e7-74b3120babad"), "Nigeria", new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8983), new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), false, true, new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8984), "Democracy Day" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "665376b2-6d04-42d6-95a8-4a14e1819649",
                column: "ConcurrencyStamp",
                value: "9/23/2025 12:54:58 AM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b35f5379-539e-413c-8ebc-e407fdf705c2",
                column: "ConcurrencyStamp",
                value: "9/23/2025 12:54:58 AM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6a2221f-9e15-4474-9264-73a76447849e",
                column: "ConcurrencyStamp",
                value: "9/23/2025 12:54:58 AM");
        }
    }
}
