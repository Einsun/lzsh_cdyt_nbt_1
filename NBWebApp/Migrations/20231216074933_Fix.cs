using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NBWebApp.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "channel",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "maxsignal",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "minsignal",
                table: "devices");

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "devices",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "alarmtype",
                table: "alarmdatas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unit",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "alarmtype",
                table: "alarmdatas");

            migrationBuilder.AddColumn<int>(
                name: "channel",
                table: "devices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "maxsignal",
                table: "devices",
                type: "decimal(10,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "minsignal",
                table: "devices",
                type: "decimal(10,5)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
