using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace EbookSTR
{
    public partial class Purchase
    {
        [Required(ErrorMessage = "Поле необхідно заповнити")]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int EbookId { get; set; }

        [Display(Name = "Дата придбання")]
        public DateTime Date { get; set; }

        public virtual Client Client { get; set; }
        public virtual Ebook Ebook { get; set; }
    }
}
