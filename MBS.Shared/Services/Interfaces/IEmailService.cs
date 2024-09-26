using MBS.Shared.Common.Email;

namespace MBS.Shared.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}