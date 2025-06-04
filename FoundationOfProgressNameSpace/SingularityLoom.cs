using Blindsided.SaveData;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;

namespace FoundationOfProgressNameSpace
{
    public class SingularityLoom : Building
    {
        public override FopBuilding FopBuildingData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.SingularityLoom;
            set => FoundationOfProductionStaticReferences.FopBuildings.SingularityLoom = value;
        }

        public override FopBuilding FopBuildingCreatedData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.GalacticNexus;
            set => FoundationOfProductionStaticReferences.FopBuildings.GalacticNexus = value;
        }

        public override FopBuildingStats FopBuildingStatsData
        {
            get => Stats.SingularityLoomStats;
            set => Stats.SingularityLoomStats = value;
        }

        public override double RequiredCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.QuantumSeeds;
            set => FoundationOfProductionStaticReferences.FopCurrencies.QuantumSeeds = value;
        }

        public override double ProducedCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.TranscendentFilaments;
            set => FoundationOfProductionStaticReferences.FopCurrencies.TranscendentFilaments = value;
        }

        public override bool AutoBuy => Ascendancy.AutobuySingularityLoom;
    }
}