using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExBookWebAPI.Models;

public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int user_id { get; set; }

    public string username { get; set; } = null!;

    public string last_name { get; set; } = null!;

    public string first_name { get; set; } = null!;

    public string email { get; set; } = null!;

    public string phone_number { get; set; } = null!;

    public int? coins { get; set; }

    public string passwordHash { get; set; } = null!;

    public bool verified { get; set; }

    public byte[]? photo { get; set; }
}
