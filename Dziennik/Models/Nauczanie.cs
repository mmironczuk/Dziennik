using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziennik.Models
{
    public class Nauczanie
    {
        public int NauczanieId { get; set; }
        public int KlasaId { get; set; }
        public string KontoId { get; set; }
        public int PrzedmiotId { get; set; }
        public virtual Klasa Klasa { get; set; }
        public virtual Konto Konto { get; set; }
        public virtual Przedmiot Przedmiot { get; set; }
        public virtual ICollection<Ocena> Ocena { get; set; }
        public virtual ICollection<Ogloszenie> Ogloszenia { get; set; }
        public virtual ICollection<Lekcja> Lekcje { get; set; }
        public virtual ICollection<Nauczanie> Nauczania { get; set; }
        public virtual ICollection<Test> Testy { get; set; }
    }
}
