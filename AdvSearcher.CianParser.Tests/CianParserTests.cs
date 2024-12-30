// using AdvSearcher.Core.Entities.ServiceUrls;
// using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
// using AdvSearcher.Infrastructure.CianParser.DependencyInjection;
// using AdvSearcher.Parser.SDK.Contracts;
// using AdvSearcher.Parser.SDK.DependencyInjection;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace AdvSearcher.CianParser.Tests;
//
// [TestFixture]
// [Category("CianParser")]
// public class CianParserTests
// {
//     private readonly IServiceCollection _services;
//
//     public CianParserTests()
//     {
//         _services = new ServiceCollection();
//         _services.AddParserSDK().AddCianParser();
//     }
//
//     [Test, Order(1)]
//     public async Task TestCianParserSinglePage()
//     {
//         var mode = ServiceUrlMode.Loadable;
//         var url = ServiceUrlValue.Create(
//             "https://krasnoyarsk.cian.ru/kupit-kvartiru-krasnoyarskiy-kray-lesosibirsk/"
//         );
//         var serviceUrl = ServiceUrl.Create(url, mode);
//         var provider = _services.BuildServiceProvider();
//         var parser = provider.GetRequiredService<IParser>();
//         await parser.ParseData(serviceUrl);
//         var items = parser.Results;
//         Assert.That(items, Is.Not.Empty);
//     }
// }
