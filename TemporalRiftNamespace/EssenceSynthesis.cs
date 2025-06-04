// Ensure this using static statement points to the class containing your static variables

using System;
using System.Collections.Generic;
using System.Linq;
using Blindsided.Utilities;
using MPUIKIT;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;
using static TemporalRiftNamespace.TemporalRiftsStaticReferences;
using static Blindsided.SaveData.TextColourStrings;


namespace TemporalRiftNamespace
{
    public class EssenceSynthesis : SerializedMonoBehaviour, IUpgradable
    {
        public List<EssenceConverter> essenceConverters = new();

        // (UI Elements and Parameters remain the same)

        #region UI Elements & Parameters

        // ─────────────────────────────────────────────────────────────
        //                  RECALIBRATION UI ELEMENTS
        // ─────────────────────────────────────────────────────────────
        [Header("Matrix Recalibration UI")] [SerializeField]
        private TextMeshProUGUI stabilizedMatrixFragmentsText;

        [SerializeField] private TextMeshProUGUI matrixFragmentsToGainText;
        [SerializeField] private TextMeshProUGUI recalibrationTimerText;
        [SerializeField] private MPImage recalibrationProgressBar;
        [SerializeField] private Button recalibrationButton;
        [SerializeField] private TextMeshProUGUI recalibrationButtonText;

        // ─────────────────────────────────────────────────────────────
        //                 RECALIBRATION PARAMETERS
        // ─────────────────────────────────────────────────────────────
        [Header("Matrix Recalibration Parameters")]
        [Tooltip(
            "The divisor for Total Eternum Essence in the SMF calculation. Higher values mean fewer SMF per Essence. Formula part: (Essence / X)^Y")]
        public double recalibrationX = 10000;

        [Tooltip(
            "The exponent for the Essence term in the SMF calculation. Values < 1 give diminishing returns. Formula part: (Essence / X)^Y")]
        public double recalibrationY = 0.5;

        [Tooltip(
            "The divisor for Peak Strain in the SMF calculation. Higher values reduce the impact of Peak Strain. Formula part: (1 + PeakStrain / Z)")]
        public double recalibrationZ = 100;

        [Tooltip("Base time in seconds for recalibration, before adding time per fragment.")]
        public float recalibrationBaseTime = 180f; // 3 minutes

        [Tooltip("Additional time in seconds added to recalibration duration for each SMF to be gained.")]
        public float recalibrationTimePerFragment = 15f;

        [Tooltip("Minimum total time in seconds for any recalibration cycle.")]
        public float recalibrationMinTime = 180f; // min 3 mins

        [Tooltip("Maximum total time in seconds for any recalibration cycle.")]
        public float recalibrationMaxTime = 600f; // max 10 mins

        #endregion

        // ─────────────────────────────────────────────────────────────
        //            RECALIBRATION STATE (Internal / Static)
        // ─────────────────────────────────────────────────────────────
        private float currentRecalibrationTimeTotal_Instance;
        // Static members accessed via 'using static': Recalibrating, RecalibrationTime, etc.

        // ─────────────────────────────────────────────────────────────
        //                       UNITY LIFECYCLE
        // ─────────────────────────────────────────────────────────────
        private void Start()
        {
            SetupSaveData();
            SetupButtons();
            SyncRecalibrationStateOnLoad();
            // Initial full UI update called from manager should handle this,
            // but a call here ensures state on first frame.
            UpdateAllVisuals();
        }

        private void SyncRecalibrationStateOnLoad()
        {
            if (Recalibrating) // Static access
            {
                currentRecalibrationTimeTotal_Instance = CurrentRecalibrationTimeTotal; // Static access
                Debug.Log($"EssenceSynthesis Start: Synced local total time: {currentRecalibrationTimeTotal_Instance}");
            }
            else
            {
                currentRecalibrationTimeTotal_Instance = 0f;
            }
        }

