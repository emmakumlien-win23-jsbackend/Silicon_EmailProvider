using Azure.Communication.Email;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Silicon_EmailProvider.Functions;
using Silicon_EmailProvider.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<EmailClient>(new EmailClient(Environment.GetEnvironmentVariable("CommunicationServices")));
        services.AddSingleton<IEmailService, EmailService>();
        
    })
    .Build();

host.Run();
