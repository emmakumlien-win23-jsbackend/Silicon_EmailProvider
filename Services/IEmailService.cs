using Azure.Messaging.ServiceBus;
using Silicon_EmailProvider.Models;

namespace Silicon_EmailProvider.Services;

public interface IEmailService
{
    bool SendEmail(EmailRequest emailRequest);
    EmailRequest UnpackEmailRequest(ServiceBusReceivedMessage message);
}