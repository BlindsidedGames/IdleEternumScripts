using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Blindsided.SaveData
{
    [HideReferenceObjectPicker]
    public class RealmOfResearchSaveData
    {
        public RorCurrencies Currencies = new();
        public RorBuildings AllRorBuildings = new();
        public RorStats Stats = new();

        public bool ResearchBuildingFoldoutPreference = true;
        public int UpgradeDropdownIndex = 0;
        public bool HideMaxedResearches = true;
        public Dictionary<string, int> UpgradesOwned = new();
    }

    [HideReferenceObjectPicker]
    public class RorCurrencies
    {
        private double entropyFragments;

        public double EntropyFragments
        {
            get => entropyFragments;
            set
            {
                if (double.IsNaN(value) || value > double.MaxValue)
                    entropyFragments = double.MaxValue;
                else if (value < 0)
                    entropyFragments = 0;
                else
                    entropyFragments = value;
            }
        }
    }

    [HideReferenceObjectPicker]
    public class RorBuildings
    {
        public RorBuilding VoidScribe = new() { NumberCreated = 1 };
        public RorBuilding FractureLoom = new();
        public RorBuilding DarkArchitect = new();
        public RorBuilding CollapseEngine = new();
        public RorBuilding SingularityVault = new();
    }

    [HideReferenceObjectPicker]
    public class RorStats
    {
        public RorBuildingStats VoidScribe = new();
        public RorBuildingStats FractureLoom = new();
        public RorBuildingStats DarkArchitect = new();
        public RorBuildingStats CollapseEngine = new();
        public RorBuildingStats SingularityVault = new();
    }


    [HideReferenceObjectPicker]
    public class RorBuilding
    {
        public double NumberOwned => NumberCreated + NumberPurchased;
        public double NumberCreated;
        public double NumberPurchased = 0;
    }

    [HideReferenceObjectPicker]
    public class RorBuildingStats
    {
        public double TotalPurchased;
        public double TotalCreated;
    }
}