using AdvSearcher.Infrastructure.Avito.Materials;
using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito.Utils.WebDriverCommands.FetchGalleryItems;

internal sealed class FetchGalleryItemsCommand(AvitoCatalogueItem item)
    : IAvitoWebDriverCommand<FetchGalleryItemsCommand>
{
    private const string GalleryItemsPath = "//*[@data-marker='item-view/gallery']";
    private const string LiAttribute = "li";
    private const string SlideButtonPath = "//*[@class='image-frame-controlButton-_vPNK']";
    private const string ImageFrameWrapper = "//*[@class='image-frame-wrapper-_NvbY']";
    private const string ImgTag = "img";
    private const string SrcTag = "src";

    public async Task Execute(IWebDriver driver)
    {
        IWebElement galleryItemsElement = null;
        int gallerySearchingTries = 0;
        while (galleryItemsElement == null && gallerySearchingTries <= 50)
        {
            try
            {
                galleryItemsElement = driver.FindElement(By.XPath(GalleryItemsPath));
            }
            catch
            {
                gallerySearchingTries++;
            }
        }

        if (gallerySearchingTries == 50)
            await Task.CompletedTask;

        var galleryItemsArray = galleryItemsElement.FindElements(By.TagName(LiAttribute)).ToArray();
        var galleryItemsCount = galleryItemsArray.Length;
        var slideButtonElement = galleryItemsElement.FindElement(By.XPath(SlideButtonPath));
        var srcValue = string.Empty;
        for (var index = 0; index < galleryItemsCount; index++)
        {
            slideButtonElement.Click();
            await Task.Delay(1000);
            try
            {
                var updatedImage = galleryItemsElement.FindElement(By.XPath(ImageFrameWrapper));
                var updateImageWithSrc = updatedImage.FindElement(By.TagName(ImgTag));
                var attribute = updateImageWithSrc.GetDomAttribute(SrcTag);
                if (attribute != srcValue)
                {
                    srcValue = attribute;
                    item.PhotoUrls.Add(srcValue);
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}
