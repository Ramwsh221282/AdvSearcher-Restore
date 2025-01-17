using AdvSearcher.Core.Entities.Advertisements.ValueObjects;

namespace AdvSearcher.Application.Contracts.AdvertisementCache;

public record ExistingAdvertisementTokens(AdvertisementId Id, AdvertisementServiceName Service);
