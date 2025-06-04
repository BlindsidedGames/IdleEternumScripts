using Blindsided.SaveData;
using static Blindsided.Oracle;

namespace RealmOfResearchNamespace
{
    public static class RealmOfResearchStaticReferences
    {
        public static RorBuildings RorBuildings =>
            oracle.saveData.RealmOfResearchSaveDataData.AllRorBuildings;

        public static RorCurrencies Currencies =>
            oracle.saveData.RealmOfResearchSaveDataData.Currencies;

        public static bool ResearchBuildingFoldoutPreference
        {
            get => oracle.saveData.RealmOfResearchSaveDataData.ResearchBuildingFoldoutPreference;
            set => oracle.saveData.RealmOfResearchSaveDataData.ResearchBuildingFoldoutPreference = value;
        }

        public static bool HideMaxedResearches
        {
            get => oracle.saveData.RealmOfResearchSaveDataData.HideMaxedResearches;
            set => oracle.saveData.RealmOfResearchSaveDataData.HideMaxedResearches = value;
        }

        public static int UpgradeDropdownIndex
        {
            get => oracle.saveData.RealmOfResearchSaveDataData.UpgradeDropdownIndex;
            set => oracle.saveData.RealmOfResearchSaveDataData.UpgradeDropdownIndex = value;
        }
    }
}