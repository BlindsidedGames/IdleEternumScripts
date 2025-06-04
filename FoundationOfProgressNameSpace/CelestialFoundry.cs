using Blindsided.SaveData;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;

namespace FoundationOfProgressNameSpace
{
    public class CelestialFoundry : Building
    {
        public override FopBuilding FopBuildingData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.CelestialFoundry;
            set => FoundationOfProductionStaticReferences.FopBuildings.CelestialFoundry = value;
        }

        public override FopBuilding FopBuildingCreatedData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.NebulaGenerator;
            set => FoundationOfProductionStaticReferences.FopBuildings.NebulaGenerator = value;
        }

        public override FopBuildingStats FopBuildingStatsData
        {
            get => Stats.CelestialFoundryStats;
            set => Stats.CelestialFoundryStats = value;
        }

        public override double RequiredCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.NebulaDust;
            set => FoundationOfProductionStaticReferences.FopCurrencies.NebulaDust = value;
        }

        public override double ProducedCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.ForgeEssence;
            set => FoundationOfProductionStaticReferences.FopCurrencies.ForgeEssence = value;
        }

        public override bool AutoBuy => Ascendancy.AutobuyCelestialFoundry;
    }
}