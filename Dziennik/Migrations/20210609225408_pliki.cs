using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dziennik.Migrations
{
    public partial class pliki : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "tytul",
                table: "Wiadomosc",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tresci",
                table: "Przedmiot",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "akceptacja",
                table: "Ogloszenie",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "wszyscy",
                table: "Ogloszenie",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Plik",
                columns: table => new
                {
                    PlikId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(nullable: true),
                    typ = table.Column<string>(nullable: true),
                    plik = table.Column<byte[]>(nullable: true),
                    utworzenie = table.Column<DateTime>(nullable: false),
                    PrzedmiotId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plik", x => x.PlikId);
                    table.ForeignKey(
                        name: "FK_Plik_Przedmiot_PrzedmiotId",
                        column: x => x.PrzedmiotId,
                        principalTable: "Przedmiot",
                        principalColumn: "PrzedmiotId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plik_PrzedmiotId",
                table: "Plik",
                column: "PrzedmiotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plik");

            migrationBuilder.DropColumn(
                name: "tresci",
                table: "Przedmiot");

            migrationBuilder.DropColumn(
                name: "akceptacja",
                table: "Ogloszenie");

            migrationBuilder.DropColumn(
                name: "wszyscy",
                table: "Ogloszenie");

            migrationBuilder.AlterColumn<string>(
                name: "tytul",
                table: "Wiadomosc",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
