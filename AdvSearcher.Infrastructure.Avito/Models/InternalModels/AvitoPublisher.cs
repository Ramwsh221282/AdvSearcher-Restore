using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Materials;

namespace AdvSearcher.Infrastructure.Avito.Models.InternalModels;

internal sealed record AvitoPublisher : IParsedPublisher
{
    public string Info { get; set; }

    private AvitoPublisher(string info) => Info = info;

    public static Result<IParsedPublisher> Create(AvitoCatalogueItem item) =>
        string.IsNullOrWhiteSpace(item.PublisherInfo)
            ? ParserErrors.CantParsePublisher
            : new AvitoPublisher(item.PublisherInfo);
}