        // ─────────────────────────────────────────────────────────────
        //                      TIMER LOGIC UPDATE
        // ─────────────────────────────────────────────────────────────
        /// <summary>
        ///     Updates the internal timer values for active processes based on deltaTime.
        ///     Handles completion logic when timers expire.
        ///     Does NOT update UI visuals directly.
        /// </summary>
        public void UpdateTimers(float deltaTime)
        {
            var triggerFullVisualUpdate = false; // Flag if a process finishes

            // --- Update Converter Timers ---
            foreach (var converter in essenceConverters)
            {
                if (converter.EssenceSaveData == null || !converter.EssenceSaveData.IsConverting) continue;

                var duration = (float)converter.GetStat(StatType.EssenceConverterDuration).CachedValue;
                if (duration <= 0f) duration = 1f;
                converter.EssenceSaveData.Timer += deltaTime;

                if (converter.EssenceSaveData.Timer >= duration)
                {
                    converter.EssenceSaveData.IsConverting = false;
                    converter.EssenceSaveData.Timer = 0f;
                    EternumEssence += converter.EssenceSaveData.AmountGaining; // Static access
                    triggerFullVisualUpdate = true;
                }
            }

            // --- Update Recalibration Timer ---
            if (Recalibrating) // Static access
            {
                RecalibrationTime -= deltaTime; // Static access
                if (RecalibrationTime <= 0f) // Static access
                    // Completion logic is handled within CompleteRecalibration
                    CompleteRecalibration();
                // CompleteRecalibration calls UpdateAllVisuals internally, so no need to set flag here
            }

            // If a converter finished, trigger a visual update immediately
            // This provides instant feedback rather than waiting for the next manager update cycle.
            if (triggerFullVisualUpdate) UpdateAllVisuals();
        }

        // ─────────────────────────────────────────────────────────────
        //                 COMBINED UI VISUALS UPDATE (NEW METHOD)
        // ─────────────────────────────────────────────────────────────
        /// <summary>
        ///     Updates ALL visual elements of the Essence Synthesis panel, including
        ///     general states (idle/N/A, previews, resource amounts) and active timer displays.
        ///     Call this regularly (e.g., every frame) from an external manager AFTER UpdateTimers.
        ///     Also called internally when processes start/stop for immediate feedback.
        /// </summary>
        public void UpdateAllVisuals()
        {
            // Determine global states first
            var isRecalibrationGloballyActive = Recalibrating; // Static access
            var isAnySynthesisGloballyActive = AnyRunningConverters();

            // --- Update All Converter Visuals ---
            foreach (var c in essenceConverters)
            {
                if (c == null || c.EssenceSaveData == null) continue;
                UpdateSingleConverterVisuals(c, isRecalibrationGloballyActive, isAnySynthesisGloballyActive);
            }

            // --- Update Recalibration Visuals ---
            UpdateRecalibrationVisuals(isRecalibrationGloballyActive, isAnySynthesisGloballyActive);

            // --- Update Button Interactability ---
            // Note: Called within UpdateRecalibrationVisuals as well, ensures it runs.
            SetButtonsInteractable(isRecalibrationGloballyActive, isAnySynthesisGloballyActive);
        }

        // Helper method to update a single converter's visuals based on global state
        private void UpdateSingleConverterVisuals(EssenceConverter c, bool isRecalibrationGloballyActive,
            bool isAnySynthesisGloballyActive)
        {
            // Update common elements like name and cost text
            if (c.conversionText != null)
                c.conversionText.text =
                    $"<b>Conversion Cost</b> | {ColourOrange}{c.EssenceSaveData.CurrentCostPercent / 100:P1}{EndColour}";
            var currentResourceAmount = GetCurrentResourceAmount(c.essenceType);
            if (c.nameText != null)
                c.nameText.text =
                    $"{c.essenceType.ToString()} | {ColourGreen}{CalcUtils.FormatNumber(currentResourceAmount)}{EndColour}";

            // Handle state-dependent UI
            if (c.EssenceSaveData.IsConverting) // This specific converter is ACTIVE
            {
                // Display "Gaining..." text
                if (c.gainText != null)
                    c.gainText.text =
                        $"<b>Gaining</b> | {ColourGreen}{CalcUtils.FormatNumber(c.EssenceSaveData.AmountGaining)} {ColourGrey}Eternum Essence{EndColour}";

                // Update active timer visuals (progress bar and countdown)
                var duration = (float)c.GetStat(StatType.EssenceConverterDuration).CachedValue;
                if (duration <= 0f) duration = 1f;
                if (c.conversionProgressBar != null)
                    c.conversionProgressBar.fillAmount = Mathf.Clamp01(c.EssenceSaveData.Timer / duration);
                if (c.timerText != null)
                {
                    var remainingTime = Mathf.Max(0f, duration - c.EssenceSaveData.Timer);
                    c.timerText.text = $"{CalcUtils.FormatTimeRemaining(remainingTime, true)}";
                }
            }
            else // This specific converter is IDLE
            {
                if (isRecalibrationGloballyActive || isAnySynthesisGloballyActive) // Show N/A
                {
                    if (c.timerText != null) c.timerText.text = $"{ColourRed}N/A{EndColour}";
                    if (c.gainText != null) c.gainText.text = $"<b>Gain</b> | {ColourRed}N/A{EndColour}";
                    if (c.conversionProgressBar != null) c.conversionProgressBar.fillAmount = 0f;
                }
                else // Show normal Idle state (preview gain, default duration text)
                {
                    var pool = GetCurrentResourceAmount(c.essenceType);
                    var preview = 0d;
                    if (pool >= 1d && c.EssenceSaveData.CurrentCostPercent < 100f)
                        preview = Math.Log10(pool) * (100f - c.EssenceSaveData.CurrentCostPercent) / 100f *
                                  c.GetStat(StatType.EssenceConverterGainMultiplier).CachedValue;
                    if (c.gainText != null)
                        c.gainText.text =
                            $"<b>Gain</b> | {ColourGreen}{CalcUtils.FormatNumber(preview)} {ColourGrey}Eternum Essence{EndColour}";

                    if (c.conversionProgressBar != null) c.conversionProgressBar.fillAmount = 0f;
                    var duration = (float)c.GetStat(StatType.EssenceConverterDuration).CachedValue;
                    if (duration <= 0f) duration = 1f;
                    if (c.timerText != null)
                        c.timerText.text =
                            $"{CalcUtils.FormatTimeRemaining(duration, true)}"; // Show potential duration
                }
            }
        }

