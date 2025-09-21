using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PathFinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changed_PostedById_To_RecruiterId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Recruiters_PostedById",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_PostedById",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "PostedById",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "PostedByUserId",
                table: "Jobs");

            migrationBuilder.AddColumn<Guid>(
                name: "RecruiterId",
                table: "Jobs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_RecruiterId",
                table: "Jobs",
                column: "RecruiterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Recruiters_RecruiterId",
                table: "Jobs",
                column: "RecruiterId",
                principalTable: "Recruiters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Recruiters_RecruiterId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_RecruiterId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "RecruiterId",
                table: "Jobs");

            migrationBuilder.AddColumn<Guid>(
                name: "PostedById",
                table: "Jobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostedByUserId",
                table: "Jobs",
                type: "text",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_PostedById",
                table: "Jobs",
                column: "PostedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Recruiters_PostedById",
                table: "Jobs",
                column: "PostedById",
                principalTable: "Recruiters",
                principalColumn: "Id");
        }
    }
}
