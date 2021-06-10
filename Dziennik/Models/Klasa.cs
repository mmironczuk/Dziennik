using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dziennik.Models
{
    public class Klasa
    {
        public int KlasaId { get; set; }
        [Display(Name="Nazwa klasy")]
        [Required]
        public string nazwa { get; set; }
        [Display(Name ="Wychowawca")]
        [Required]
        public string KontoId { get; set; }
        public virtual Konto Wychowawca{ get; set; }
        public virtual ICollection<Konto> Uczniowie { get; set; }
        public virtual ICollection<Nauczanie> Nauczania { get; set; }
    }
}
