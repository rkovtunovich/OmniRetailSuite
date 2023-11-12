using System.Threading.Tasks;

namespace ProductCatalog.Core.Messaging;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}
