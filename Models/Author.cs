using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace EbookSTR
{
    public partial class Author
    {
        public Author()
        {
            Ebooks = new HashSet<Ebook>();
        }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        [StringLength(60)]
        [Display(Name = "Ім\'я")]
        public string Name { get; set; }

        [Display(Name = "Жанри")]
        public int GenreId { get; set; }

        [Display(Name = "Жанри")]
        public virtual Genre Genre { get; set; }
        public virtual ICollection<Ebook> Ebooks { get; set; }
    }
}
