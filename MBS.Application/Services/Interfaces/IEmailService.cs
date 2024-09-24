using MBS.Application.Common.Email;

namespace MBS.Application.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}