using System.Text;
using Newtonsoft.Json.Linq;

namespace AdvSearcher.Infrastructure.Domclick.InternalModels;

internal sealed class DomclickFetchResult
{
    public string Id { get; init; } = string.Empty;
    public string[] PhotoUrls { get; init; }
    public string Description { get; init; }
    public bool IsAgent { get; init; }
    public string SourceUrl { get; init; } = string.Empty;
    public DateOnly PublishedDate { get; init; }
    public string Price { get; init; }
    public string PhoneNumber { get; set; } = string.Empty;

    public DomclickFetchResult(JToken json)
    {
        JToken? pathJson = json["path"];
        if (pathJson != null)
            this.SourceUrl = pathJson.ToString();
        JToken? idJson = json["id"];
        if (idJson != null)
            this.Id = idJson.ToString();
        this.PhotoUrls = ExtractPhotoUrls(json);
        this.Description = ExtractDescription(json);
        this.IsAgent = CheckIfAgent(json);
        PublishedDate = ExtractDate(json);
        Price = ExtractPrice(json);
    }

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
                        .Append(p["url"]!.ToString())
                        .ToString()
                )
                .ToArray();
        }
        catch
        {
            return [];
        }
    }

    private bool CheckIfAgent(JToken json)
    {
        try
        {
            JToken? agentJson = json["isAgent"];
            if (agentJson == null)
                return false;
            string agentInfo = agentJson.ToString();
            return string.Equals(
                    agentJson["isAgency"]!.ToString(),
                    "true",
                    StringComparison.OrdinalIgnoreCase
                ) || string.Equals(agentInfo, "true", StringComparison.OrdinalIgnoreCase);
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