        // Helper method to update recalibration visuals based on global state
        private void UpdateRecalibrationVisuals(bool isRecalibrationCurrentlyActive, bool isAnySynthesisCurrentlyActive)
        {
            // Update stabilized fragments display (always relevant)
            if (stabilizedMatrixFragmentsText != null)
                stabilizedMatrixFragmentsText.text =
                    $"<b>Stabilized Matrix Fragments</b> | {ColourGreen}{CalcUtils.FormatNumber(StabilizedMatrixFragments)}{EndColour}"; // Static access

            if (isRecalibrationCurrentlyActive) // Recalibration IS active
            {
                // Display "Stabilizing..." text
                if (matrixFragmentsToGainText != null)
                    matrixFragmentsToGainText.text =
                        $"<b>Stabilizing</b> | {ColourGreen}{CalcUtils.FormatNumber(MatrixFragmentsStabilizing)} {ColourGrey}Matrix Fragments{EndColour}"; // Static access
                if (recalibrationButtonText != null) recalibrationButtonText.text = "Stabilizing...";

                // Update active timer visuals (progress bar and countdown)
                UpdateRecalibrationTimerVisuals(); // Keep this specialized method for active recalibration
            }
            else // Recalibration is NOT active
            {
                if (isAnySynthesisCurrentlyActive) // Show N/A because synthesis is active
                {
                    if (recalibrationTimerText != null) recalibrationTimerText.text = $"{ColourRed}N/A{EndColour}";
                    if (matrixFragmentsToGainText != null)
                        matrixFragmentsToGainText.text = $"<b>Gain</b> | {ColourRed}N/A{EndColour}";
                    if (recalibrationButtonText != null) recalibrationButtonText.text = "Stabilize";
                    if (recalibrationProgressBar != null) recalibrationProgressBar.fillAmount = 0f;
                }
                else // Show normal Idle state (preview gain, potential duration text)
                {
                    var smfPreview = CalculateSMFToGain();
                    if (matrixFragmentsToGainText != null)
                        matrixFragmentsToGainText.text =
                            $"<b>Gain</b> | {ColourGreen}{CalcUtils.FormatNumber(smfPreview)} {ColourGrey}Matrix Fragments{EndColour}";
                    if (recalibrationButtonText != null) recalibrationButtonText.text = "Stabilize";
                    if (recalibrationProgressBar != null) recalibrationProgressBar.fillAmount = 0f;
                    // Show potential duration when idle
                    if (recalibrationTimerText != null)
                    {
                        var potentialTotalTime =
                            recalibrationBaseTime + (float)smfPreview * recalibrationTimePerFragment;
                        potentialTotalTime =
                            Mathf.Clamp(potentialTotalTime, recalibrationMinTime, recalibrationMaxTime);
                        if (smfPreview > 0)
                            recalibrationTimerText.text = $"{CalcUtils.FormatTimeRemaining(potentialTotalTime, true)}";
                        else
                            recalibrationTimerText.text =
                                $"{CalcUtils.FormatTimeRemaining(recalibrationMinTime, true)}";
                    }
                }
            }
            // SetButtonsInteractable is called by the main UpdateAllVisuals after this.
        }

