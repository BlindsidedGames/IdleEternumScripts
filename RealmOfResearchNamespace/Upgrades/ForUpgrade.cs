using System.Collections.Generic;
using Blindsided.SaveData;
using Sirenix.OdinInspector;
using UnityEngine;
using static Blindsided.Utilities.CalcUtils;
using static RealmOfResearchNamespace.RealmOfResearchStaticReferences;

namespace RealmOfResearchNamespace.Upgrades
{
    [CreateAssetMenu(fileName = "BuildingUpgrade", menuName = "Foundation Of Research/Upgrade")]
    public class ForUpgrade : SerializedScriptableObject
    {
        [FoldoutGroup("Strings")] public string upgradeName;

        [FoldoutGroup("Strings")] [TextArea(5, 10)]
        public string upgradeDescription;

        [FoldoutGroup("Cost")] public double upgradeBaseCost;
        public bool IsRepeatable => maxLevel > 1;
        [FoldoutGroup("Cost")] public double upgradeCostExponent;
        [FoldoutGroup("Cost")] public int maxLevel = 1;

        [FoldoutGroup("UpgradeApplication")] public List<UpgradeData> UpgradeData = new() { new UpgradeData() };

        [HideInInspector] public int owned;
        [HideInInspector] public GameObject instantiatedReference;
        public bool unlocked;
        public int sortOrder;

        public void SetActive()
        {
            if (instantiatedReference == null) return;
            var hideResearch = HideMaxedResearches && IsMaxLevel;
            var showResearch = !hideResearch && unlocked;
            instantiatedReference.SetActive(showResearch);
        }

        public bool IsMaxLevel => owned >= maxLevel;

        public bool CanAfford(double currency)
        {
            if (owned >= maxLevel) return false;
            return currency >= Cost();
        }

        public double Cost()
        {
            return BuyXCost(1, upgradeBaseCost, upgradeCostExponent, owned);
        }

        public void ApplyUpgrade()
        {
            SetActive();
            if (owned == 0) return;
        }
    }

    public class UpgradeData
    {
        public bool OtherUpgrade;
        public bool AffectsAllBuildings;
        private bool HideBuildingType => OtherUpgrade || AffectsAllBuildings;

        [HideIf("HideBuildingType")] public BuildingType BuildingType;

        [HideIf("OtherUpgrade")] public EffectType EffectType;
        [ShowIf("OtherUpgrade")] public OtherEffects OtherEffect;
        public double EffectAmount;
    }


    public class AppliedUpgrade
    {
        public string UpgradeName;
        public double UpgradeValue;
        public int Level;
        public double TotalValue => UpgradeValue * Level;
    }

    public enum EffectType
    {
        Multiply,
        CostMultiplier,
        BaseProduction,
        BaseCreation
    }

    public enum OtherEffects
    {
        MaxForwardTime,
        MaxBackwardTime
    }
}