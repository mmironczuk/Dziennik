using Microsoft.EntityFrameworkCore.Migrations;

namespace Dziennik.Migrations
{
    public partial class poprawaTestow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "czas_trwania",
                table: "Pytanie");

            migrationBuilder.AddColumn<int>(
                name: "czas_trwania",
                table: "Test",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "czas_trwania",
                table: "Test");

            migrationBuilder.AddColumn<int>(
                name: "czas_trwania",
                table: "Pytanie",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