        // Specialized method only for active recalibration visuals (called by UpdateRecalibrationVisuals)
        private void UpdateRecalibrationTimerVisuals()
        {
            if (recalibrationTimerText != null)
            {
                var remainingTime = Mathf.Max(0f, RecalibrationTime); // Static access
                recalibrationTimerText.text = $"{CalcUtils.FormatTimeRemaining(remainingTime, true)}";
            }

            if (recalibrationProgressBar != null)
            {
                var totalTimeToUse = CurrentRecalibrationTimeTotal; // Static access
                if (totalTimeToUse <= 0 && Recalibrating && RecalibrationTime > 0)
                {
                    // Static access
                    Debug.LogWarning(
                        $"UpdateRecalibrationTimerVisuals: Static CurrentRecalibrationTotalTime ({totalTimeToUse}) invalid. Using instance fallback.");
                    totalTimeToUse = currentRecalibrationTimeTotal_Instance; // Fallback
                }

                if (totalTimeToUse > 0)
                    recalibrationProgressBar.fillAmount =
                        Mathf.Clamp01(1f - RecalibrationTime / totalTimeToUse); // Static access
                else
                    recalibrationProgressBar.fillAmount = RecalibrationTime <= 0f ? 1f : 0f;
            }
        }


        // (Button Setup, StartSynthesis, Recalibration Logic, SaveData, Helpers, Debug, UpgradeSystem remain largely the same,
        // but Start/Complete methods should now call UpdateAllVisuals() for immediate feedback)

        #region Process Start/Stop and Button Logic

        // ─────────────────────────────────────────────────────────────
        //                         BUTTON SETUP
        // ─────────────────────────────────────────────────────────────
        private void SetupButtons()
        {
            foreach (var converter in essenceConverters)
            {
                if (converter.convertButton == null) continue;
                converter.convertButton.onClick.RemoveAllListeners();
                var typeToConvert = converter.essenceType;
                converter.convertButton.onClick.AddListener(() => StartSynthesis(typeToConvert));
            }

            if (recalibrationButton != null)
            {
                recalibrationButton.onClick.RemoveAllListeners();
                recalibrationButton.onClick.AddListener(StartRecalibration);
            }
        }

        // ─────────────────────────────────────────────────────────────
        //                 ENTRY POINT FROM CONVERT BUTTON
        // ─────────────────────────────────────────────────────────────
        private void StartSynthesis(EssenceType essenceType)
        {
            if (AnyRunningConverters() || Recalibrating) return;
            var converter = essenceConverters.FirstOrDefault(c => c.essenceType == essenceType);
            if (converter == null || converter.EssenceSaveData == null) /* Error */ return;
            var costPercent = converter.EssenceSaveData.CurrentCostPercent;
            if (costPercent >= 100f) return;
            var totalPool = GetCurrentResourceAmount(essenceType);
            if (totalPool < 1d) return;

            var amountToConsume = totalPool * (costPercent / 100f);
            SubtractResource(essenceType, amountToConsume);

            var logPool = Math.Log10(totalPool);
            var eeGain = logPool * (100f - costPercent) / 100f *
                         converter.GetStat(StatType.EssenceConverterGainMultiplier).CachedValue;

            converter.EssenceSaveData.AmountGaining = eeGain;
            converter.EssenceSaveData.Timer = 0f;
            converter.EssenceSaveData.IsConverting = true;

            converter.EssenceSaveData.CurrentCostPercent = Mathf.Min(100f,
                costPercent + (float)GetStat(StatType.EssenceSynthesisCostIncrease).CachedValue);
            foreach (var other in essenceConverters)
                if (other.EssenceSaveData != null && other.essenceType != essenceType)
                    other.EssenceSaveData.CurrentCostPercent = Mathf.Max(10f,
                        other.EssenceSaveData.CurrentCostPercent -
                        (float)GetStat(StatType.EssenceSynthesisCostDecrease).CachedValue);

            // Update UI immediately for feedback
            UpdateAllVisuals();
        }

