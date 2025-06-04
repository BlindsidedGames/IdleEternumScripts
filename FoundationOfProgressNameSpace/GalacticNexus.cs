using Blindsided.SaveData;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;

namespace FoundationOfProgressNameSpace
{
    public class GalacticNexus : Building
    {
        public override FopBuilding FopBuildingData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.GalacticNexus;
            set => FoundationOfProductionStaticReferences.FopBuildings.GalacticNexus = value;
        }

        public override FopBuilding FopBuildingCreatedData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.NovaSpire;
            set => FoundationOfProductionStaticReferences.FopBuildings.NovaSpire = value;
        }

        public override FopBuildingStats FopBuildingStatsData
        {
            get => Stats.GalacticNexusStats;
            set => Stats.GalacticNexusStats = value;
        }

        public override double RequiredCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.NovaCores;
            set => FoundationOfProductionStaticReferences.FopCurrencies.NovaCores = value;
        }

        public override double ProducedCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.QuantumSeeds;
            set => FoundationOfProductionStaticReferences.FopCurrencies.QuantumSeeds = value;
        }

        public override bool AutoBuy => Ascendancy.AutobuyGalacticNexus;
    }
}