using Blindsided.SaveData;
using static RealmOfResearchNamespace.RealmOfResearchStaticReferences;

namespace RealmOfResearchNamespace
{
    public class SingularityVault : Researcher
    {
        public override RorBuilding BuildingData
        {
            get => RealmOfResearchStaticReferences.RorBuildings.SingularityVault;
            set => RealmOfResearchStaticReferences.RorBuildings.SingularityVault = value;
        }

        public override RorBuilding BuildingCreatedData
        {
            get => RealmOfResearchStaticReferences.RorBuildings.CollapseEngine;
            set => RealmOfResearchStaticReferences.RorBuildings.CollapseEngine = value;
        }

        public override double RequiredCurrency
        {
            get => Currencies.EntropyFragments;
            set => Currencies.EntropyFragments = value;
        }

        public override bool AutoBuy => false;
    }
}