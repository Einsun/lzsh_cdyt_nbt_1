using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NBWebApp.Migrations
{
    /// <inheritdoc />
    public partial class RuleLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "level",
                table: "alarmrules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "endtime",
                table: "aewavedatas",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "starttime",
                table: "aewavedatas",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "level",
                table: "alarmrules");

            migrationBuilder.DropColumn(
                name: "endtime",
                table: "aewavedatas");

            migrationBuilder.DropColumn(
                name: "starttime",
                table: "aewavedatas");
        }
    }
}
