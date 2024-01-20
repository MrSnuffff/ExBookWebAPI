namespace Authorization.Models.User
{
    public class LoginModelViaPhoneNumber
    {
        public string phone_number { get; set; } = null!;

        public string passwordHash { get; set; } = null!;
    }
}
