using AdvSearcher.Core.Tools;

namespace AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;

internal interface IOkAdvertisementBuilder<TResultType>
{
    Task<Result<TResultType>> Build();
}
