using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dziennik.Models
{
    public class Test
    {
        public int TestId { get; set; }
        [Required]
        [Display(Name ="Nazwa")]
        public string nazwa { get; set; }
        [Display(Name = "Opis")]
        public string opis { get; set; }
        [Display(Name = "Czas trwania (w minutach)")]
        [Range(1,Double.PositiveInfinity)]
        public int czas_trwania { get; set; }
        public int NauczanieId { get; set; }
        public virtual Nauczanie Nauczanie { get; set; }
        public virtual ICollection<Pytanie> Pytania { get; set; }
    }
}
