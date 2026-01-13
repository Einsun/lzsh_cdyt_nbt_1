using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NBWebApp.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "count",
                table: "gatherrules");

            migrationBuilder.DropColumn(
                name: "gatherruleid",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "ip",
                table: "commdevices");

            migrationBuilder.AddColumn<int>(
                name: "gatherruleid",
                table: "commdevices",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gatherruleid",
                table: "commdevices");

            migrationBuilder.AddColumn<int>(
                name: "count",
                table: "gatherrules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "gatherruleid",
                table: "devices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ip",
                table: "commdevices",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
