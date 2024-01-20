using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExBookWebAPI.Models;

public partial class Publisher
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int publisher_id { get; set; }

    public string publisher_name { get; set; } = null!;

    
}
