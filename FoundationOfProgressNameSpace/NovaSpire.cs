using Blindsided.SaveData;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;

namespace FoundationOfProgressNameSpace
{
    public class NovaSpire : Building
    {
        public override FopBuilding FopBuildingData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.NovaSpire;
            set => FoundationOfProductionStaticReferences.FopBuildings.NovaSpire = value;
        }

        public override FopBuilding FopBuildingCreatedData
        {
            get => FoundationOfProductionStaticReferences.FopBuildings.CelestialFoundry;
            set => FoundationOfProductionStaticReferences.FopBuildings.CelestialFoundry = value;
        }

        public override FopBuildingStats FopBuildingStatsData
        {
            get => Stats.NovaSpireStats;
            set => Stats.NovaSpireStats = value;
        }

        public override double RequiredCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.ForgeEssence;
            set => FoundationOfProductionStaticReferences.FopCurrencies.ForgeEssence = value;
        }

        public override double ProducedCurrency
        {
            get => FoundationOfProductionStaticReferences.FopCurrencies.NovaCores;
            set => FoundationOfProductionStaticReferences.FopCurrencies.NovaCores = value;
        }

        public override bool AutoBuy => Ascendancy.AutobuyNovaSpire;
    }
}