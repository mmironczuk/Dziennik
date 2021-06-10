using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dziennik.Models
{
    public class Wiadomosc
    {
        public int WiadomoscId { get; set; }
        [Required]
        [Display(Name ="Tytuł")]
        public string tytul { get; set; }
        [Display(Name ="Treść")]
        public string tresc { get; set; }
        public DateTime data { get; set; }
        public string OdbiorcaId { get; set; }
        public string NadawcaId { get; set; }
        public int czy_odczytana { get; set; }
        public virtual Konto Odbiorca { get; set; }
        public virtual Konto Nadawca { get; set; }
    }
}