        // ─────────────────────────────────────────────────────────────
        //                    RECALIBRATION LOGIC
        // ─────────────────────────────────────────────────────────────
        public double CalculateSMFToGain()
        {
            /* calculation... */
            if (EternumEssence <= 0) return 0; 
            double peakStrain = essenceConverters.Where(converter => converter.EssenceSaveData != null)
                .Sum(converter => converter.EssenceSaveData.CurrentCostPercent);
            var essenceTerm = EternumEssence / recalibrationX;
            if (essenceTerm <= 0) return 0;
            var essenceFactor = Math.Pow(essenceTerm, recalibrationY);
            var strainFactor = 1 + peakStrain / recalibrationZ;
            var smfGain = Math.Floor(essenceFactor * strainFactor);
            return Math.Max(0, smfGain);
        }

        public void StartRecalibration()
        {
            if (AnyRunningConverters() || Recalibrating) return;
            MatrixFragmentsStabilizing = CalculateSMFToGain(); // Static access
            if (MatrixFragmentsStabilizing <= 0) return;

            Recalibrating = true; // Static access
            var totalRecalTime =
                recalibrationBaseTime + (float)MatrixFragmentsStabilizing * recalibrationTimePerFragment;
            var clampedTotalTime = Mathf.Clamp(totalRecalTime, recalibrationMinTime, recalibrationMaxTime);
            currentRecalibrationTimeTotal_Instance = clampedTotalTime;
            CurrentRecalibrationTimeTotal = clampedTotalTime; // Static access
            RecalibrationTime = clampedTotalTime; // Static access
            Debug.Log($"Starting Recalibration. Gaining: {MatrixFragmentsStabilizing} SMF. Time: {clampedTotalTime}s.");

            // Update UI immediately for feedback
            UpdateAllVisuals();
        }

        private void CompleteRecalibration()
        {
            Debug.Log($"Recalibration Complete! Gained {MatrixFragmentsStabilizing} SMF.");
            StabilizedMatrixFragments += MatrixFragmentsStabilizing; // Static access
            EternumEssence = 0; // Static access
            Recalibrating = false; // Static access
            RecalibrationTime = 0; // Static access
            MatrixFragmentsStabilizing = 0; // Static access
            currentRecalibrationTimeTotal_Instance = 0f;
            CurrentRecalibrationTimeTotal = 0f; // Static access
            Debug.Log("Recalibration Complete state reset.");

            foreach (var converter in essenceConverters)
            {
                // Reset costs/state
                if (converter.EssenceSaveData != null)
                {
                    converter.EssenceSaveData.CurrentCostPercent = 10f;
                    converter.EssenceSaveData.IsConverting = false;
                    converter.EssenceSaveData.Timer = 0f;
                    converter.EssenceSaveData.AmountGaining = 0;
                }

                if (SavedEssenceConverters.TryGetValue(converter.essenceType, out var saveData))
                {
                    // Static access
                    saveData.CurrentCostPercent = 10f;
                    saveData.IsConverting = false;
                    saveData.Timer = 0f;
                    saveData.AmountGaining = 0;
                }
            }

            // Update UI immediately after completion
            UpdateAllVisuals();
        }

        // ─────────────────────────────────────────────────────────────
        //               BUTTON ENABLE / DISABLE HANDLING
        // ─────────────────────────────────────────────────────────────
        // Pass state flags to avoid redundant checks within the method
        private void SetButtonsInteractable(bool isRecalibrating, bool isAnyConverterRunning)
        {
            var canRecalibrate = CalculateSMFToGain() > 0; // Need to calculate this regardless

            foreach (var c in essenceConverters)
            {
                if (c == null || c.EssenceSaveData == null || c.convertButton == null) continue;
                var atMaxCost = c.EssenceSaveData.CurrentCostPercent >= 100f;
                var hasResources = GetCurrentResourceAmount(c.essenceType) >= 1d;
                c.convertButton.interactable = !isAnyConverterRunning && !isRecalibrating && !atMaxCost && hasResources;
            }

            if (recalibrationButton != null)
                recalibrationButton.interactable = !isAnyConverterRunning && !isRecalibrating && canRecalibrate;
        }

        private bool AnyRunningConverters()
        {
            return essenceConverters.Any(c => c?.EssenceSaveData != null && c.EssenceSaveData.IsConverting);
        }

