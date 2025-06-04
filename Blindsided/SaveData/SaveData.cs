using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnlockNexus;

namespace Blindsided.SaveData
{
    public class SaveData
    {
        public double PlayTime;
        public string DateStarted;
        public string DateQuitString;
        public double OfflineTime = 0;
        public double OfflineTimeScaleMultiplier = 2f;
        public double OfflineTimeCap = 3600f;

        public float TimeScale = 0f;
        public float CurrentTime = 0;

        [TabGroup("Preferences")] public Preferences SavedPreferences = new();
        [TabGroup("Devoptions")] public Devoptions DevOptions = new();


        [HideReferenceObjectPicker] [TabGroup("Foundation Of Progress")]
        public FoundationOfProgressSaveData FoundationOfProgressSaveDataData = new();

        [HideReferenceObjectPicker] [TabGroup("Realm Of Research")]
        public RealmOfResearchSaveData RealmOfResearchSaveDataData = new();

        [HideReferenceObjectPicker] [TabGroup("Engines Of Expansion")]
        public EnginesOfExpansionSaveData EnginesOfExpansionSaveDataData = new();

        [HideReferenceObjectPicker] [TabGroup("Collapse Of Time")]
        public CollapseOfTimeSaveData CollapseOfTimeSaveDataData = new();

        [HideReferenceObjectPicker] [TabGroup("Chronicle Archives")]
        public ChronicleArchivesSaveData ChronicleArchivesSaveDataData = new();

        [HideReferenceObjectPicker] [TabGroup("Temporal Rifts")]
        public TemporalRiftSaveData TemporalRiftSaveData = new();


        [HideReferenceObjectPicker] [TabGroup("Unlock Nexus")]
        public Dictionary<UnlockManager.UnlockKeys, bool> Unlocks = new();

        [HideReferenceObjectPicker] [TabGroup("UpgradeSystem")]
        public Dictionary<string, int> UpgradeLevels = new();

        [HideReferenceObjectPicker] [TabGroup("Statistics")]
        public Statistics Stats = new();

        [HideReferenceObjectPicker]
        public class Preferences
        {
            public Tab LayerTab = Tab.Zero;
            public bool ExtraBuyOptions = true;
            public NumberTypes Notation;
            public BuyMode BuyMode = BuyMode.BuyMax;
            public bool RoundedBulkBuy = true;
            public bool OfflineTimeActive;
            public bool OfflineTimeAutoDisable;
            public bool UseScaledTimeForValues;

            public bool ShortLongCurrencyDisplay;
            public bool TransparentUi;
            public bool InvertMenu;
            public bool Tutorial;
            public bool Music = true;

            public bool StatsFoldout;
            public bool SettingsFoldout;
            public bool ShopFoldout = false;
            public Dictionary<string, bool> Foldouts = new();
        }

        [HideReferenceObjectPicker]
        public class Statistics
        {
            public TimeSpentInRealms TimeSpentInRealms = new();
            public TimeSpentInRealms ScaledTimeSpentInRealms = new();
        }

        [HideReferenceObjectPicker]
        public class TimeSpentInRealms
        {
            public float Total => EventHorizon + FoundationOfProduction + RealmOfResearch + EnginesOfExpansion +
                                  CollapseOfTime + ChronicleArchives + TemporalRifts + VoidLull;

            public float EventHorizon;
            public float FoundationOfProduction;
            public float RealmOfResearch;
            public float EnginesOfExpansion;
            public float CollapseOfTime;
            public float ChronicleArchives;
            public float TemporalRifts;
            public float VoidLull;
        }

        [HideReferenceObjectPicker]
        public class Devoptions
        {
            public bool DevSpeed;
        }

        #region Enums

        #region UI

        public enum Tab
        {
            Zero,
            RealmOfResearch,
            FoundationOfProduction,
            CollapseOfTime,
            EnginesOfExpansion,
            TemporalRifts,
            ChronicleArchives
        }

        public enum BuyMode
        {
            Buy1,
            Buy10,
            Buy50,
            Buy100,
            BuyMax
        }

        public enum NumberTypes
        {
            Standard,
            Scientific,
            Engineering
        }

        public enum SideSetting
        {
            Left,
            Right
        }

        public enum SaveFile
        {
            Unset,
            Standard,
            Hardcore,
            Duplicator,
            Replicator
        }

        public enum Tier
        {
            Tier1,
            Tier2,
            Tier3,
            Tier4,
            Tier5,
            Tier6
        }

        public enum StoryState
        {
            Prologue,
            Chapter1,
            Chapter2,
            Chapter3,
            Chapter4,
            Chapter5,
            Chapter6,
            Chapter7,
            Chapter8,
            End
        }

        #endregion

        #endregion
    }
}