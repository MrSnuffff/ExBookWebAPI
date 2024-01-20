using System.ComponentModel.DataAnnotations;

namespace Authorization.Models.User
{
    public class LoginModelViaUsername
    {

        public string username { get; set; } = null!;

        public string passwordHash { get; set; } = null!;
    }
}
