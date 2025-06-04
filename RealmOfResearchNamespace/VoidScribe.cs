using Blindsided.SaveData;
using static RealmOfResearchNamespace.RealmOfResearchStaticReferences;

namespace RealmOfResearchNamespace
{
    public class VoidScribe : Researcher
    {
        public override RorBuilding BuildingData
        {
            get => RealmOfResearchStaticReferences.RorBuildings.VoidScribe;
            set => RealmOfResearchStaticReferences.RorBuildings.VoidScribe = value;
        }

        public override RorBuilding BuildingCreatedData
        {
            get => RealmOfResearchStaticReferences.RorBuildings.VoidScribe;
            set => RealmOfResearchStaticReferences.RorBuildings.VoidScribe = value;
        }

        public override double RequiredCurrency
        {
            get => Currencies.EntropyFragments;
            set => Currencies.EntropyFragments = value;
        }

        public override double ProducedCurrency
        {
            get => Currencies.EntropyFragments;
            set => Currencies.EntropyFragments = value;
        }

        public override bool AutoBuy => false;
    }
}