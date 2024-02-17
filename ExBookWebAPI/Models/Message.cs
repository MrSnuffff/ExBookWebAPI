using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExBookWebAPI.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Message_id { get; set; }

        public int sender_id { get; set; }

        public int recipient_id { get; set; }

        public string message_text { get; set; }

        public DateTime Timestamp { get; } = DateTime.Now;
    }

}
