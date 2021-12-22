using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace EbookSTR
{
    public partial class Genre
    {
        public Genre()
        {
            Authors = new HashSet<Author>();
        }
        [Required(ErrorMessage = "Поле необхідно заповнити")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле необхідно заповнити")]
        [StringLength(60)]
        public string Name { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
    }
}
