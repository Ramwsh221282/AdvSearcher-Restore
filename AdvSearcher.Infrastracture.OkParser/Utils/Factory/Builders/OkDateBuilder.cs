using System.Text;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;
using HtmlAgilityPack;

namespace AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;

internal sealed class OkDateBuilder(HtmlNode node, IAdvertisementDateConverter<OkParser> converter)
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
