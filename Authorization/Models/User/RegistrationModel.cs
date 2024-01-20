using System.ComponentModel.DataAnnotations;

namespace Authorization.Models.User
{
    public class RegistrationModel
    {

        public string username { get; set; } = null!;

        public string first_name { get; set; } = null!;

        public string last_name { get; set; } = null!;

        public string email { get; set; } = null!;

        public int? country_id { get; set; }

        public string phone_number { get; set; } = null!;

        public string passwordHash { get; set; } = null!;


    }
}
