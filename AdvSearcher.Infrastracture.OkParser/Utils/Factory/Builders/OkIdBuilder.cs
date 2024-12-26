using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;

internal sealed class OkIdBuilder(IOkAdvertisementBuilder<string> builder)
    : IOkAdvertisementBuilder<string>
{
    public async Task<Result<string>> Build()
    {
        var url = await builder.Build();
        if (url.IsFailure)
            return ParserErrors.CantParseId;
        ReadOnlySpan<string> parts = url.Value.Split('/');
        return await Task.FromResult(new string(parts[^1]));
    }
}
