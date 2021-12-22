using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace EbookSTR
{
    public partial class Ebook
    {
        public Ebook()
        {
            Purchases = new HashSet<Purchase>();
        }


        public int Id { get; set; }

        [Display(Name = "Автор")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        [Display(Name = "Назва")]
        [StringLength(60)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        [Display(Name = "Ціна")]

        public decimal Price { get; set; }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        [StringLength(100)]
        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        [Display(Name = "Кількість сторінок")]

        public int Pages { get; set; }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        [Display(Name = "Формат")]
        [StringLength(60)]
        public string Format { get; set; }

        [Display(Name = "Автор")]
        public virtual Author Author { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
