using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PathFinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changed_JobSkills_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSkills",
                table: "JobSkills");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "JobSkills");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "JobSkills");

            migrationBuilder.DropColumn(
                name: "IsDeprecated",
                table: "JobSkills");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "JobSkills");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSkills",
                table: "JobSkills",
                columns: new[] { "JobId", "SkillId" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "665376b2-6d04-42d6-95a8-4a14e1819649",
                column: "ConcurrencyStamp",
                value: "9/21/2025 3:35:07 PM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b35f5379-539e-413c-8ebc-e407fdf705c2",
                column: "ConcurrencyStamp",
                value: "9/21/2025 3:35:07 PM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6a2221f-9e15-4474-9264-73a76447849e",
                column: "ConcurrencyStamp",
                value: "9/21/2025 3:35:07 PM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSkills",
                table: "JobSkills");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "JobSkills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "JobSkills",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeprecated",
                table: "JobSkills",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "JobSkills",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSkills",
                table: "JobSkills",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "665376b2-6d04-42d6-95a8-4a14e1819649",
                column: "ConcurrencyStamp",
                value: "9/21/2025 2:41:36 PM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b35f5379-539e-413c-8ebc-e407fdf705c2",
                column: "ConcurrencyStamp",
                value: "9/21/2025 2:41:36 PM");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6a2221f-9e15-4474-9264-73a76447849e",
                column: "ConcurrencyStamp",
                value: "9/21/2025 2:41:36 PM");
        }
    }
}
