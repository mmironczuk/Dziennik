using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziennik.Models
{
    public class Plik
    {
        public int PlikId { get; set; }
        public string nazwa { get; set; }
        public string typ { get; set; }
        public byte[] plik { get; set; }
        public DateTime utworzenie { get; set; }
        public int PrzedmiotId { get; set; }
        public virtual Przedmiot Przedmiot { get; set; }
    }
}
