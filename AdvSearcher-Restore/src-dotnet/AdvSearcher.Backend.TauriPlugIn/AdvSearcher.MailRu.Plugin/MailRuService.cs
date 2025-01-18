using System.Net;
using System.Net.Mail;
using AdvSearcher.Core.Tools;
using AdvSearcher.Publishing.SDK;
using AdvSearcher.Publishing.SDK.Models;

namespace AdvSearcher.MailRu.Plugin;

public class MailRuService : IMailingClient
{
    private readonly PublishingLogger _logger;
    private Action<int>? _currentProgress;
    private Action<int>? _maxProgress;

    public MailRuService(PublishingLogger logger)
    {
        _logger = logger;
    }

    public async Task Send(IEnumerable<AdvertisementFileResponse> files, MailingAddress address)
    {
        _logger.Log("Starting email publishing requests");
        MailRuSettingsLoader loader = new MailRuSettingsLoader();
        Result<MailRuSettings> settings = loader.Load();
        if (settings.IsFailure)
        {
            _logger.Log("No email settings for mail ru service received. Stopping process");
            return;
        }
        _maxProgress?.Invoke(files.Count());
        int currentProgress = 0;
        using (SmtpClient client = CreateClient(settings))
            foreach (var file in files)
                using (MailMessage message = BuildMessage(file, address, settings))
                {
                    await client.SendMailAsync(message);
                    currentProgress = currentProgress + 1;
                    _currentProgress?.Invoke(currentProgress);
                }
    }

    private SmtpClient CreateClient(MailRuSettings settings)
    {
        SmtpClient client = new SmtpClient("smtp.mail.ru", 587);
        client.Credentials = new NetworkCredential(settings.FromEmail, settings.SmtpKey);
        client.EnableSsl = true;
        _logger.Log("Smtp client instance created");
        return client;
    }

    private MailMessage BuildMessage(
        AdvertisementFileResponse file,
        MailingAddress address,
        MailRuSettings settings
    )
    {
        MailMessage message = new MailMessage();
        message.From = new MailAddress(settings.FromEmail);
        message.To.Add(address.Email);
        message.Subject = address.Subject;
        message.Body = File.ReadAllText(
            Path.Combine(file.Path, AdvertisementFileResponse.TextFilePath)
        );
        _logger.Log("Mail message has been created");
        return IncludeAttachments(file, message);
    }

    private MailMessage IncludeAttachments(AdvertisementFileResponse file, MailMessage message)
    {
        if (!file.Photos.Any())
        {
            _logger.Log("Photo has no images. Not including photos");
            return message;
        }
        file.Photos.ForEach(p => message.Attachments.Add(new Attachment(p)));
        _logger.Log("Attachments for photo has been included");
        return message;
    }

    public void SetCurrentProgressValuePublisher(Action<int> actionPublisher) =>
        _currentProgress = actionPublisher;

    public void SetMaxProgressValuePublisher(Action<int> actionPublisher) =>
        _maxProgress = actionPublisher;
}
