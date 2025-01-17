namespace AdvSearcher.Parser.SDK.HttpParsing;

public interface IHttpService
{
    public Task Execute(IHttpClient client, IHttpRequest request);
    public string Result { get; }
}
