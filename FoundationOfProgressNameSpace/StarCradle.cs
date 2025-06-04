using Blindsided.SaveData;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;

namespace FoundationOfProgressNameSpace
{
    public class StarCradle : Building
    {
        public override FopBuilding FopBuildingData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.StarCradle;
            set => FoundationOfProductionStaticReferences.FopBuildings.StarCradle = value;
        }

        public override FopBuilding FopBuildingCreatedData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.StarCradle;
            set => FoundationOfProductionStaticReferences.FopBuildings.StarCradle = value;
        }

        public override FopBuildingStats FopBuildingStatsData
        {
            get => Stats.StarCradleStats;
            set => Stats.StarCradleStats = value;
        }

        public override double RequiredCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.StellarParticles;
            set => FoundationOfProductionStaticReferences.FopCurrencies.StellarParticles = value;
        }

        public override double ProducedCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.StellarParticles;
            set => FoundationOfProductionStaticReferences.FopCurrencies.StellarParticles = value;
        }

        public override bool AutoBuy => false;
    }
}