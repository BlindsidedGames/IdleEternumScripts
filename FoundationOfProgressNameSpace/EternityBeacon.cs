using Blindsided.SaveData;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;

namespace FoundationOfProgressNameSpace
{
    public class EternityBeacon : Building
    {
        public override FopBuilding FopBuildingData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.EternityBeacon;
            set => FoundationOfProductionStaticReferences.FopBuildings.EternityBeacon = value;
        }

        public override FopBuilding FopBuildingCreatedData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.SingularityLoom;
            set => FoundationOfProductionStaticReferences.FopBuildings.SingularityLoom = value;
        }

        public override FopBuildingStats FopBuildingStatsData
        {
            get => Stats.EternityBeaconStats;
            set => Stats.EternityBeaconStats = value;
        }

        public override double RequiredCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.TranscendentFilaments;
            set => FoundationOfProductionStaticReferences.FopCurrencies.TranscendentFilaments = value;
        }

        public override double ProducedCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.EterniumSparks;
            set => FoundationOfProductionStaticReferences.FopCurrencies.EterniumSparks = value;
        }

        public override bool AutoBuy => Ascendancy.AutobuyEternityBeacon;
    }
}