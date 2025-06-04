using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UpgradeSystem;

namespace FoundationOfProgressNameSpace.BuildingScriptables
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "Game/Building Data")]
    public class BuildingData : SerializedScriptableObject
    {
        [FoldoutGroup("Strings")] public string buildingName;
        [FoldoutGroup("Cost Settings")] public double costExponent;

        [FoldoutGroup("Base Stats")] public double baseCost;
        [FoldoutGroup("Base Stats")] public double baseProduction;
        [FoldoutGroup("Base Stats")] public double baseCreation;

        [FoldoutGroup("Production Settings")] public bool creates = true;

        public Dictionary<StatType, UpgradableStat> UpgradableStats = new();

        private void Awake()
        {
            SetupSoStats();
        }

        public void CacheStats()
        {
            foreach (var stat in UpgradableStats) stat.Value.CacheStat();
        }

        private void SetupSoStats()
        {
            UpgradableStats.Clear();
            UpgradableStats.Add(StatType.FopCostMultiplier,
                new UpgradableStat { baseValue = 1, flipMultiplicativeColor = true });
            UpgradableStats.Add(StatType.FopCreation, new UpgradableStat { baseValue = baseCreation });
            UpgradableStats.Add(StatType.FopProduction, new UpgradableStat { baseValue = baseProduction });
        }


        [Button]
        public void ApplyBaseStats()
        {
            UpgradableStats[StatType.FopCostMultiplier].baseValue = 1;
            UpgradableStats[StatType.FopCreation].baseValue = baseCreation;
            UpgradableStats[StatType.FopProduction].baseValue = baseProduction;
        }

        public string ProducedCurrencyString(bool plural)
        {
            return plural ? pluralProducedCurrencyName : producedCurrencyName;
        }

        public string RequiredCurrencyString(bool plural)
        {
            return plural ? pluralRequiredCurrencyName : requiredCurrencyName;
        }

        public string ProducedBuildingString(bool plural)
        {
            return plural ? pluralProducedBuildingName : producedBuildingName;
        }


        [FoldoutGroup("Strings")] [Space(10)] public string producedCurrencyName;
        [FoldoutGroup("Strings")] public string requiredCurrencyName;
        [FoldoutGroup("Strings")] public string producedBuildingName;

        [FoldoutGroup("Strings")] [Space(10)] public string pluralProducedCurrencyName;
        [FoldoutGroup("Strings")] public string pluralRequiredCurrencyName;
        [FoldoutGroup("Strings")] public string pluralProducedBuildingName;

        public int sortOrder;
    }
}