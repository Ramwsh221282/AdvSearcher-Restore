// using AdvSearcher.Infrastructure.Domclick.DependencyInjection;
// using AdvSearcher.Parser.SDK.Contracts;
// using AdvSearcher.Parser.SDK.DependencyInjection;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace AdvSearcher.Domclick.Tests;
//
// [TestFixture]
// [Category("Domclick")]
// public class DomclickTests
// {
//     private readonly IServiceCollection _services;
//
//     public DomclickTests()
//     {
//         _services = new ServiceCollection();
//         _services = _services.AddParserSDK().AddDomclickParsingServices();
//     }
//
//     [Test, Order(1)]
//     public async Task TestDomclick()
//     {
//         IServiceProvider provider = _services.BuildServiceProvider();
//         IParser parser = provider.GetRequiredService<IParser>();
//         await parser.ParseData(null!);
//         IReadOnlyCollection<IParserResponse> responses = parser.Results;
//         Assert.That(responses, Is.Not.Empty);
//     }
// }
