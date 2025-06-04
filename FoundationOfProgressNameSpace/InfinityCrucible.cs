using Blindsided.SaveData;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;

namespace FoundationOfProgressNameSpace
{
    public class InfinityCrucible : Building
    {
        public override FopBuilding FopBuildingData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.InfinityCrucible;
            set => FoundationOfProductionStaticReferences.FopBuildings.InfinityCrucible = value;
        }

        public override FopBuilding FopBuildingCreatedData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.EternityBeacon;
            set => FoundationOfProductionStaticReferences.FopBuildings.EternityBeacon = value;
        }

        public override FopBuildingStats FopBuildingStatsData
        {
            get => Stats.InfinityCrucibleStats;
            set => Stats.InfinityCrucibleStats = value;
        }

        public override double RequiredCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.EterniumSparks;
            set => FoundationOfProductionStaticReferences.FopCurrencies.EterniumSparks = value;
        }

        public override double ProducedCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.TemporalShards;
            set => FoundationOfProductionStaticReferences.FopCurrencies.TemporalShards = value;
        }

        public override bool AutoBuy => Ascendancy.AutobuyInfinityCrucible;
    }
}