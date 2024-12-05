using System.Net;
using System.Net.Mail;
using EnergyManagementSystem.Core.Configuration;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Interfaces.IService;
using Microsoft.Extensions.Options;

namespace EnergyManagementSystem.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
            {
                EnableSsl = _emailSettings.UseTls,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password)
            };

            var message = new MailMessage
            {
                From = new MailAddress(_emailSettings.Username),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(to);

            await client.SendMailAsync(message);
        }

        public async Task SendPasswordResetEmailAsync(string to, string resetToken)
        {
            var subject = "Password Reset Request";
            var body = $@"
                <h2>Password Reset Request</h2>
                <p>To reset your password, click the link below:</p>
                <p><a href='http://localhost:7211/reset-password?token={resetToken}&email={to}'>Reset Password</a></p>
                <p>If you didn't request a password reset, please ignore this email.</p>
                <p>This link will expire in 1 hour.</p>";

            await SendEmailAsync(to, subject, body);
        }
        public async Task SendEmailConfirmationAsync(string to, string confirmationToken)
        {
            var subject = "Confirm Your Email Address";
            var body = $@"
            <h2>Welcome to Energy Management System!</h2>
            <p>Please confirm your email address by clicking the link below:</p>
            <p><a href='http://localhost:7211/confirm-email?token={confirmationToken}&email={to}'>Confirm Email</a></p>
            <p>This link will expire in 24 hours.</p>
            <p>If you didn't create an account, please ignore this email.</p>";

            await SendEmailAsync(to, subject, body);
        }
    }
}