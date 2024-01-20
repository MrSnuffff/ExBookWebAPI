using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExBookWebAPI.Models;

public partial class Userbook
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int userbook_id { get; set; }

    public int book_id { get; set; } 

    public int user_id { get; set; }

    public string? description { get; set; }


    [ForeignKey("book_id")]
    public virtual Book? Book { get; set; }

}
