using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PathFinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Report_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    AssetUrl = table.Column<string>(type: "text", nullable: true),
                    IsComplete = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CompletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "665376b2-6d04-42d6-95a8-4a14e1819649",
                column: "ConcurrencyStamp",
                value: "9/25/2025 1:39:16 PM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b35f5379-539e-413c-8ebc-e407fdf705c2",
                column: "ConcurrencyStamp",
                value: "9/25/2025 1:39:16 PM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6a2221f-9e15-4474-9264-73a76447849e",
                column: "ConcurrencyStamp",
                value: "9/25/2025 1:39:16 PM");

            migrationBuilder.UpdateData(
                table: "Holidays",
                keyColumn: "Id",
                keyValue: new Guid("9e41a73c-3ad9-4efd-98e7-74b3120babad"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 13, 39, 16, 223, DateTimeKind.Utc).AddTicks(4878), new DateTime(2025, 9, 25, 13, 39, 16, 223, DateTimeKind.Utc).AddTicks(4878) });

            migrationBuilder.UpdateData(
                table: "Holidays",
                keyColumn: "Id",
                keyValue: new Guid("9e41a73c-3ad9-ef4d-98e7-74b3120babad"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 13, 39, 16, 223, DateTimeKind.Utc).AddTicks(4882), new DateTime(2025, 9, 25, 13, 39, 16, 223, DateTimeKind.Utc).AddTicks(4882) });

            migrationBuilder.UpdateData(
                table: "Holidays",
                keyColumn: "Id",
                keyValue: new Guid("9e41a7c3-3da9-4efd-98e7-74b3120babad"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 13, 39, 16, 223, DateTimeKind.Utc).AddTicks(4847), new DateTime(2025, 9, 25, 13, 39, 16, 223, DateTimeKind.Utc).AddTicks(4847) });

            migrationBuilder.UpdateData(
                table: "Holidays",
                keyColumn: "Id",
                keyValue: new Guid("9e41a7c3-3da9-4efd-98e7-74b3120badab"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 13, 39, 16, 223, DateTimeKind.Utc).AddTicks(4866), new DateTime(2025, 9, 25, 13, 39, 16, 223, DateTimeKind.Utc).AddTicks(4866) });

            migrationBuilder.UpdateData(
                table: "Holidays",
                keyColumn: "Id",
                keyValue: new Guid("9e4a17c3-3da9-4efd-98e7-74b3120babad"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 13, 39, 16, 223, DateTimeKind.Utc).AddTicks(4875), new DateTime(2025, 9, 25, 13, 39, 16, 223, DateTimeKind.Utc).AddTicks(4875) });

            migrationBuilder.UpdateData(
                table: "Holidays",
                keyColumn: "Id",
                keyValue: new Guid("e941a7c3-3da9-4efd-98e7-74b3120babad"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 13, 39, 16, 223, DateTimeKind.Utc).AddTicks(4871), new DateTime(2025, 9, 25, 13, 39, 16, 223, DateTimeKind.Utc).AddTicks(4871) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");

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

            migrationBuilder.UpdateData(
                table: "Holidays",
                keyColumn: "Id",
                keyValue: new Guid("9e41a73c-3ad9-4efd-98e7-74b3120babad"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8990), new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8990) });

            migrationBuilder.UpdateData(
                table: "Holidays",
                keyColumn: "Id",
                keyValue: new Guid("9e41a73c-3ad9-ef4d-98e7-74b3120babad"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8997), new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8997) });

            migrationBuilder.UpdateData(
                table: "Holidays",
                keyColumn: "Id",
                keyValue: new Guid("9e41a7c3-3da9-4efd-98e7-74b3120babad"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8958), new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8959) });

            migrationBuilder.UpdateData(
                table: "Holidays",
                keyColumn: "Id",
                keyValue: new Guid("9e41a7c3-3da9-4efd-98e7-74b3120badab"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8978), new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8979) });

            migrationBuilder.UpdateData(
                table: "Holidays",
                keyColumn: "Id",
                keyValue: new Guid("9e4a17c3-3da9-4efd-98e7-74b3120babad"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8987), new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8987) });

            migrationBuilder.UpdateData(
                table: "Holidays",
                keyColumn: "Id",
                keyValue: new Guid("e941a7c3-3da9-4efd-98e7-74b3120babad"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8983), new DateTime(2025, 9, 24, 14, 54, 38, 259, DateTimeKind.Utc).AddTicks(8984) });
        }
    }
}
