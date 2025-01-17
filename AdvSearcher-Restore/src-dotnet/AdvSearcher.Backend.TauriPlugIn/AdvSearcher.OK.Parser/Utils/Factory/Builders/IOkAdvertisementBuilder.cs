using AdvSearcher.Core.Tools;

namespace AdvSearcher.OK.Parser.Utils.Factory.Builders;

public interface IOkAdvertisementBuilder<TResultType>
{
    Task<Result<TResultType>> Build();
}