        #endregion

        #region SaveData, Helpers, Debug, Upgrades

        // ─────────────────────────────────────────────────────────────
        //                     SAVE‑DATA INITIALISATION
        // ─────────────────────────────────────────────────────────────
        private void SetupSaveData()
        {
            foreach (var converter in essenceConverters)
            {
                if (!SavedEssenceConverters.ContainsKey(converter.essenceType)) // Static access
                    SavedEssenceConverters.Add(converter.essenceType,
                        new EssenceSaveData { CurrentCostPercent = 10f }); // Static access
                converter.EssenceSaveData = SavedEssenceConverters[converter.essenceType]; // Static access
            }
        }

        // ─────────────────────────────────────────────────────────────
        //     RESOURCE HELPERS (PROJECT‑SPECIFIC, ASSUMED STATIC)
        // ─────────────────────────────────────────────────────────────
        private double GetCurrentResourceAmount(EssenceType type)
        {
            /* As before */
            return type switch
            {
                EssenceType.StellarParticles => StellarParticles, EssenceType.EntropyFragments => EntropyFragments,
                EssenceType.Chronotons => Chronotons, EssenceType.TemporalWorkers => TemporalWorkers,
                EssenceType.Chronicles => Chronicles, _ => 0d
            };
        }

        private void SubtractResource(EssenceType type, double amt)
        {
            /* As before */
            switch (type)
            {
                case EssenceType.StellarParticles: StellarParticles -= amt; break;
                case EssenceType.EntropyFragments: EntropyFragments -= amt; break;
                case EssenceType.Chronotons: Chronotons -= amt; break;
                case EssenceType.TemporalWorkers: TemporalWorkers -= amt; break;
                case EssenceType.Chronicles: Chronicles -= amt; break;
                default: Debug.LogError($"Unhandled resource type {type}"); break;
            }
        }

        // ─────────────────────────────────────────────────────────────
        //                        DEBUG SHORTCUT
        // ─────────────────────────────────────────────────────────────
        [Button]
        public void ResetConvertersAndRecalibration()
        {
            // Stop processes
            Recalibrating = false;
            RecalibrationTime = 0f;
            MatrixFragmentsStabilizing = 0f;
            currentRecalibrationTimeTotal_Instance = 0f;
            CurrentRecalibrationTimeTotal = 0f;
            foreach (var converter in essenceConverters)
            {
                if (converter.EssenceSaveData != null)
                {
                    converter.EssenceSaveData.IsConverting = false;
                    converter.EssenceSaveData.Timer = 0f;
                }

                if (SavedEssenceConverters.TryGetValue(converter.essenceType, out var saveData))
                {
                    saveData.IsConverting = false;
                    saveData.Timer = 0f;
                }
            }

            // Reset values
            EternumEssence = 0;
            foreach (var converter in essenceConverters)
            {
                if (converter.EssenceSaveData != null)
                {
                    converter.EssenceSaveData.AmountGaining = 0;
                    converter.EssenceSaveData.CurrentCostPercent = 10f;
                }

                if (SavedEssenceConverters.TryGetValue(converter.essenceType, out var saveData))
                {
                    saveData.AmountGaining = 0;
                    saveData.CurrentCostPercent = 10f;
                }
            }

            // Update UI
            UpdateAllVisuals();
            Debug.Log("Essence Converters & Recalibration State Reset");
        }

        // ─────────────────────────────────────────────────────────────
        //                        UPGRADE SYSTEM
        // ─────────────────────────────────────────────────────────────
        [TabGroup("UpgradeData")] public List<string> tags;
        [TabGroup("UpgradeData")] public Dictionary<StatType, UpgradableStat> UpgradableStats = new();

        public List<string> GetTags()
        {
            return tags;
        }

        public UpgradableStat GetStat(StatType statType)
        {
            return UpgradableStats.GetValueOrDefault(statType);
        }

        public void Register()
        {
            UpgradeManager.Instance?.RegisterEntity(this);
        }

        public void Unregister()
        {
            UpgradeManager.Instance?.UnregisterEntity(this);
        }

        private void OnEnable()
        {
            Register();
        }

        private void OnDisable()
        {
            Unregister();
        }

        #endregion
    }

    // Assuming necessary supporting classes/enums like EssenceConverter, EssenceSaveData, EssenceType, StatType exist.
}