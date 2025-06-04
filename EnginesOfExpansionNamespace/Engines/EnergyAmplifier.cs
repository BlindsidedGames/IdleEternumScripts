// Ensure this using static directive correctly points to the class containing the AutoButtonCycleState property and enum

using System;
using System.Collections.Generic;
using MPUIKIT;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;
using static EnginesOfExpansionNamespace.EnginesOfExpansionStaticReferences;
using static Blindsided.SaveData.TextColourStrings;
using static Blindsided.Utilities.CalcUtils;
// Corrected: Using alias for AutoButtonCycleState from its actual namespace
using AutoButtonCycleState = Blindsided.SaveData.AutoButtonCycleState;

namespace EnginesOfExpansionNamespace.Engines
{
    public class EnergyAmplifier : UpgradableFacility
    {
        public double cost;
        public bool useRealTime = true;
        public TMP_Text chargeText;
        public TMP_Text autoText;
        public Button autoButton;
        public TMP_Text costAndDescriptionText;
        public Button buyAndActivateButton;
        public TMP_Text buyAndActivateButtonText;

        public MPImage progressBar;

        public double CurrentBuffValue =>
            EnergyAmplifierBuffActive ? GetStat(StatType.EoEBuffValue)?.CachedValue ?? 1.0 : 1;

        public double ToConsume => Chronotons / 100;

        private double RequiredCharge =>
            GetStat(StatType.EoERequiredCharge)?.CachedValue ?? double.MaxValue;

        private double BuffDuration =>
            GetStat(StatType.EoEBaseDuration)?.CachedValue ?? 0.0;

        private void Start()
        {
            autoButton.onClick.AddListener(ToggleAutoButtonState);

            ApplyCurrentAutoButtonSettings();
            SetAutoText();

            buyAndActivateButton.onClick.AddListener(() =>
            {
                if (EnergyAmplifierUnlocked)
                {
                    ActivateBuff();
                }
                else
                {
                    if (Chronotons >= cost)
                    {
                        EnergyAmplifierUnlocked = true;
                        EnergyAmplifierCharge = 0;
                        ResetEnergyAmplifier();
                        SetBuyAndActivateText();
                        Chronotons -= cost;
                        UpdateUI();
                    }
                }
            });
            SetBuyAndActivateText();
            UpdateUI();
        }

        private void SetBuyAndActivateText()
        {
            buyAndActivateButtonText.text = EnergyAmplifierUnlocked
                ? "Activate"
                : "Buy";
        }

        private void ApplyCurrentAutoButtonSettings()
        {
            // Assuming EnginesOfExpansionStaticReferences.AutoButtonCycleState is the correct static property
            var currentState = EnginesOfExpansionStaticReferences.AutoButtonCycleState;

            switch (currentState)
            {
                case AutoButtonCycleState.ChargeDisabled:
                    EnergyAmplifierAutoActivate = false;
                    // Charging is handled in Produce method
                    break;
                case AutoButtonCycleState.AutoEnabled:
                    EnergyAmplifierAutoActivate = true;
                    break;
                case AutoButtonCycleState.AutoDisabled:
                    EnergyAmplifierAutoActivate = false;
                    break;
            }
        }

        private void ToggleAutoButtonState()
        {
            var currentState = EnginesOfExpansionStaticReferences.AutoButtonCycleState;
            var nextState =
                (AutoButtonCycleState)(((int)currentState + 1) % Enum.GetValues(typeof(AutoButtonCycleState)).Length);
            EnginesOfExpansionStaticReferences.AutoButtonCycleState = nextState;

            ApplyCurrentAutoButtonSettings();
            SetAutoText();
            UpdateUI();
        }

        private void SetAutoText()
        {
            var currentState = EnginesOfExpansionStaticReferences.AutoButtonCycleState;
            switch (currentState)
            {
                case AutoButtonCycleState.ChargeDisabled:
                    autoText.text = "Charge | " + ColourRed + "Disabled" + EndColour;
                    break;
                case AutoButtonCycleState.AutoEnabled:
                    autoText.text = "Auto | " + ColourGreen + "Enabled" + EndColour;
                    break;
                case AutoButtonCycleState.AutoDisabled:
                    autoText.text = "Auto | " + ColourRed + "Disabled" + EndColour;
                    break;
                default:
                    autoText.text = "Auto | Error";
                    break;
            }
        }

