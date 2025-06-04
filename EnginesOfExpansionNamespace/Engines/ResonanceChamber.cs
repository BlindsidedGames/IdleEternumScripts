// Assuming needed for progress bar/slider
// Assuming needed for text elements
// Assuming needed for Button and Slider
// Needed for IUpgradable and StatType

using System;
using System.Collections.Generic;
using MPUIKIT;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;
using static EnginesOfExpansionNamespace.EnginesOfExpansionStaticReferences; // Base path for static references
using static Blindsided.SaveData.TextColourStrings; // For text formatting
using static Blindsided.Utilities.CalcUtils; // For formatting numbers/time
// Using specific static classes or SaveData might be needed depending on where Chronotons and the static properties reside
// Assuming SaveData static access for Chronotons if not passed differently

// Assuming static properties are accessible here or directly via EoeResetOnPrestigeData


namespace EnginesOfExpansionNamespace.Engines
{
    // Inherits from the UpgradableFacility base class
    // Designed to closely match EnergyAmplifier structure, with mini-game modifications
    public class ResonanceChamber : UpgradableFacility
    {
        [Header("Core UI Elements")] public double cost = 1000000; // Example unlock cost
        public bool useRealTime = true; // Added flag to control time source for buff countdown
        public TMP_Text chargeText; // Displays charge level OR buff status
        public TMP_Text costAndDescriptionText; // Displays cost and description
        public Button buyAndActivateButton; // Handles Buy AND Stop Pendulum actions
        public TMP_Text buyAndActivateButtonText;
        public MPImage progressBar; // Standard progress bar for charging or buff duration

        [Header("Mini-Game Elements")]
        public GameObject miniGamePanel; // Parent GameObject for the mini-game UI (Slider)

        public Slider pendulumSlider; // Slider for the pendulum mini-game

        // --- Mini-Game Logic ---
        private bool _isMiniGameActive;
        private readonly float _pendulumSpeed = 1.0f; // Speed the slider moves back and forth (adjust as needed)
        private float _pendulumDirection = 1.0f;


        // --- Properties using GetStat (from base class) ---
        // Property for external access, mimicking EnergyAmplifier.CurrentBuffValue
        // Returns the stored multiplier if the buff is active, otherwise 1
        public double CurrentBuffValue => ResonanceChamberBuffActive ? ResonanceMultiplier : 1.0;

        // Consume 1% of Chronotons per second
        public double ToConsume => Chronotons * 0.01; // Requires access to current Chronotons count

        // Using exact StatTypes from EnergyAmplifier
        private double RequiredCharge => GetStat(StatType.EoERequiredCharge)?.CachedValue ?? double.MaxValue;

        private double BuffDuration => GetStat(StatType.EoEBaseDuration)?.CachedValue ?? 0.0;
        // Note: EoEBuffValue stat exists but isn't directly used for CurrentBuffValue,
        // as the mini-game determines the multiplier. It *could* be used as a base or for upgrades.


        // --- Initialization ---
        private void Start()
        {
            buyAndActivateButton.onClick.AddListener(() => // Combined Buy/Stop logic
            {
                if (!ResonanceChamberUnlocked)
                {
                    // Try to buy
                    if (Chronotons >= cost)
                    {
                        Chronotons -= cost; // Deduct cost
                        ResonanceChamberUnlocked = true;
                        ResetBuff(); // Initialize state (sets charge=0, active=false, time=0, multi=1)
                        _isMiniGameActive = false;
                        SetInitialStateUI(); // Set UI for unlocked state
                    }
                }
                else if (_isMiniGameActive)
                {
                    // If unlocked and mini-game is active, button STOPS the pendulum
                    StopPendulum();
                }
                // No action if unlocked but charging or buff active (button should be disabled)
            });

            SetInitialStateUI(); // Includes initial UpdateUI call
        }

        // Sets UI state right after purchase or on game start
        private void SetInitialStateUI()
        {
            miniGamePanel.SetActive(false);
            progressBar.gameObject.SetActive(true);
            UpdateUI(); // Update text, interactability etc. based on current loaded state
        }


