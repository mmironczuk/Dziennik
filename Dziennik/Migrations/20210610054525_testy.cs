using Microsoft.EntityFrameworkCore.Migrations;

namespace Dziennik.Migrations
{
    public partial class testy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Test_Lekcja_LekcjaId",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Test_LekcjaId",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "LekcjaId",
                table: "Test");

            migrationBuilder.AddColumn<int>(
                name: "NauczanieId",
                table: "Test",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "tresc",
                table: "Pytanie",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "poprawna",
                table: "Pytanie",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "odpD",
                table: "Pytanie",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "odpC",
                table: "Pytanie",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "odpB",
                table: "Pytanie",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "odpA",
                table: "Pytanie",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "czas_trwania",
                table: "Pytanie",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "punkty",
                table: "Pytanie",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Test_NauczanieId",
                table: "Test",
                column: "NauczanieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Nauczanie_NauczanieId",
                table: "Test",
                column: "NauczanieId",
                principalTable: "Nauczanie",
                principalColumn: "NauczanieId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Test_Nauczanie_NauczanieId",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Test_NauczanieId",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "NauczanieId",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "czas_trwania",
                table: "Pytanie");

            migrationBuilder.DropColumn(
                name: "punkty",
                table: "Pytanie");

            migrationBuilder.AddColumn<int>(
                name: "LekcjaId",
                table: "Test",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "tresc",
                table: "Pytanie",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "poprawna",
                table: "Pytanie",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "odpD",
                table: "Pytanie",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "odpC",
                table: "Pytanie",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "odpB",
                table: "Pytanie",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "odpA",
                table: "Pytanie",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Test_LekcjaId",
                table: "Test",
                column: "LekcjaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Lekcja_LekcjaId",
                table: "Test",
                column: "LekcjaId",
                principalTable: "Lekcja",
                principalColumn: "LekcjaId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
