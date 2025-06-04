// Keep for StatType enum etc.

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
    public class ChronotonDrill : UpgradableFacility
    {
        public ResonanceChamber resonanceChamber;
        public double baseCost = 100000;
        public double costExponent = 1.1;
        private double CostMultiplier => GetStat(StatType.EoECostMultiplier).CachedValue;
        private double Duration => GetStat(StatType.EoEBaseDuration).CachedValue;

        private double Production => GetStat(StatType.EoEBaseProduction).CachedValue * ChronotonDrillLevel *
                                     resonanceChamber.CurrentBuffValue;

        private double ProgressPerSecond => GetStat(StatType.EoEProgressPerSecond).CachedValue;

        public bool Affordable => ResurgenceEnergy >= Cost();
        public string AffordableString => Affordable ? ColourHighlight : ColourRed;

        // Keep Start for specific initialization if needed (like listeners)
        private void Start()
        {
            purchaseButton.onClick.AddListener(PurchaseBuildings);
            // Initial UI update might be needed if not called elsewhere on game start
            UpdateUI();
        }

        // Implement Produce from base class
        public override void Produce(float deltaTime)
        {
            var progress = ChronotonDrillLevel > 0 ? deltaTime * ProgressPerSecond : 0;
            ChronotonDrillProgress += progress;
            if (ChronotonDrillProgress >= Duration)
            {
                var timesCompleted = Math.Floor(ChronotonDrillProgress / Duration);
                ChronotonDrillProgress -= timesCompleted * Duration;
                Chronotons += Production * timesCompleted;
            }
        }

        #region User Interface

        public TMP_Text progressText;
        public TMP_Text countText;
        public TMP_Text chronotonCountText;
        public TMP_Text costText;
        public MPImage progressBar;
        public Button purchaseButton;
        public TMP_Text purchaseButtonText;

        // Implement UpdateUI from base class
        public override void UpdateUI()
        {
            // Ensure GetStat doesn't return null before accessing CachedValue if there's a chance stats aren't ready
            // Adding null checks or default values might be safer depending on initialization order.
            var currentDuration = Duration; // Cache value for fillAmount calculation
            purchaseButton.interactable = Cost() <= ResurgenceEnergy;
            countText.text = $"{ColourGreen}{FormatNumber(ChronotonDrillLevel)}{EndColour}";
            purchaseButtonText.text = $"Buy ({PurchaseAmount()})";
            progressBar.fillAmount = currentDuration > 0 ? (float)(ChronotonDrillProgress / currentDuration) : 0f;
            progressText.text =
                $"<b>{ColourGreen}{FormatNumber(Production)}{EndColour} Chronotons</b> | {ColourGreen}{FormatTimeRemaining(currentDuration - ChronotonDrillProgress, true, na: false)}{EndColour}";
            costText.text =
                $"<b>Cost</b> | {AffordableString}{FormatNumber(ResurgenceEnergy)}{EndColour}/ {AffordableString}{FormatNumber(Cost())}{EndColour} {ColourGrey}Resurgence Energy{EndColour}";
            chronotonCountText.text =
                $"<b>Chronotons</b> | {ColourGreen}{FormatNumber(Chronotons)}{EndColour}";
        }

        public void PurchaseBuildings()
        {
            var tempCost = Cost();
            if (ResurgenceEnergy < tempCost) return;
            ChronotonDrillLevel += PurchaseAmount();
            ResurgenceEnergy -= tempCost;
            UpdateUI(); // Update UI after purchase
        }

        public double Cost()
        {
            return BuyXCost(PurchaseAmount(), baseCost, costExponent, ChronotonDrillLevel,
                CostMultiplier);
        }

        public int PurchaseAmount()
        {
            return PurchaseMode switch
            {
                BuyMode.Buy1 => 1,
                BuyMode.Buy10 => RoundedBulkBuy ? 10 - (int)Math.Floor(ChronotonDrillLevel % 10) : 10,
                BuyMode.Buy50 => RoundedBulkBuy ? 50 - (int)Math.Floor(ChronotonDrillLevel % 50) : 50,
                BuyMode.Buy100 => RoundedBulkBuy ? 100 - (int)Math.Floor(ChronotonDrillLevel % 100) : 100,
                BuyMode.BuyMax => Math.Max(1,
                    MaxAffordable(ResurgenceEnergy, baseCost, costExponent, ChronotonDrillLevel,
                        CostMultiplier)),
                _ => 1
            };
        }

        #endregion

        #region IUpgradable Implementation (Now Base Class)

        // Field to hold the specific tags for this facility
        [SerializeField] private List<string> tags = new() { "EoECD" };

        // Implement abstract property from base class
        protected override List<string> FacilityTags => tags;

        // Define base values for stats specific to this facility
        private readonly double _baseDuration = 50;
        private readonly double _baseProduction = 0.1;
        private readonly double _costMultiplier = 1;
        private readonly double _progressPerSecond = 1;

        // Implement abstract method from base class to set up specific stats
        protected override void SetUpSpecificVariables()
        {
            // Use the UpgradableStats dictionary inherited from the base class
            UpgradableStats[StatType.EoEBaseDuration] = new UpgradableStat { baseValue = _baseDuration };
            UpgradableStats[StatType.EoEBaseProduction] = new UpgradableStat { baseValue = _baseProduction };
            UpgradableStats[StatType.EoECostMultiplier] = new UpgradableStat { baseValue = _costMultiplier };
            UpgradableStats[StatType.EoEProgressPerSecond] = new UpgradableStat { baseValue = _progressPerSecond };
        }

        // OnEnable, OnDisable, Register, Unregister, GetStat, GetTags are handled by the base class

        #endregion
    }
}