using System.Threading.Tasks;

namespace Catalog.Core.Messaging;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}
