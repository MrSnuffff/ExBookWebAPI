using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Authorization.Models
{
    public class VerificationCodes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int verificationcodes_id { get; set; }
        public int user_id { get; set; }
        public string VerificationCode { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
