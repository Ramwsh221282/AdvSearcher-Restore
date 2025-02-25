using System.Text;
using AdvSearcher.Core.Tools;
using AdvSearcher.OK.Parser.Utils.Converters;
using AdvSearcher.Parser.SDK.Contracts;
using HtmlAgilityPack;

namespace AdvSearcher.OK.Parser.Utils.Factory.Builders;

public sealed class OkDateBuilder(HtmlNode node, OkDateConverter converter)
    : IOkAdvertisementBuilder<DateOnly>
{
    private const string Path = ".//div[@class='feed_date']";

    public async Task<Result<DateOnly>> Build()
    {
        var selectedNode = node.SelectSingleNode(Path);
        if (selectedNode == null)
            return ParserErrors.CantConvertDate;

        var innerText = selectedNode.InnerText;
        if (string.IsNullOrWhiteSpace(innerText))
            return ParserErrors.CantConvertDate;

        var stringBuilder = new StringBuilder(innerText);
        var stringDate = stringBuilder.Replace("добавлена", string.Empty).ToString();
        var result = converter.Convert(stringDate);
        return result.IsFailure
            ? await Task.FromResult(result.Error)
            : await Task.FromResult(result);
    }
}
