using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Publishers;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Infrastracture.OkParser.Models.InternalModels;

internal sealed record OkParsedPublisher : IParsedPublisher
{
    public string Info { get; set; } = string.Empty;

    private OkParsedPublisher(string info) => Info = info;

    public static Result<OkParsedPublisher> Create(string? info)
    {
        if (string.IsNullOrWhiteSpace(info))
            return PublisherErrors.InfoEmpty;

        return new OkParsedPublisher(info);
    }
}
