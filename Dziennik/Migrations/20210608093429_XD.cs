using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dziennik.Migrations
{
    public partial class XD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Przedmiot",
                columns: table => new
                {
                    PrzedmiotId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(nullable: false),
                    dziedzina = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Przedmiot", x => x.PrzedmiotId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Klasa",
                columns: table => new
                {
                    KlasaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(nullable: false),
                    KontoId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klasa", x => x.KlasaId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    imie = table.Column<string>(nullable: true),
                    nazwisko = table.Column<string>(nullable: true),
                    pesel = table.Column<string>(nullable: true),
                    adres = table.Column<string>(nullable: true),
                    active = table.Column<int>(nullable: false),
                    typ_uzytkownika = table.Column<int>(nullable: false),
                    KlasaId = table.Column<int>(nullable: true),
                    RodzicId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Klasa_KlasaId",
                        column: x => x.KlasaId,
                        principalTable: "Klasa",
                        principalColumn: "KlasaId");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetUsers_RodzicId",
                        column: x => x.RodzicId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Nauczanie",
                columns: table => new
                {
                    NauczanieId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlasaId = table.Column<int>(nullable: false),
                    KontoId = table.Column<string>(nullable: true),
                    PrzedmiotId = table.Column<int>(nullable: false),
                    NauczanieId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nauczanie", x => x.NauczanieId);
                    table.ForeignKey(
                        name: "FK_Nauczanie_Klasa_KlasaId",
                        column: x => x.KlasaId,
                        principalTable: "Klasa",
                        principalColumn: "KlasaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nauczanie_AspNetUsers_KontoId",
                        column: x => x.KontoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nauczanie_Nauczanie_NauczanieId1",
                        column: x => x.NauczanieId1,
                        principalTable: "Nauczanie",
                        principalColumn: "NauczanieId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nauczanie_Przedmiot_PrzedmiotId",
                        column: x => x.PrzedmiotId,
                        principalTable: "Przedmiot",
                        principalColumn: "PrzedmiotId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wiadomosc",
                columns: table => new
                {
                    WiadomoscId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tytul = table.Column<string>(nullable: true),
                    tresc = table.Column<string>(nullable: true),
                    data = table.Column<DateTime>(nullable: false),
                    OdbiorcaId = table.Column<string>(nullable: true),
                    NadawcaId = table.Column<string>(nullable: true),
                    czy_odczytana = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wiadomosc", x => x.WiadomoscId);
                    table.ForeignKey(
                        name: "FK_Wiadomosc_AspNetUsers_NadawcaId",
                        column: x => x.NadawcaId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Wiadomosc_AspNetUsers_OdbiorcaId",
                        column: x => x.OdbiorcaId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lekcja",
                columns: table => new
                {
                    LekcjaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NauczanieId = table.Column<int>(nullable: false),
                    data = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lekcja", x => x.LekcjaId);
                    table.ForeignKey(
                        name: "FK_Lekcja_Nauczanie_NauczanieId",
                        column: x => x.NauczanieId,
                        principalTable: "Nauczanie",
                        principalColumn: "NauczanieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ocena",
                columns: table => new
                {
                    OcenaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    wartosc = table.Column<string>(nullable: true),
                    opis = table.Column<string>(nullable: true),
                    data = table.Column<DateTime>(nullable: false),
                    KontoId = table.Column<string>(nullable: true),
                    NauczanieId = table.Column<int>(nullable: false),
                    koncowa = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ocena", x => x.OcenaId);
                    table.ForeignKey(
                        name: "FK_Ocena_AspNetUsers_KontoId",
                        column: x => x.KontoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ocena_Nauczanie_NauczanieId",
                        column: x => x.NauczanieId,
                        principalTable: "Nauczanie",
                        principalColumn: "NauczanieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ogloszenie",
                columns: table => new
                {
                    OgloszenieId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NauczanieId = table.Column<int>(nullable: false),
                    nazwa = table.Column<string>(nullable: true),
                    opis = table.Column<string>(nullable: true),
                    data = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ogloszenie", x => x.OgloszenieId);
                    table.ForeignKey(
                        name: "FK_Ogloszenie_Nauczanie_NauczanieId",
                        column: x => x.NauczanieId,
                        principalTable: "Nauczanie",
                        principalColumn: "NauczanieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Obecnosc",
                columns: table => new
                {
                    ObecnoscId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typ_obecnosci = table.Column<int>(nullable: false),
                    LekcjaId = table.Column<int>(nullable: false),
                    KontoId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obecnosc", x => x.ObecnoscId);
                    table.ForeignKey(
                        name: "FK_Obecnosc_AspNetUsers_KontoId",
                        column: x => x.KontoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Obecnosc_Lekcja_LekcjaId",
                        column: x => x.LekcjaId,
                        principalTable: "Lekcja",
                        principalColumn: "LekcjaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    TestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(nullable: true),
                    opis = table.Column<string>(nullable: true),
                    LekcjaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.TestId);
                    table.ForeignKey(
                        name: "FK_Test_Lekcja_LekcjaId",
                        column: x => x.LekcjaId,
                        principalTable: "Lekcja",
                        principalColumn: "LekcjaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pytanie",
                columns: table => new
                {
                    PytanieId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tresc = table.Column<string>(nullable: true),
                    odpA = table.Column<string>(nullable: true),
                    odpB = table.Column<string>(nullable: true),
                    odpC = table.Column<string>(nullable: true),
                    odpD = table.Column<string>(nullable: true),
                    poprawna = table.Column<string>(nullable: true),
                    TestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pytanie", x => x.PytanieId);
                    table.ForeignKey(
                        name: "FK_Pytanie_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_KlasaId",
                table: "AspNetUsers",
                column: "KlasaId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RodzicId",
                table: "AspNetUsers",
                column: "RodzicId");

            migrationBuilder.CreateIndex(
                name: "IX_Klasa_KontoId",
                table: "Klasa",
                column: "KontoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lekcja_NauczanieId",
                table: "Lekcja",
                column: "NauczanieId");

            migrationBuilder.CreateIndex(
                name: "IX_Nauczanie_KlasaId",
                table: "Nauczanie",
                column: "KlasaId");

            migrationBuilder.CreateIndex(
                name: "IX_Nauczanie_KontoId",
                table: "Nauczanie",
                column: "KontoId");

            migrationBuilder.CreateIndex(
                name: "IX_Nauczanie_NauczanieId1",
                table: "Nauczanie",
                column: "NauczanieId1");

            migrationBuilder.CreateIndex(
                name: "IX_Nauczanie_PrzedmiotId",
                table: "Nauczanie",
                column: "PrzedmiotId");

            migrationBuilder.CreateIndex(
                name: "IX_Obecnosc_KontoId",
                table: "Obecnosc",
                column: "KontoId");

            migrationBuilder.CreateIndex(
                name: "IX_Obecnosc_LekcjaId",
                table: "Obecnosc",
                column: "LekcjaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocena_KontoId",
                table: "Ocena",
                column: "KontoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocena_NauczanieId",
                table: "Ocena",
                column: "NauczanieId");

            migrationBuilder.CreateIndex(
                name: "IX_Ogloszenie_NauczanieId",
                table: "Ogloszenie",
                column: "NauczanieId");

            migrationBuilder.CreateIndex(
                name: "IX_Pytanie_TestId",
                table: "Pytanie",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_LekcjaId",
                table: "Test",
                column: "LekcjaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wiadomosc_NadawcaId",
                table: "Wiadomosc",
                column: "NadawcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Wiadomosc_OdbiorcaId",
                table: "Wiadomosc",
                column: "OdbiorcaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Klasa_AspNetUsers_KontoId",
                table: "Klasa",
                column: "KontoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Klasa_AspNetUsers_KontoId",
                table: "Klasa");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Obecnosc");

            migrationBuilder.DropTable(
                name: "Ocena");

            migrationBuilder.DropTable(
                name: "Ogloszenie");

            migrationBuilder.DropTable(
                name: "Pytanie");

            migrationBuilder.DropTable(
                name: "Wiadomosc");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Test");

            migrationBuilder.DropTable(
                name: "Lekcja");

            migrationBuilder.DropTable(
                name: "Nauczanie");

            migrationBuilder.DropTable(
                name: "Przedmiot");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Klasa");
        }
    }
}
