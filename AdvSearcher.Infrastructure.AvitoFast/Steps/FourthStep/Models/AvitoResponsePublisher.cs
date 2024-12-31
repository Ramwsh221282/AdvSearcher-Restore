using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.AvitoFast.InternalModels;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.FourthStep.Models;

internal sealed record AvitoResponsePublisher : IParsedPublisher
{
    public string Info { get; set; }

    private AvitoResponsePublisher(string info)
    {
        Info = info;
    }

    public static Result<IParsedPublisher> Create(AvitoAdvertisement advertisement)
    {
        string info = advertisement.PublisherInfo.ToString();
        if (string.IsNullOrEmpty(info))
            return new Error("Publisher info is empty");
        return new AvitoResponsePublisher(info);
    }
};
