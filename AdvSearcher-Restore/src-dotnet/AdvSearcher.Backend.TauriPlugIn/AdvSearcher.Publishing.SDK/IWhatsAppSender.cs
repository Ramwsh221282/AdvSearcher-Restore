using System.Text;
using AdvSearcher.Core.Tools;
using AdvSearcher.Publishing.SDK.Models;

namespace AdvSearcher.Publishing.SDK;

public sealed record WhatsAppSendRequest(string PhoneNumber);

public static class WhatsAppSendRequestFactory
{
    public static Result<WhatsAppSendRequest> CreateCorrect(WhatsAppSendRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            return new Error("Номер телефона не был указан");
        if (!request.PhoneNumber.All(c => char.IsDigit(c)))
            return new Error("Номер телефона должен состоять только из цифр");
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(request.PhoneNumber);
        stringBuilder.Append("@c.us");
        return new WhatsAppSendRequest(stringBuilder.ToString());
    }
}

public interface IWhatsAppSender
{
    Task Publish(IEnumerable<AdvertisementFileResponse> files, WhatsAppSendRequest request);
}
