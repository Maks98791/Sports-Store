using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Podaj poprawne imię i nazwisko.")]
        public string Name { get; set; }
        public string Surname { get; set; }

        [Required(ErrorMessage = "Podaj poprawny adres.")]
        public string Street { get; set; }
        public int Number { get; set; }

        [Required(ErrorMessage = "Podaj poprawną nazwe miasta.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Podaj poprawną nazwe województwa.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Podaj poprawny kod pocztowy.")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Podaj poprawną nazwe kraju.")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
    }
}
