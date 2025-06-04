// Added for Math.Min

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
using static Blindsided.SaveData.StaticReferences; // Chronotons, TimeScale, etc.

namespace EnginesOfExpansionNamespace.Engines
{
    public class TemporalAccelerator : UpgradableFacility
    {
        // ----------------- Inspector -----------------
        public TimeCore timeCore;
        public ChronotonDrill chronotonDrill;
        public double cost;

        [Header("UI")] public TMP_Text chargeText;
        public TMP_Text targetText;
        public Button targetButton;
        public TMP_Text costAndDescriptionText;
        public Button buyAndActivateButton;
        public TMP_Text buyAndActivateButtonText;
        public MPImage progressBar;

        // ----------------- Charging ------------------
        private double RequiredCharge => GetStat(StatType.EoERequiredCharge)?.CachedValue ?? double.MaxValue;
        [SerializeField] private double chargeRate = 1.0; // units-per-second (real-time)
        private double BuffDuration => GetStat(StatType.EoEBaseDuration)?.CachedValue ?? 0.0;

        // ----------------- Unity ---------------------
        private void Start()
        {
            targetButton.onClick.AddListener(ToggleTarget);
            SetTargetText();

            buyAndActivateButton.onClick.AddListener(() =>
            {
                if (TemporalAcceleratorUnlocked)
                {
                    ActivateBuff();
                }
                else if (Chronotons >= cost)
                {
                    TemporalAcceleratorUnlocked = true;
                    TemporalAcceleratorCharge = 0;
                    SetBuyAndActivateText();
                    Chronotons -= cost;
                    UpdateUI();
                }
            });

            SetBuyAndActivateText();
            UpdateUI();
            Register();
        }

        // ----------------- Production ----------------
        public override void Produce(float deltaTime)
        {
            if (!TemporalAcceleratorUnlocked || TemporalAcceleratorCharge >= RequiredCharge)
                return;

            TemporalAcceleratorCharge = Math.Min(
                TemporalAcceleratorCharge + chargeRate * deltaTime,
                RequiredCharge);

            UpdateUI();
        }

        private static void ResetTemporalAccelerator()
        {
            TemporalAcceleratorCharge = 0;
        }

        private void PerformTimeJump()
        {
            if (!TemporalAcceleratorUnlocked || TemporalAcceleratorCharge < RequiredCharge)
                return;

            if (timeCore == null || chronotonDrill == null)
            {
                Debug.LogError("Temporal Accelerator targets not assigned!");
                return;
            }

            var effectiveJump = (float)(BuffDuration * TimeScale);
            if (TemporalAcceleratorTarget) timeCore.Produce(effectiveJump);
            else chronotonDrill.Produce(effectiveJump);

            ResetTemporalAccelerator();
            UpdateUI();
        }

        private void ActivateBuff()
        {
            PerformTimeJump();
        }

        // ----------------- UI helpers ----------------
        private void SetBuyAndActivateText()
        {
            buyAndActivateButtonText.text = TemporalAcceleratorUnlocked ? "Jump" : "Buy";
        }

        private void ToggleTarget()
        {
            TemporalAcceleratorTarget = !TemporalAcceleratorTarget;
            SetTargetText();
            UpdateUI();
        }

        private void SetTargetText()
        {
            targetText.text = "Target | " +
                              (TemporalAcceleratorTarget
                                  ? $"{ColourGreen}TC{EndColour}"
                                  : $"{ColourHighlight}CD{EndColour}");
        }

        // *** NEW: Countdown-style read-out ***
        private string _chargeText
        {
            get
            {
                if (!TemporalAcceleratorUnlocked)
                    return "<b>Locked</b>";

                // Treat tiny float errors as "full"
                const double epsilon = 1e-6;
                if (TemporalAcceleratorCharge >= RequiredCharge - epsilon)
                    return $"<b>{ColourGreen}Charged{EndColour}</b>";

                var secondsRemaining = (RequiredCharge - TemporalAcceleratorCharge) / chargeRate;
                var timeString = FormatTime(secondsRemaining, true, shortForm: false);

                return $"<b>Charged in</b> | {ColourGreen}{timeString}{EndColour}";
            }
        }


        private string _costAndDescriptionText => TemporalAcceleratorUnlocked
            ? "Accumulates charge in real time (1 unit / sec). When fully charged, jump " +
              $"{ColourOrange}{(TemporalAcceleratorTarget ? "Time Core" : "Chronoton Drill")}{EndColour} " +
              $"{ColourGreen}{FormatTime(BuffDuration, true, shortForm: false)}{EndColour} into the future."
            : $"<b>Cost</b> | {AffordableString}{FormatNumber(Chronotons)}{EndColour} / " +
              $"{AffordableString}{FormatNumber(cost)}{EndColour} {ColourGrey}Chronotons{EndColour}";

        private bool Affordable => Chronotons >= cost;
        private string AffordableString => Affordable ? ColourHighlight : ColourRed;

        public override void UpdateUI()
        {
            var requiredCharge = RequiredCharge;

            chargeText.text = _chargeText;
            costAndDescriptionText.text = _costAndDescriptionText;
            progressBar.fillAmount = requiredCharge > 0 ? (float)(TemporalAcceleratorCharge / requiredCharge) : 0f;

            buyAndActivateButton.interactable = TemporalAcceleratorUnlocked
                ? TemporalAcceleratorCharge >= requiredCharge // “Jump” button
                : Chronotons >= cost; // “Buy” button

            targetButton.interactable = TemporalAcceleratorUnlocked;
        }

        // ----------------- Upgradable ----------------
        [SerializeField] private List<string> tags = new() { "EoETA" };
        protected override List<string> FacilityTags => tags;

        private readonly double _baseCharge = 10; // seconds to full charge
        private readonly double _baseDuration = 15;
        private readonly double _baseBuff = 2; // unused

        protected override void SetUpSpecificVariables()
        {
            UpgradableStats[StatType.EoERequiredCharge] = new UpgradableStat { baseValue = _baseCharge };
            UpgradableStats[StatType.EoEBaseDuration] = new UpgradableStat { baseValue = _baseDuration };
            UpgradableStats[StatType.EoEBuffValue] = new UpgradableStat { baseValue = _baseBuff };
        }
    }
}