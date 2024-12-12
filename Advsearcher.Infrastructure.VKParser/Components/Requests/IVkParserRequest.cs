namespace Advsearcher.Infrastructure.VKParser.Components.Requests;

internal interface IVkParserRequest
{
    Task<string?> ExecuteAsync();
}
