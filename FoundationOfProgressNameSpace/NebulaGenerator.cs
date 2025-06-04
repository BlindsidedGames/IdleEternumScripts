using Blindsided.SaveData;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;

namespace FoundationOfProgressNameSpace
{
    public class NebulaGenerator : Building
    {
        public override FopBuilding FopBuildingData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.NebulaGenerator;
            set => FoundationOfProductionStaticReferences.FopBuildings.NebulaGenerator = value;
        }

        public override FopBuilding FopBuildingCreatedData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.StarCradle;
            set => FoundationOfProductionStaticReferences.FopBuildings.StarCradle = value;
        }

        public override FopBuildingStats FopBuildingStatsData
        {
            get => Stats.NebulaGeneratorStats;
            set => Stats.NebulaGeneratorStats = value;
        }

        public override double RequiredCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.StellarParticles;
            set => FoundationOfProductionStaticReferences.FopCurrencies.StellarParticles = value;
        }

        public override double ProducedCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.NebulaDust;
            set => FoundationOfProductionStaticReferences.FopCurrencies.NebulaDust = value;
        }

        public override bool AutoBuy => Ascendancy.AutobuyNebulaGenerator;
    }
}