        // --- Core Logic (Produce called each frame/tick) ---
        public override void Produce(float deltaTime) // deltaTime is game simulation time step
        {
            // Ensure Unlock status is checked based on the correctly loaded value
            if (!ResonanceChamberUnlocked) return;

            var stateChanged = false;

            // --- Buff Handling ---
            if (ResonanceChamberBuffActive)
            {
                // Use Time.deltaTime if useRealTime is true, otherwise use game simulation deltaTime
                ResonanceBuffRemainingTime -= useRealTime ? Time.deltaTime : deltaTime;
                if (ResonanceBuffRemainingTime <= 0)
                {
                    ResetBuff();
                    stateChanged = true;
                }
                else
                {
                    stateChanged = true; // Time is ticking
                }
            }
            // --- Mini-Game Handling ---
            else if (_isMiniGameActive)
            {
                // Pendulum speed should likely use Time.deltaTime for smooth visual movement regardless of game speed
                pendulumSlider.value += _pendulumDirection * _pendulumSpeed * Time.deltaTime;
                if (pendulumSlider.value >= pendulumSlider.maxValue || pendulumSlider.value <= pendulumSlider.minValue)
                {
                    _pendulumDirection *= -1; // Reverse direction
                    pendulumSlider.value = Mathf.Clamp(pendulumSlider.value, pendulumSlider.minValue,
                        pendulumSlider.maxValue); // Clamp value
                }

                stateChanged = true; // Slider is moving
            }
            // --- Charging Handling ---
            else // Not buffed, not in mini-game -> charge if possible
            {
                if (ResonanceCharge < RequiredCharge && Chronotons > 0)
                {
                    // Charging should use game simulation deltaTime
                    var amountToConsume = ToConsume >= RequiredCharge ? RequiredCharge : ToConsume;
                    var chronotonsToConsume = amountToConsume * deltaTime;
                    // Clamp consumption to available Chronotons
                    chronotonsToConsume = Math.Min(chronotonsToConsume, Chronotons);

                    if (chronotonsToConsume > 0)
                    {
                        // Match EnergyAmplifier: charge gain = consumption amount (proportional to game time)
                        var chargeToAdd = chronotonsToConsume;

                        ResonanceCharge += chargeToAdd;
                        Chronotons -= chronotonsToConsume;
                        ResonanceCharge = Math.Min(ResonanceCharge, RequiredCharge); // Clamp charge
                        stateChanged = true;
                    }
                }

                // Check if fully charged AFTER potentially adding charge this frame
                // Automatically start the mini-game when charge is full
                if (!ResonanceChamberBuffActive && !_isMiniGameActive && ResonanceCharge >= RequiredCharge)
                {
                    StartMiniGame();
                    stateChanged = true;
                }
            }

            // Update UI if any relevant state changed
            if (stateChanged) UpdateUI();
        }

        // --- State Management Methods ---

        // Resets buff state, charge, and multiplier
        private void ResetBuff()
        {
            ResonanceChamberBuffActive = false;
            ResonanceBuffRemainingTime = 0;
            ResonanceCharge = 0; // Reset charge when buff ends or on purchase
            ResonanceMultiplier = 1.0; // Reset multiplier to default
            _isMiniGameActive = false; // Ensure mini-game is not active

            // Ensure UI visibility is correct after reset
            progressBar.gameObject.SetActive(true);
            miniGamePanel.SetActive(false);
        }

        // Activates the buff state after mini-game completion
        private void ActivateBuff()
        {
            // Check if actually ready to activate (Should be called only from StopPendulum)
            if (!ResonanceChamberUnlocked || ResonanceChamberBuffActive || _isMiniGameActive) return;

            ResonanceChamberBuffActive = true;
            ResonanceBuffRemainingTime = BuffDuration; // Set duration based on stat

            // Multiplier (ResonanceMultiplier) should already be set by StopPendulum

            UpdateUI(); // Update UI immediately after activation
        }

        // Starts the mini-game sequence
        private void StartMiniGame()
        {
            if (ResonanceChamberBuffActive || _isMiniGameActive) return; // Safety check

            _isMiniGameActive = true;
            // Ensure charge is exactly max for UI consistency if needed, or just leave it >= RequiredCharge
            // ResonanceCharge = RequiredCharge;
            progressBar.gameObject.SetActive(false); // Hide progress bar
            miniGamePanel.SetActive(true); // Show mini-game panel (containing the slider)
            pendulumSlider.value = pendulumSlider.minValue; // Start slider at min
            _pendulumDirection = 1.0f; // Start moving right

            UpdateUI(); // Update button states etc.
        }

        // Stops the pendulum, calculates multiplier, and activates the buff
        private void StopPendulum()
        {
            if (!_isMiniGameActive) return; // Safety check

            _isMiniGameActive = false;
            miniGamePanel.SetActive(false); // Hide mini-game panel
            progressBar.gameObject.SetActive(true); // Show progress bar again (to show buff duration)

            var stoppedValue = pendulumSlider.value; // Get value where it stopped (normalized 0-1)

            // Calculate and store the multiplier
            ResonanceMultiplier = GetMultiplierForValue(stoppedValue);

            // Apply the buff effects (set active flag and timer)
            ActivateBuff();

            // Reset charge *after* activating the buff
            ResonanceCharge = 0;

            Debug.Log($"Resonance Chamber Pendulum Stopped: Multiplier x{ResonanceMultiplier}");

            UpdateUI(); // Update UI to show buff state
        }

        // Determines multiplier based on slider value (0-1)
        private int GetMultiplierForValue(float sliderValue)
        {
            // Red: 0-0.1 and 0.9-1.0 -> 2x
            // Orange: 0.1-0.4 and 0.6-0.9 -> 3x
            // Green: 0.4-0.6 -> 5x
            if (sliderValue >= 0.4f && sliderValue <= 0.6f)
            {
                PerfectResonanceScore = true;
                return 5;
            }

            if ((sliderValue > 0.1f && sliderValue < 0.4f) || (sliderValue > 0.6f && sliderValue < 0.9f))
                return 3; // Orange
            return 2; // Red
        }


