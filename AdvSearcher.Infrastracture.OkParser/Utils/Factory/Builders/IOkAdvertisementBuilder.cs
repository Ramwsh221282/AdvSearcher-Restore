using AdvSearcher.Core.Tools;

namespace AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;

public interface IOkAdvertisementBuilder<TResultType>
{
    Task<Result<TResultType>> Build();
}
