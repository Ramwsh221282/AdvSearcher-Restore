using System.Text;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.CianParser.Materials.CianComponents;

internal sealed class CianAdvertisementCard
{
    private const string WrapperPath = "_93444fe79c--content--lXy9G";
    private const string ContentPath = "_93444fe79c--general--BCXJ4";
    private const string OwnerPath = "_93444fe79c--aside--ygGB3";

    public const string TagName = "article";
    public IWebElement Card { get; }
    public IWebElement? ContentPart { get; private set; }
    public IWebElement? OwnerPart { get; private set; }

    public CianAdvertisementCardDetails Details { get; }

    public CianAdvertisementCard(IWebElement card)
    {
        Card = card;
        Details = new CianAdvertisementCardDetails();
        var wrapper = card.FindElement(By.ClassName(WrapperPath));
        if (wrapper == null)
            return;
        InitContentPart(wrapper);
        InitOwnerPart(wrapper);
    }

    private void InitContentPart(IWebElement wrapper) =>
        ContentPart = wrapper.FindElement(By.ClassName(ContentPath));

    private void InitOwnerPart(IWebElement wrapper) =>
        OwnerPart = wrapper.FindElement(By.ClassName(OwnerPath));
}

internal static class CianAdvertisementCardExtensions
{
    private const string ContactsWrapper = "_93444fe79c--content--lXy9G";
    private const string ContactsActionLayoutPath = ".//div[@data-name='ContactActionsLayout']";
    private const string PhoneButtonPath = ".//button[@data-mark='PhoneButton']";
    private const string ButtonContainer = "_93444fe79c--actions--nXzOo";
    private const string HrefAttribute = "href";
    private const string AAttribute = "a";
    private const string PhotosWrapper = "_93444fe79c--media--9P6wN";
    private const string PhotosContainer = "_93444fe79c--cont--hnKQl";
    private const string UlElement = "ul";
    private const string LiElement = "li";
    private const string ImgElement = "img";
    private const string SrcElement = "src";
    private const string DateElement = "_93444fe79c--vas--ojM2L";
    private const string HomeOwner = "собственник";

    internal static void InitializeMobilePhone(this CianAdvertisementCard card)
    {
        var contactsElement = card.Card.FindElement(By.ClassName(ContactsWrapper));
        var wrapper = contactsElement?.FindElement(By.ClassName(ButtonContainer));
        var contacts = wrapper?.FindElement(By.XPath(ContactsActionLayoutPath));
        var phoneButton = contacts?.FindElement(By.XPath(PhoneButtonPath));
        if (phoneButton == null)
            return;
        var isClicked = false;
        while (!isClicked)
        {
            try
            {
                phoneButton.Click();
                isClicked = true;
            }
            catch
            {
                // ignored
            }
        }
        if (contacts == null)
            return;

        var contactsText = string.Empty;
        while (string.IsNullOrWhiteSpace(contactsText))
        {
            try
            {
                contactsText = contacts.Text;
            }
            catch
            {
                // ignored
            }
        }

        ReadOnlySpan<string> splittedDetails = contactsText.Split("\r\n");
        var sb = new StringBuilder();
        foreach (var line in splittedDetails)
        {
            if (line.Any(char.IsDigit))
                sb.AppendLine(line);
        }
        card.Details.MobilePhone = sb.ToString();
    }

    internal static bool IsHomeowner(this CianAdvertisementCard card)
    {
        if (card.OwnerPart == null)
            return false;
        var text = card.OwnerPart.Text;
        return text.Contains(HomeOwner, StringComparison.OrdinalIgnoreCase);
    }

    internal static void InitializeContent(this CianAdvertisementCard card)
    {
        if (card.ContentPart == null)
            return;

        card.Details.Description = card.ContentPart.Text;
    }

    internal static void InitializeDate(this CianAdvertisementCard card)
    {
        var dateElement = card.OwnerPart?.FindElement(By.ClassName(DateElement));
        if (dateElement == null)
            return;

        card.Details.DateDescription = dateElement.Text;
    }

    internal static void InitializeSourceUrl(this CianAdvertisementCard card)
    {
        var linkElement = card.ContentPart?.FindElement(By.TagName(AAttribute));
        if (linkElement == null)
            return;

        var attribute = linkElement.GetAttribute(HrefAttribute);
        if (string.IsNullOrWhiteSpace(attribute))
            return;

        card.Details.SourceUrl = attribute;
    }

    internal static void InitializeId(this CianAdvertisementCard card)
    {
        if (string.IsNullOrWhiteSpace(card.Details.SourceUrl))
            return;
        ReadOnlySpan<string> splittedUrl = card.Details.SourceUrl.Split(
            '/',
            StringSplitOptions.RemoveEmptyEntries
        );
        card.Details.Id = splittedUrl[^1];
    }

    internal static void InitializePhotos(this CianAdvertisementCard card)
    {
        var mediaElement = card.Card.FindElement(By.ClassName(PhotosWrapper));
        var galleryElement = mediaElement?.FindElement(By.ClassName(PhotosContainer));
        var picturesContainer = galleryElement?.FindElement(By.TagName(UlElement));
        var pictures = picturesContainer?.FindElements(By.TagName(LiElement));
        if (pictures == null)
            return;
        foreach (var pic in pictures)
        {
            IWebElement? image;
            try
            {
                image = pic?.FindElement(By.TagName(ImgElement));
            }
            catch
            {
                continue;
            }
            if (image == null)
                continue;
            var srcValue = image.GetAttribute(SrcElement);
            if (string.IsNullOrWhiteSpace(srcValue))
                continue;
            card.Details.PhotoUrls.Add(srcValue);
        }
    }
}
