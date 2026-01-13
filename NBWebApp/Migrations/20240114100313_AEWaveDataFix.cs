using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NBWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AEWaveDataFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "gatherrules");

            migrationBuilder.AddColumn<string>(
                name: "faultenum",
                table: "aewavedatas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "faultsta",
                table: "aewavedatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "mesdataaddr1",
                table: "aewavedatas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "mesdataimage1",
                table: "aewavedatas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "rootmeansquare",
                table: "aewavedatas",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "settle",
                table: "aewavedatas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "faultenum",
                table: "aewavedatas");

            migrationBuilder.DropColumn(
                name: "faultsta",
                table: "aewavedatas");

            migrationBuilder.DropColumn(
                name: "mesdataaddr1",
                table: "aewavedatas");

            migrationBuilder.DropColumn(
                name: "mesdataimage1",
                table: "aewavedatas");

            migrationBuilder.DropColumn(
                name: "rootmeansquare",
                table: "aewavedatas");

            migrationBuilder.DropColumn(
                name: "settle",
                table: "aewavedatas");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "gatherrules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
