using Dziennik.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dziennik.Data
{
    public class ApplicationDbContext : IdentityDbContext<Konto>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Wiadomosc>(entity =>
            {
                entity.HasOne(x => x.Nadawca)
                .WithMany(x => x.wyslane)
                .HasForeignKey(x => x.NadawcaId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.Odbiorca)
                .WithMany(x => x.odebrane)
                .HasForeignKey(x => x.OdbiorcaId)
                .OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<Klasa>(entity =>
            {
                entity.HasOne<Konto>(x => x.Wychowawca)
                .WithOne(x => x.Wychowankowie)
                .HasForeignKey<Klasa>(x => x.KontoId)
                .OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<Konto>(entity =>
            {
                entity.HasOne(x => x.Klasa)
                .WithMany(x => x.Uczniowie)
                .HasForeignKey(x => x.KlasaId)
                .OnDelete(DeleteBehavior.NoAction);
            });
        }
        public DbSet<Konto> Konta { get; set; }
        public DbSet<Klasa> Klasa { get; set; }
        public DbSet<Lekcja> Lekcja { get; set; }
        public DbSet<Nauczanie> Nauczanie { get; set; }
        public DbSet<Obecnosc> Obecnosc { get; set; }
        public DbSet<Ocena> Ocena { get; set; }
        public DbSet<Ogloszenie> Ogloszenie { get; set; }
        public DbSet<Plik> Plik { get; set; }
        public DbSet<Przedmiot> Przedmiot { get; set; }
        public DbSet<Pytanie> Pytanie { get; set; }
        public DbSet<Test> Test { get; set; }
        public DbSet<Wiadomosc> Wiadomosc { get; set; }
    }
}
