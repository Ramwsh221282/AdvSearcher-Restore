using AdvSearcher.Avito.Parser.InternalModels;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Avito.Parser.Steps.FourthStep.Models;

public sealed record AvitoResponsePublisher : IParsedPublisher
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
