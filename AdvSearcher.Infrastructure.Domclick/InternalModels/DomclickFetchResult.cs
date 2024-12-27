using System.Text;
using Newtonsoft.Json.Linq;

namespace AdvSearcher.Infrastructure.Domclick.InternalModels;

internal sealed class DomclickFetchResult
{
    public string Id { get; init; } = string.Empty;
    public string[] PhotoUrls { get; init; }
    public string Description { get; init; }
    public bool IsAgent { get; private set; }
    public string SourceUrl { get; init; } = string.Empty;
    public DateOnly PublishedDate { get; init; }
    public string Price { get; init; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string FullName { get; init; }

    public DomclickFetchResult(JToken json)
    {
        FullName = ExtractFullName(json);
        JToken? pathJson = json["path"];
        if (pathJson != null)
            this.SourceUrl = pathJson.ToString();
        JToken? idJson = json["id"];
        if (idJson != null)
            this.Id = idJson.ToString();
        this.PhotoUrls = ExtractPhotoUrls(json);
        this.Description = ExtractDescription(json);
        if (CheckIsAgent(json) || CheckIsAgency(json) || CheckIsCompany(json))
            this.IsAgent = true;
        PublishedDate = ExtractDate(json);
        Price = ExtractPrice(json);
        int bpoint = 0;
    }

    public void MarkAsAgent() => IsAgent = true;

    private string[] ExtractPhotoUrls(JToken json)
    {
        try
        {
            var photosJson = json["photos"];
            if (photosJson == null)
                return [];

            return photosJson
                .Select(p =>
                    new StringBuilder("https://img.dmclk.ru")
                        .Append(p["url"]!.Value<string>())
                        .ToString()
                )
                .ToArray();
        }
        catch
        {
            return [];
        }
    }

    private string ExtractFullName(JToken json)
    {
        JToken? sellerPart = json["seller"];
        if (sellerPart is null)
            return string.Empty;
        JToken? agentPart = sellerPart["agent"];
        if (agentPart is null)
            return string.Empty;
        JToken? fullNamePart = agentPart["fullName"];
        if (fullNamePart is null)
            return string.Empty;
        string? value = fullNamePart.Value<string>();
        return value ?? string.Empty;
    }

    private bool CheckIsCompany(JToken json)
    {
        try
        {
            JToken? sellerPart = json["seller"];
            if (sellerPart == null)
                return false;
            JToken? companyPart = sellerPart["company"];
            if (companyPart == null)
                return false;
            bool hasValues = companyPart.HasValues;
            if (hasValues)
                return true;

            return false;
        }
        catch
        {
            return false;
        }
    }

    private bool CheckIsAgency(JToken json)
    {
        try
        {
            JToken? sellerPart = json["seller"];
            if (sellerPart == null)
                return false;
            JToken? agentPart = sellerPart["agent"];
            if (agentPart == null)
                return false;
            JToken? agencyPart = agentPart["isAgency"];
            if (agencyPart == null)
                return false;
            string? value = agencyPart.Value<string>();
            if (string.IsNullOrWhiteSpace(value))
                return false;
            if (value.Equals("true", StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        catch
        {
            return false;
        }
    }

    private bool CheckIsAgent(JToken json)
    {
        try
        {
            JToken? sellerPart = json["seller"];
            if (sellerPart == null)
                return false;
            JToken? agentPart = sellerPart["agent"];
            if (agentPart == null)
                return false;
            JToken? isAgent = agentPart["isAgent"];
            if (isAgent == null)
                return false;
            string? value = isAgent.Value<string>();
            if (string.IsNullOrWhiteSpace(value))
                return false;
            if (value.Equals("true", StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        catch
        {
            return false;
        }
    }

    private string ExtractDescription(JToken json)
    {
        string displayNameText = string.Empty;
        string descriptionText = string.Empty;

        JToken? address = json["address"];
        if (address != null)
        {
            JToken? displayName = address["displayName"];
            if (displayName != null)
                displayNameText = displayName.ToString();
        }

        JToken? description = json["description"];
        if (description != null)
            descriptionText = description.ToString();

        return new StringBuilder()
            .AppendLine(displayNameText)
            .AppendLine(descriptionText)
            .ToString();
    }

    private DateOnly ExtractDate(JToken json)
    {
        JToken? publishedDateJson = json["publishedDate"];
        if (publishedDateJson == null)
            return DateOnly.MinValue;
        DateTime date = DateTime.Parse(publishedDateJson.ToString());
        return DateOnly.FromDateTime(date);
    }

    private string ExtractPrice(JToken json)
    {
        JToken? priceJson = json["price"];
        if (priceJson != null)
            return priceJson.ToString();
        return string.Empty;
    }
}
