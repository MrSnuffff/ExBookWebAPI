using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExBookWebAPI.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int book_id { get; set; }

        public string title { get; set; } = null!;

        public string? isbn { get; set; }

        public int? author_id { get; set; }

        public int? publisher_id { get; set; }

        public int? language_id { get; set; }

        // Навигационные свойства
        [ForeignKey("author_id")]
        public virtual Author? Author { get; set; }

        [ForeignKey("publisher_id")]
        public virtual Publisher? Publisher { get; set; }

        //[ForeignKey("language_id")]
        //public virtual Language? Language { get; set; }

        //public virtual ICollection<Userbook> Userbooks { get; } = new List<Userbook>();
    }
}

