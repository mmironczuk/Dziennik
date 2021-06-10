using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dziennik.Models
{
    public class Pytanie
    {
        public int PytanieId { get; set; }
        [Required]
        [Display(Name = "Treść pytania")]
        public string tresc { get; set; }
        [Required]
        [Display(Name ="Odpowiedź A")]
        public string odpA { get; set; }
        [Required]
        [Display(Name = "Odpowiedź B")]
        public string odpB { get; set; }
        [Required]
        [Display(Name = "Odpowiedź C")]
        public string odpC { get; set; }
        [Required]
        [Display(Name = "Odpowiedź D")]
        public string odpD { get; set; }
        [Required]
        [Display(Name = "Poprawna odpowiedź")]
        public string poprawna { get; set; }
        [Range(0,5)]
        public int punkty { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
    }
}
