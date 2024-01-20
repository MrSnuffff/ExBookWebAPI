using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExBookWebAPI.Models;

public partial class Author
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int author_id { get; set; }

    public string author_name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
