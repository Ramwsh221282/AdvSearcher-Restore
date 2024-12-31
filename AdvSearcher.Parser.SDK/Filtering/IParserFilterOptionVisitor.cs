namespace AdvSearcher.Parser.SDK.Filtering;

// a contract that should implement a service that needs filtering during parsing process.
public interface IParserFilterOptionVisitor
{
    bool MatchesFilterOption(ParserFilterWithDate dateFilter);
    bool MatchesFilterOption(ParserFilterWithIgnoreNames ignoreNamesFilter);
    bool MatchesFilterOption(ParserFilterWithCachedAdvertisements cachedAdvertisementsFilter);
}