        public override void Produce(float deltaTime)
        {
            if (!EnergyAmplifierUnlocked || Chronotons <= 0)
                return;

            var stateChanged = false;
            // Get the current button state from the static reference
            var currentButtonState = EnginesOfExpansionStaticReferences.AutoButtonCycleState;

            // --- Charging Logic ---
            // MODIFIED: Only charge if not in ChargeDisabled state
            if (currentButtonState != AutoButtonCycleState.ChargeDisabled)
                if (EnergyAmplifierCharge < RequiredCharge)
                {
                    var amountToConsume = ToConsume >= RequiredCharge ? RequiredCharge : ToConsume;
                    var chargeToAdd = amountToConsume * deltaTime;

                    chargeToAdd = Math.Min(chargeToAdd, Chronotons * deltaTime);
                    EnergyAmplifierCharge += chargeToAdd;
                    Chronotons -= chargeToAdd;
                    EnergyAmplifierCharge = Math.Min(EnergyAmplifierCharge, RequiredCharge);
                    stateChanged = true;
                }
            // --- End Charging Logic ---

            // Auto-activation logic:
            // EnergyAmplifierAutoActivate is set by ApplyCurrentAutoButtonSettings based on currentButtonState.
            // This will be true only for AutoButtonCycleState.AutoEnabled.
            if (!EnergyAmplifierBuffActive && EnergyAmplifierAutoActivate && EnergyAmplifierCharge >= RequiredCharge)
            {
                ActivateBuff();
                stateChanged = true;
            }

            if (EnergyAmplifierBuffActive)
            {
                EnergyAmplifierBuffRemainingTime -= useRealTime ? Time.deltaTime : deltaTime;
                if (EnergyAmplifierBuffRemainingTime <= 0)
                {
                    ResetEnergyAmplifier();
                    stateChanged = true;
                }

                stateChanged = true; // Time is ticking down, so UI might need an update
            }

            if (stateChanged) UpdateUI();
        }

        private static void ResetEnergyAmplifier()
        {
            EnergyAmplifierBuffActive = false;
            EnergyAmplifierCharge = 0;
            EnergyAmplifierBuffRemainingTime = 0;
        }

        private void ActivateBuff()
        {
            if (!EnergyAmplifierUnlocked || EnergyAmplifierBuffActive || EnergyAmplifierCharge < RequiredCharge) return;

            EnergyAmplifierBuffActive = true;
            EnergyAmplifierBuffRemainingTime = BuffDuration;
            UpdateUI();
        }

        private string _chargeText => EnergyAmplifierUnlocked
            ? EnergyAmplifierBuffActive
                ? "<b>Time Core Multiplier | " +
                  $"{ColourGreen}{CurrentBuffValue:P0}{EndColour}</b> | {ColourGreen}{(useRealTime ? FormatTime(EnergyAmplifierBuffRemainingTime, true) : FormatTimeRemaining(EnergyAmplifierBuffRemainingTime, true))}{EndColour}"
                : $"<b>Charge</b> | {ColourGreen}{FormatNumber(EnergyAmplifierCharge)} / {FormatNumber(RequiredCharge)}{EndColour}"
            : "<b>Locked</b>";

        private string _costAndDescriptionText => EnergyAmplifierUnlocked
            ? "Consume 1% of your chronotons per second to charge the Energy Amplifier.\n" +
              $"When fully charged, it will buff {ColourOrange}Time Cores{EndColour} for {ColourGreen}{FormatTime(BuffDuration, true, shortForm: false)}{EndColour}"
            : $"<b>Cost</b> | {AffordableString}{FormatNumber(Chronotons)}{EndColour}/ {AffordableString}{FormatNumber(cost)}{EndColour} {ColourGrey}Chronotons{EndColour}";

        public bool Affordable => Chronotons >= cost;
        public string AffordableString => Affordable ? ColourHighlight : ColourRed;

        public override void UpdateUI()
        {
            var buffDuration = BuffDuration;
            var requiredCharge = RequiredCharge;

            chargeText.text = _chargeText;
            costAndDescriptionText.text = _costAndDescriptionText;
            progressBar.fillAmount = EnergyAmplifierBuffActive
                ? buffDuration > 0 ? (float)(EnergyAmplifierBuffRemainingTime / buffDuration) : 0f
                : requiredCharge > 0
                    ? (float)(EnergyAmplifierCharge / requiredCharge)
                    : 0f;

            buyAndActivateButton.interactable = EnergyAmplifierUnlocked
                ? !EnergyAmplifierBuffActive && EnergyAmplifierCharge >= requiredCharge && !EnergyAmplifierAutoActivate
                : Chronotons >= cost;

            autoButton.interactable = EnergyAmplifierUnlocked;
        }

        #region IUpgradable Implementation (Now Base Class)

        [SerializeField] private List<string> tags = new() { "EoEEA" };
        protected override List<string> FacilityTags => tags;

        private readonly double _baseCharge = 50;
        private readonly double _baseDuration = 15;
        private readonly double _baseBuff = 2;

        protected override void SetUpSpecificVariables()
        {
            UpgradableStats[StatType.EoERequiredCharge] = new UpgradableStat { baseValue = _baseCharge };
            UpgradableStats[StatType.EoEBaseDuration] = new UpgradableStat { baseValue = _baseDuration };
            UpgradableStats[StatType.EoEBuffValue] = new UpgradableStat { baseValue = _baseBuff };
        }

        #endregion
    }
}