using Blindsided.SaveData;
using static Blindsided.Oracle;

namespace FoundationOfProgressNameSpace
{
    public static class FoundationOfProductionStaticReferences
    {
        public static FopBuildings FopBuildings =>
            oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.Prestige.ResurgenceResets.FopBuildings;

        public static FopStats Stats => oracle.saveData.FoundationOfProgressSaveDataData.Stats;

        public static FopCurrencies FopCurrencies =>
            oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.Prestige.ResurgenceResets.Currencies;

        public static ResurgenceResets ResurgenceResetData
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.Prestige.ResurgenceResets;
            set => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.Prestige.ResurgenceResets = value;
        }

        public static bool Prestiged
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.Prestiged;
            set => oracle.saveData.FoundationOfProgressSaveDataData.Prestiged = value;
        }

        public static long TimesPrestiged
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.TimesPrestiged;
            set => oracle.saveData.FoundationOfProgressSaveDataData.TimesPrestiged = value;
        }

        public static bool Ascended
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.Ascended;
            set => oracle.saveData.FoundationOfProgressSaveDataData.Ascended = value;
        }

        public static long TimesAscended
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.TimesAscended;
            set => oracle.saveData.FoundationOfProgressSaveDataData.TimesAscended = value;
        }

        public static long TotalResets => oracle.saveData.FoundationOfProgressSaveDataData.TotalResets;

        public static bool AdditionalBuildings
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.AdditionalBuildings;
            set => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.AdditionalBuildings = value;
        }

        public static FopAscendancy Ascendancy => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy;

        public static double ResurgenceEnergy
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.Prestige.ResurgenceEnergy;
            set => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.Prestige.ResurgenceEnergy = value;
        }

        public static double ContinuumShards
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.ContinuumShards;
            set => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.ContinuumShards = value;
        }

        public static double StarCradles => FopBuildings.StarCradle.NumberOwned;

        public static double ResurgenceProductionMultiplier
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.Prestige.ProductionMultiplier;
            set => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.Prestige.ProductionMultiplier = value;
        }

        public static double ContinuumProductionBuff
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.ProductionExponent;
            set => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.ProductionExponent = value;
        }

        public static double ContinuumRealmOfResearchMultiplier
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.RealmOfResearchMultiplier;
            set => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.RealmOfResearchMultiplier = value;
        }

        public static bool BuildingFoldoutPreference
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.BuildingFoldoutPreference;
            set => oracle.saveData.FoundationOfProgressSaveDataData.BuildingFoldoutPreference = value;
        }

        public static bool HideAutomatedBuildings
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.HideAutomatedBuildings;
            set => oracle.saveData.FoundationOfProgressSaveDataData.HideAutomatedBuildings = value;
        }

        public static float AutoPrestigeSavedValue
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.AutoPrestigeSavedValue;
            set => oracle.saveData.FoundationOfProgressSaveDataData.AutoPrestigeSavedValue = value;
        }

        public static bool AutoPrestigeEnabled
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.AutoPrestigeEnabled;
            set => oracle.saveData.FoundationOfProgressSaveDataData.AutoPrestigeEnabled = value;
        }

        public static bool AutoBuyEnabled
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.AutoBuyEnabled;
            set => oracle.saveData.FoundationOfProgressSaveDataData.AutoBuyEnabled = value;
        }
    }
}