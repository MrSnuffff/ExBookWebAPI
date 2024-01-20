namespace Authorization.Models.User
{
    public class LoginModelViaEmail
    {
        public string email { get; set; } = null!;

        public string passwordHash { get; set; } = null!;
    }
}
