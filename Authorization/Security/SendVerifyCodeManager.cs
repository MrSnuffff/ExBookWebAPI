using Authorization.Models;
using Authorization.Models.User;
using Microsoft.EntityFrameworkCore;
using Authorization.Data;
using Authorization.Models.UserModels;

namespace Authorization.Security
{
    public class SendVerifyCodeManager
    {
        private readonly AppDbContext db;

        public SendVerifyCodeManager(AppDbContext context)
        {
            db = context;
        }

        public async Task SendCodeAsync(string email)
        {
            Random random = new Random();
            string code = random.Next(100000, 1000000).ToString();

            User? existingUser = await db.Users.FirstOrDefaultAsync(x => x.email == email);

            if (existingUser != null)
            {
                string subject = "Verify your ExBook account email";
                string body = $"Verify code: {code}";

                VerificationCodes _verificationCode = new VerificationCodes()
                {
                    user_id = existingUser.user_id,
                    VerificationCode = code,
                    CreatedAt = DateTime.Now
                };

                VerificationCodes? verificationCodes = await db.VerificationCodes.FirstOrDefaultAsync(x => x.user_id == existingUser.user_id);
                if(verificationCodes != null) 
                    db.VerificationCodes.Remove(verificationCodes);
                
                db.VerificationCodes.Add(_verificationCode);
                await db.SaveChangesAsync();
                await SendingEmailManager.SendEmailAsync(email, subject, body);
            }
            else
            {
                
            }
        }
    }
}
