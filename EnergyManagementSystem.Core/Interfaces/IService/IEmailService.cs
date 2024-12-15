using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Interfaces.IService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendPasswordResetEmailAsync(string to, string resetToken);
        Task SendEmailConfirmationAsync(string to, string confirmationToken);
        Task SendEmailChangeConfirmationAsync(string to, string token);
    }
}
