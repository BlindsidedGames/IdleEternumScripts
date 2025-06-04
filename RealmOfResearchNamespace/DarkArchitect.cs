using Blindsided.SaveData;
using static RealmOfResearchNamespace.RealmOfResearchStaticReferences;

namespace RealmOfResearchNamespace
{
    public class DarkArchitect : Researcher
    {
        public override RorBuilding BuildingData
        {
            get => RealmOfResearchStaticReferences.RorBuildings.DarkArchitect;
            set => RealmOfResearchStaticReferences.RorBuildings.DarkArchitect = value;
        }

        public override RorBuilding BuildingCreatedData
        {
            get => RealmOfResearchStaticReferences.RorBuildings.FractureLoom;
            set => RealmOfResearchStaticReferences.RorBuildings.FractureLoom = value;
        }

        public override double RequiredCurrency
        {
            get => Currencies.EntropyFragments;
            set => Currencies.EntropyFragments = value;
        }

        public override bool AutoBuy => false;
    }
}