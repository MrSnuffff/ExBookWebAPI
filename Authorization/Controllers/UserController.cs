﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Authorization.Data;
using Authorization.Models;
using Authorization.Security;
using Authorization.Models.User;


namespace Authorization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly AppDbContext db;
        private readonly SendVerifyCodeManager _sendVerifyCode;
        public UsersController(AppDbContext context, SendVerifyCodeManager sendVerifyCodeManager)
        {
            db = context;
            _sendVerifyCode = sendVerifyCodeManager;
        }

        [HttpPost]
        [Route("LoginViaUsername")]
        public async Task<ActionResult<string>> LoginViaUsername(LoginModelViaUsername loginModel)
        {
            
            User? existingUser = await db.Users.FirstOrDefaultAsync(x => x.username == loginModel.username);
            if (existingUser != null)
                return Unauthorized("User not found");

            if (!existingUser.verified)
                return BadRequest("User not verifed");
            
            string _passwordHash = PasswordHash.HashPassword(loginModel.passwordHash);

            if (existingUser.passwordHash == _passwordHash)
            {
                var (accessToken, refreshToken) = AuthenticationTokenManager.GenerateTokens(existingUser.username, existingUser.email, existingUser.phone_number);

                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
            }
            return Unauthorized("Invalid username or password");
        }


        [HttpPost]
        [Route("LoginViaEmail")]
        public async Task<ActionResult<string>> LoginViaEmail(LoginModelViaEmail loginModel)
        {

            User? existingUser = await db.Users.FirstOrDefaultAsync(x => x.email == loginModel.email);
            if (existingUser != null)
                return Unauthorized("User not verifed");


            if (!existingUser.verified)
                return BadRequest("User not verifed");

            string _passwordHash = PasswordHash.HashPassword(loginModel.passwordHash);

            if (existingUser.passwordHash == _passwordHash)
            {
                var (accessToken, refreshToken) = AuthenticationTokenManager.GenerateTokens(existingUser.username, existingUser.email, existingUser.phone_number);

                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
            }
            return Unauthorized("Invalid username or password");
        }


        [HttpPost]
        [Route("Registration")]
        public async Task<ActionResult<string>> RegistrationAsync(RegistrationModel registrationModel)
        {
            if (db.Users.Any(x => x.username == registrationModel.username || x.email == registrationModel.email ))
            {
                return BadRequest("User already exists");
            }

            registrationModel.passwordHash = PasswordHash.HashPassword(registrationModel.passwordHash);

            User user = new User
            {
                username = registrationModel.username,
                passwordHash = registrationModel.passwordHash,
                country_id = registrationModel.country_id,
                first_name = registrationModel.first_name,
                last_name = registrationModel.last_name,
                email = registrationModel.email,
                phone_number = registrationModel.phone_number
            };
            db.Users.Add(user);

            await db.SaveChangesAsync();
            var (accessToken, refreshToken) = AuthenticationTokenManager.GenerateTokens(registrationModel.username, registrationModel.email, registrationModel.phone_number);

            return Ok();
        }


        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken(string RefreshToken)
        {
            var (isValid, principal) = AuthenticationTokenManager.ValidateToken(RefreshToken);

            if (!isValid || principal == null)
            {
                return Unauthorized("Invalid refresh token");
            }
            string? username = principal.FindFirst(ClaimTypes.Name)?.Value;
            string? email = principal.FindFirst(ClaimTypes.Email)?.Value;
            string? phone_number = principal.FindFirst(ClaimTypes.MobilePhone)?.Value;

            var (newAccessToken, _) = AuthenticationTokenManager.GenerateTokens(username, email, phone_number);

            return Ok(new { AccessToken = newAccessToken });
        }

        [HttpPost]
        [Route("SendVerifyCode")]
        public async Task SendVerifyCode(string email)
        {
            await _sendVerifyCode.SendCodeAsync(email);
        }

        [HttpPut]
        [Route("CheckVerifyCode")]
        public async Task<ActionResult<string>> CheckVerifyCode(string email, string code)
        { 
            User? user = await db.Users.FirstOrDefaultAsync(x => x.email == email);
            VerificationCodes? verificationCodes = await db.VerificationCodes.FirstOrDefaultAsync(x => x.user_id == user.user_id);
            if (verificationCodes == null || verificationCodes.CreatedAt > DateTime.Now.AddMinutes(5) || verificationCodes.VerificationCode != code)
                return BadRequest();

            user.verified = true;

            await db.SaveChangesAsync();

            var (accessToken, refreshToken) = AuthenticationTokenManager.GenerateTokens(user.username, user.email, user.phone_number);

            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
        }
    }
}
