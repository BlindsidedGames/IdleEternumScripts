// Added for Math

using System;
using System.Collections.Generic;
using MPUIKIT;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;
using static EnginesOfExpansionNamespace.EnginesOfExpansionStaticReferences;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;
using static Blindsided.SaveData.TextColourStrings;
using static Blindsided.Utilities.CalcUtils;
using static Blindsided.SaveData.SaveData;
using static Blindsided.SaveData.StaticReferences;

namespace EnginesOfExpansionNamespace.Engines
{
    // Inherits from the new base class
    public class TimeCore : UpgradableFacility
    {
        // Keep reference to Energy Amplifier
        public EnergyAmplifier energyAmplifier;
        public ResonanceChamber resonanceChamber;

        public double baseCost;
        public double costExponent;

        // Properties using GetStat from base class
        private double CostMultiplier => GetStat(StatType.EoECostMultiplier)?.CachedValue ?? 1.0;
        private double Duration => GetStat(StatType.EoEBaseDuration)?.CachedValue ?? double.MaxValue;

        private double BaseProduction => GetStat(StatType.EoEBaseProduction)?.CachedValue ?? 0.0;

        // Ensure energyAmplifier reference is checked before accessing CurrentBuffValue
        private double Production => BaseProduction * TimeCoreLevel *
                                     (energyAmplifier != null
                                         ? energyAmplifier.CurrentBuffValue *
                                           resonanceChamber.CurrentBuffValue
                                         : 1.0);

        private double ProgressPerSecond => GetStat(StatType.EoEProgressPerSecond)?.CachedValue ?? 1.0;

        public bool Affordable => ResurgenceEnergy >= Cost();
        public string AffordableString => Affordable ? ColourHighlight : ColourRed;

        // Keep Start for specific initialization
        private void Start()
        {
            // Ensure energyAmplifier is assigned in the inspector or via code
            if (energyAmplifier == null) Debug.LogError("EnergyAmplifier reference not set on TimeCore!", this);
            purchaseButton.onClick.AddListener(PurchaseBuildings);
            // Initial UI update
            UpdateUI();
        }

        // Implement Produce from base class
        public override void Produce(float deltaTime)
        {
            var progress = TimeCoreLevel > 0 ? deltaTime * ProgressPerSecond : 0;
            TimeCoreProgress += progress;
            var cycleCompleted = false;
            if (TimeCoreProgress >= Duration) // Use while loop for offline progress catching
            {
                var timesToProduce = (int)(TimeCoreProgress / Duration);
                TimeCoreProgress -= timesToProduce * Duration;
                ResurgenceEnergy += timesToProduce * Production; // Use Production property which includes buff
                cycleCompleted = true;
            }

            if (cycleCompleted || progress > 0) // Update UI if progress occurred or cycle completed
                UpdateUI();
        }

        #region User Interface

        public TMP_Text progressText;
        public TMP_Text costText;
        public TMP_Text countText;
        public MPImage progressBar;
        public Button purchaseButton;
        public TMP_Text purchaseButtonText;

        // Implement UpdateUI from base class
        public override void UpdateUI()
        {
            var currentDuration = Duration; // Cache value

            purchaseButton.interactable = Cost() <= ResurgenceEnergy;
            countText.text = $"{ColourGreen}{FormatNumber(TimeCoreLevel)}{EndColour}";
            purchaseButtonText.text = $"Buy ({PurchaseAmount()})";
            progressBar.fillAmount = currentDuration > 0 && currentDuration != double.MaxValue
                ? (float)(TimeCoreProgress / currentDuration)
                : 0f; // Avoid division by zero/inf
            progressText.text =
                $"<b>{ColourGreen}{FormatNumber(Production)}{EndColour} Resurgence Energy</b> | {ColourGreen}{FormatTimeRemaining(currentDuration - TimeCoreProgress, true, na: false)}{EndColour}";
            costText.text =
                $"<b>Cost</b> | {AffordableString}{FormatNumber(ResurgenceEnergy)}{EndColour}/ {AffordableString}{FormatNumber(Cost())}{EndColour} {ColourGrey}Resurgence Energy{EndColour}";
        }

        public void PurchaseBuildings()
        {
            var tempCost = Cost();
            if (ResurgenceEnergy < tempCost) return;
            TimeCoreLevel += PurchaseAmount();
            ResurgenceEnergy -= tempCost;
            UpdateUI(); // Update UI after purchase
        }

        public double Cost()
        {
            var costMultiplier = CostMultiplier; // Use property which includes null check/default
            return BuyXCost(PurchaseAmount(), baseCost, costExponent, TimeCoreLevel,
                costMultiplier);
        }

        public int PurchaseAmount()
        {
            var costMultiplier = CostMultiplier; // Use property
            return PurchaseMode switch
            {
                BuyMode.Buy1 => 1,
                BuyMode.Buy10 => RoundedBulkBuy ? 10 - (int)Math.Floor(TimeCoreLevel % 10) : 10,
                BuyMode.Buy50 => RoundedBulkBuy ? 50 - (int)Math.Floor(TimeCoreLevel % 50) : 50,
                BuyMode.Buy100 => RoundedBulkBuy ? 100 - (int)Math.Floor(TimeCoreLevel % 100) : 100,
                BuyMode.BuyMax => Math.Max(1,
                    MaxAffordable(ResurgenceEnergy, baseCost, costExponent, TimeCoreLevel,
                        costMultiplier)),
                _ => 1
            };
        }

        #endregion


        #region IUpgradable Implementation (Now Base Class)

        [SerializeField] private List<string> tags = new() { "EoETC" };
        protected override List<string> FacilityTags => tags;

        private readonly double _baseDuration = 50;
        private readonly double _baseProduction = 100;
        private readonly double _costMultiplier = 1;
        private readonly double _progressPerSecond = 1;

        protected override void SetUpSpecificVariables()
        {
            UpgradableStats[StatType.EoEBaseDuration] = new UpgradableStat { baseValue = _baseDuration };
            UpgradableStats[StatType.EoEBaseProduction] = new UpgradableStat { baseValue = _baseProduction };
            UpgradableStats[StatType.EoECostMultiplier] = new UpgradableStat { baseValue = _costMultiplier };
            UpgradableStats[StatType.EoEProgressPerSecond] = new UpgradableStat { baseValue = _progressPerSecond };
        }

        #endregion
    }
}