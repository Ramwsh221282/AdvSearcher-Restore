using AdvSearcher.Infrastructure.AvitoFast.InternalModels;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.ThirdStep.Commands;

internal sealed class ProcessAdvertisementDescription
    : IWebDriverCommand<ProcessAdvertisementDescription>
{
    private const string DescriptionElementXPath =
        ".//div[@data-marker='item-view/item-description']";
    private readonly AvitoAdvertisement _avitoAdvertisement;

    public ProcessAdvertisementDescription(AvitoAdvertisement advertisement) =>
        _avitoAdvertisement = advertisement;

    public async Task ExecuteAsync(WebDriverProvider provider)
    {
        if (provider.Instance == null)
            return;
        IWebElement descriptionElement = null!;
        while (descriptionElement == null)
        {
            try
            {
                descriptionElement = provider.Instance.FindElement(
                    By.XPath(DescriptionElementXPath)
                );
            }
            catch
            {
                // ignored
            }
        }
        string text = descriptionElement.Text;
        _avitoAdvertisement.Description.AppendLine(text);
        await Task.CompletedTask;
    }
}
