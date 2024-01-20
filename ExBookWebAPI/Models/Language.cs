using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExBookWebAPI.Models
{
    public class Language
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int language_id { get; set; }

        public string language_code { get; set; }

        public string language_name { get; set; }
    }
}