        // --- UI Update Logic ---
        public bool Affordable => Chronotons >= cost;
        public string AffordableString => Affordable ? ColourHighlight : ColourRed;

        // Updates all UI elements based on the current state
        public override void UpdateUI()
        {
            var buffDuration = BuffDuration; // Cache property access
            var requiredCharge = RequiredCharge;

            // Check the loaded state of ResonanceChamberUnlocked here
            if (!ResonanceChamberUnlocked)
            {
                chargeText.text = "<b>Locked</b>";
                costAndDescriptionText.text =
                    $"Unlocks the Resonance Chamber mini-game.\n<b>Cost</b> | {AffordableString}{FormatNumber(Chronotons)}{EndColour}/ {AffordableString}{FormatNumber(cost)}{EndColour} {ColourGrey}Chronotons{EndColour}";
                progressBar.fillAmount = 0;
                buyAndActivateButton.interactable = Affordable;
                // Removed the cost from the Buy button text as requested
                buyAndActivateButtonText.text = "Buy";
                progressBar.gameObject.SetActive(true);
                miniGamePanel.SetActive(false);
            }
            else
            {
                // Common description part when unlocked
                // Modify description slightly to reflect mini-game activation
                costAndDescriptionText.text =
                    "Consume <b>1%</b> Chronotons/sec to charge.\nWhen charged, activate the pendulum mini-game for a production rate buff " +
                    $"lasting {ColourGreen}{FormatTime(BuffDuration, true, shortForm: false)}{EndColour}";

                if (ResonanceChamberBuffActive)
                {
                    // Display active buff, multiplier, and remaining time
                    // Use FormatTime if using real time, FormatTimeRemaining if using game time delta, mirroring EnergyAmplifier
                    var timeFormatted = useRealTime
                        ? FormatTime(ResonanceBuffRemainingTime, true)
                        : FormatTimeRemaining(ResonanceBuffRemainingTime, true);
                    var buffColour = Math.Abs(CurrentBuffValue - 2) < 0.1
                        ? ColourRed
                        : Math.Abs(CurrentBuffValue - 3) < 0.1
                            ? ColourOrange
                            : ColourGreen;
                    chargeText.text =
                        $"<b>Active Buff | {buffColour}x{CurrentBuffValue:F0}{EndColour}</b> | {ColourGreen}{timeFormatted}{EndColour}";

                    progressBar.fillAmount = buffDuration > 0 ? (float)(ResonanceBuffRemainingTime / buffDuration) : 0f;
                    progressBar.gameObject.SetActive(true); // Show progress bar for duration
                    miniGamePanel.SetActive(false); // Hide mini-game
                    buyAndActivateButton.interactable = false; // Cannot interact while buff is active
                    buyAndActivateButtonText.text = "Active";
                }
                else if (_isMiniGameActive)
                {
                    // Display mini-game active state
                    chargeText.text = "<b>Pendulum Active!</b>";
                    progressBar.gameObject.SetActive(false); // Hide progress bar
                    miniGamePanel.SetActive(true); // Show mini-game panel
                    buyAndActivateButton.interactable = true; // Enable button to stop the pendulum
                    buyAndActivateButtonText.text = "Stop";
                }
                else // Charging state or charged & ready (before mini-game starts)
                {
                    chargeText.text =
                        $"<b>Charge</b> | {ColourGreen}{FormatNumber(ResonanceCharge)} / {FormatNumber(requiredCharge)}{EndColour}";
                    progressBar.fillAmount = requiredCharge > 0 ? (float)(ResonanceCharge / requiredCharge) : 0f;
                    progressBar.gameObject.SetActive(true); // Show progress bar for charge
                    miniGamePanel.SetActive(false); // Hide mini-game panel

                    // Button is non-interactable while charging. Text indicates status.
                    buyAndActivateButton.interactable = false;
                    buyAndActivateButtonText.text =
                        ResonanceCharge >= requiredCharge
                            ? "Ready..."
                            : "Charging"; // "Ready..." is shown briefly before mini-game starts
                }
            }
        }


        #region IUpgradable Implementation

        // Define tags for upgrade system targeting
        [SerializeField] private List<string> tags = new() { "EoERC" }; // Changed tag slightly
        protected override List<string> FacilityTags => tags;

        // Base values for the stats (using EnergyAmplifier's stats)
        private readonly double _baseCharge = 50; // Base for EoERequiredCharge
        private readonly double _baseDuration = 15; // Base for EoEBaseDuration
        private readonly double _baseBuff = 1; // Base for EoEBuffValue (default/unused here, set by mini-game)


        protected override void SetUpSpecificVariables()
        {
            // Use the exact same StatType enum values as EnergyAmplifier
            UpgradableStats[StatType.EoERequiredCharge] = new UpgradableStat { baseValue = _baseCharge };
            UpgradableStats[StatType.EoEBaseDuration] = new UpgradableStat { baseValue = _baseDuration };
            UpgradableStats[StatType.EoEBuffValue] = new UpgradableStat { baseValue = _baseBuff };
            // The value from EoEBuffValue isn't directly used by CurrentBuffValue in this implementation,
            // but setting it maintains consistency with EnergyAmplifier's setup.
        }

        #endregion
    }
}