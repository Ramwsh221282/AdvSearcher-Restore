using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.AvitoFast.Steps;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.AvitoFast;

internal sealed class AvitoFastParser : IParser
{
    private readonly IAvitoFastParserStep _step;
    private readonly List<IParserResponse> _results = [];
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public AvitoFastParser(IAvitoFastParserStep step) => _step = step;

    public async Task<Result<bool>> ParseData(ServiceUrl url)
    {
        if (url.Mode == ServiceUrlMode.Publicatable)
            return false;
        _step.Pipeline.SetServiceUrl(url);
        await _step.ProcessAsync();
        _results.AddRange(_step.Pipeline.Results);
        return true;
    }
}
