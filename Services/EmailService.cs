using Azure;
using Azure.Communication.Email;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Silicon_EmailProvider.Functions;
using Silicon_EmailProvider.Models;
namespace Silicon_EmailProvider.Services;

public class EmailService(ILogger<EmailService> logger, EmailClient emailClient) : IEmailService
{
    private readonly ILogger<EmailService> _logger = logger;
    private readonly EmailClient _emailClient = emailClient;


    public EmailRequest UnpackEmailRequest(ServiceBusReceivedMessage message)
    {
        try
        {
            var emailRequest = JsonConvert.DeserializeObject<EmailRequest>(message.Body.ToString());
            if (emailRequest != null)
            {
                return emailRequest;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"Eroor: EmailSender.UnpackEmailRequest :: {ex.Message}");
        }
        return null!;
    }

    public bool SendEmail(EmailRequest emailRequest)
    {
        try
        {
            var result = _emailClient.Send(
                WaitUntil.Completed,
                senderAddress: Environment.GetEnvironmentVariable("SenderAddress"),
                recipientAddress: emailRequest.To,
                subject: emailRequest.Subject,
                htmlContent: emailRequest.HtmlBody,
                plainTextContent: emailRequest.PlainText);

            if (result.HasCompleted)
                return true;

        }
        catch (Exception ex)
        {
            _logger.LogError($"Eroor: EmailSender.SendEmailAsync :: {ex.Message}");
        }
        return false;

    }
}
