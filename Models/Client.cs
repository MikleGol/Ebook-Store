using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace EbookSTR
{
    public partial class Client
    {
        public Client()
        {
            Purchases = new HashSet<Purchase>();
        }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        [StringLength(60)]
        [Display(Name = "Ім\'я")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        [StringLength(60)]
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        [StringLength(60)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
