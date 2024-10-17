﻿using TraffiLearn.Domain.Aggregates.Regions;
using TraffiLearn.Domain.Aggregates.Regions.ValueObjects;

namespace TraffiLearn.Testing.Shared.Factories
{
    public static class RegionFixtureFactory
    {
        public static RegionName CreateRegionName()
        {
            return RegionName.Create("Region1").Value;
        }

        public static Region CreateRegion(RegionName? regionName = null)
        {
            return Region.Create(
                regionId: new RegionId(Guid.NewGuid()),
                name: regionName ?? CreateRegionName()).Value;
        }
    }
}