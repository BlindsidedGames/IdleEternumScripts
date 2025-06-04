using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using static Blindsided.SaveData.StaticReferences;
using static UpgradeSystem.CurrencyManager;
using static Blindsided.Utilities.CalcUtils;

namespace UpgradeSystem
{
    [CreateAssetMenu(menuName = "Upgrade System/" +
                                "Upgrade")]
    public class UpgradeSo : ScriptableObject
    {
        [SerializeField] public string guid; // Unique identifier
        [FoldoutGroup("For Lauren")] public string upgradeName;
        [TextArea(3, 10)] public string description;
        public CurrencyType costCurrencyType;
        public double baseCost = 10;
        public double costMultiplier = 1.15;
        [FoldoutGroup("For Lauren")] public int maxPurchases = -1; // -1 for unlimited


        public List<EffectGroup> effectGroups; // List of effect groups for different entities

        public bool isUnlocked; // Managed by UnlockManager

#if UNITY_EDITOR
        [FoldoutGroup("For Lauren")]
        [Button]
        public void SetAssetName()
        {
            var assetPath = AssetDatabase.GetAssetPath(this);
            AssetDatabase.RenameAsset(assetPath, upgradeName);
            AssetDatabase.SaveAssets();
            name = upgradeName;
        }
#endif

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(guid))
                GenerateGuid();
        }

        [Button]
        public void GenerateGuid()
        {
            guid = Guid.NewGuid().ToString(); // Generate GUID once
        }

        public int GetCurrentLevel()
        {
            UpgradeLevels.TryGetValue(guid, out var level);
            return level;
        }

        public double GetCurrentCost(int currentLevel)
        {
            return BuyXCost(1, baseCost, costMultiplier, currentLevel);
        }

        public double GetCurrentCost()
        {
            return BuyXCost(1, baseCost, costMultiplier, GetCurrentLevel());
        }


        public bool CanAfford()
        {
            var cost = GetCurrentCost(GetCurrentLevel());
            return CheckAffordability(cost, costCurrencyType);
        }

        public bool CanAfford(double amount)
        {
            var cost = GetCurrentCost(GetCurrentLevel());
            return cost < amount;
        }

        public bool CheckPurchasable()
        {
            UpgradeLevels.TryGetValue(guid, out var level);
            var cost = GetCurrentCost(level);
            return !(maxPurchases >= 0 && level >= maxPurchases) && CheckAffordability(cost, costCurrencyType);
        }

        public void ResetUpgrade()
        {
            UpgradeManager.Instance.ResetUpgrade(this);
        }

        public bool IsMaxed
        {
            get
            {
                UpgradeLevels.TryGetValue(guid, out var level);
                return maxPurchases >= 0 && level >= maxPurchases;
            }
        }
    }

    [Serializable]
    public class EffectGroup
    {
        public List<string> tags; // Tags to identify target entities (e.g., "BuildingA")
        public List<Modification> modifications; // Effects to apply (e.g., production boost)
    }

    [Serializable]
    public class Modification
    {
        public StatType statType;
        public ModifierType modifierType;
        public double baseValue; // e.g., 1 for additive, 1.1 for 10% multiplicative
    }

    public enum StatType
    {
        FopProduction,
        FopCreation,
        FopCostMultiplier,
        MaxForwardsTime,
        MaxBackwardsTime,
        MaxTime,
        EoEBaseDuration,
        EoEPrestigeAmount,
        EoECompletionsRequired,
        EoEConverstionThreshold,
        IdsCostMultiplier,
        IdsProduction,
        IdsPanelLifetime,
        IdsResearchPerBot,
        EoEBaseProduction,
        EoECostMultiplier,
        EoERequiredCharge,
        EoEProgressPerSecond,
        EoEBuffValue,
        TimeScale,
        RorTimeScale,
        EoeTimeScale,
        CotTimeScale,
        CaTimeScale,
        TrTimeScale,
        IdsCashMultiplier,
        IdsScienceMultiplier,
        IdsAutoResearch,
        StartingIdsAssemblyLines,
        StartingIdsAiManagers,
        StartingIdsServers,
        StartingIdsDataCenters,
        StartingIdsPlanets,
        EssenceConverterDuration,
        EssenceConverterGainMultiplier,
        EssenceSynthesisCostIncrease,
        EssenceSynthesisCostDecrease
    }

    public enum ModifierType
    {
        Additive,
        Multiplicative
    }
}