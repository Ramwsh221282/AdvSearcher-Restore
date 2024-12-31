using System.Text;
using System.Text.RegularExpressions;
using AdvSearcher.Infrastructure.AvitoFast.InternalModels;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.ThirdStep.Commands;

internal sealed class ProcessAdvertisementPublisher
    : IWebDriverCommand<ProcessAdvertisementPublisher>
{
    private readonly AvitoAdvertisement _advertisement;
    private const string MainPhoneGetUrl = "https://m.avito.ru/api/1/items/";
    private const string SecretKey = "/phone?key=af0deccbgcgidddjgnvljitntccdduijhdinfgjgfjir";
    private const string Tag = "pre";
    private const string Pattern = "\"Позвонить на\\s*(.*?)\"";
    private static readonly Regex RegexPattern = new Regex(Pattern, RegexOptions.Compiled);

    public ProcessAdvertisementPublisher(AvitoAdvertisement advertisement) =>
        _advertisement = advertisement;

    public async Task ExecuteAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return;
        string url = BuildUrl();
        await new NavigateOnPageCommand(url).ExecuteAsync(provider);
        IWebElement? infoContainer = GetJsonInfoContainer(provider);
        if (infoContainer == null)
            return;
        string innerHtml = infoContainer.GetAttribute("innerHTML");
        Match match = RegexPattern.Match(innerHtml);
        if (match.Success)
        {
            var names = Regex.Replace(match.Groups[1].Value, "\\D", "");
            _advertisement.PublisherInfo.AppendLine(names);
        }
        await Task.CompletedTask;
    }

    private string BuildUrl()
    {
        StringBuilder phoneNavigationLinkBuilder = new StringBuilder();
        phoneNavigationLinkBuilder.Append(MainPhoneGetUrl);
        phoneNavigationLinkBuilder.Append(_advertisement.Id);
        phoneNavigationLinkBuilder.Append(SecretKey);
        return phoneNavigationLinkBuilder.ToString();
    }

    private IWebElement? GetJsonInfoContainer(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return null;

        IWebElement? jsonInfoContainer = null;
        while (jsonInfoContainer == null)
        {
            try
            {
                jsonInfoContainer = provider.Instance.FindElement(By.TagName(Tag));
            }
            catch
            {
                // ignored
            }
        }

        return jsonInfoContainer;
    }
}
