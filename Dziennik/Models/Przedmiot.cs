using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dziennik.Models
{
    public class Przedmiot
    {
        public int PrzedmiotId { get; set; }
        [Required]
        [Display(Name ="Nazwa przedmiotu")]
        public string nazwa { get; set; }
        [Display(Name ="Dziedzina")]
        public string dziedzina { get; set; }
        [Display(Name ="Treści kształceniowe")]
        public string tresci { get; set; }
        public virtual ICollection<Nauczanie> Nauczania { get; set; }
        public virtual ICollection<Plik> Pliki { get; set; }
    }
}
