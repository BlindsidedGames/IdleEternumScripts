using System;
using System.Collections.Generic;
using System.Linq;
using Blindsided.CardSelectionSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnlockNexus;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;
using static RealmOfResearchNamespace.RealmOfResearchStaticReferences;
using static Blindsided.SaveData.StaticReferences;
using static EnginesOfExpansionNamespace.EnginesOfExpansionStaticReferences;
using static CollapseOfTimeNamespace.CollapseOfTimeStaticReferences;
using static ChronicleArchivesNamespace.ChronicleArchiveStaticReferences;

namespace Game_Services
{
    public sealed class GameServicesManager : SerializedMonoBehaviour
    {
        public CardSelector cardSelector;
        [TabGroup("Achievements")] public Dictionary<AchievementKeys, AchievementData> Achievements = new();

        // track what we already reported so we don't spam the network
        private readonly Dictionary<AchievementKeys, double> _lastPercentSent = new();

        #region Unity lifecycle

        private void OnEnable()
        {
            GameServices.OnAuthStatusChange += HandleAuthStatus; // subscribe once
        }

        private void OnDisable()
        {
            GameServices.OnAuthStatusChange -= HandleAuthStatus;
        }

        private void Start()
        {
            if (!GameServices.IsAvailable()) // runtime platform check 
            {
                Debug.LogWarning("Game Services not available on this platform.");
                return;
            }

            GameServices.Authenticate(); // async, result arrives via event 
        }

        #endregion

        #region Authentication handlers

        private void HandleAuthStatus(GameServicesAuthStatusChangeResult result, Error error)
        {
            if (error == null && result.AuthStatus == LocalPlayerAuthStatus.Authenticated)
            {
                Debug.Log("Signed into Game Services.");
                // start polling achievements no sooner than 2 s after sign-in, every 30 s
                InvokeRepeating(nameof(CheckAchievements), 2f, 30f);
            }
            else
            {
                Debug.LogWarning($"Game Services auth failed or cancelled: {error}");
            }
        }

        #endregion

        #region Achievement processing

        private void CheckAchievements()
        {
            foreach (var kvp in Achievements)
            {
                var (isComplete, percent) = ProcessUnlock(kvp.Key);

                // cap between 0 and 100 just in case
                percent = Mathf.Clamp((float)percent, 0f, 100f);

                // send only if the player is authenticated and the percentage actually changed
                if (!GameServices.IsAuthenticated) continue;

                if (!_lastPercentSent.TryGetValue(kvp.Key, out var last) ||
                    Math.Abs(percent - last) >= 0.1f)
                {
                    SubmitAchievement(kvp.Value.ID, percent);
                    _lastPercentSent[kvp.Key] = percent;
                }
            }
        }

        private void SubmitAchievement(string achievementId, double progress)
        {
            GameServices.ReportAchievementProgress(
                achievementId,
                progress,
                (success, error) =>
                {
                    if (!success)
                        Debug.LogWarning($"Failed to report {achievementId}: {error}");
                }); // usage pattern 
        }

        public void ShowAchievementsUI()
        {
            if (!GameServices.IsAuthenticated)
            {
                Debug.LogWarning("User not authenticated. Cannot show achievements UI.");
                return;
            }

            GameServices.ShowAchievements(); // per docs 
        }

        #endregion

        #region Editor helpers

#if UNITY_EDITOR
        [Button(ButtonSizes.Small)]
        private void SetupAchievements()
        {
            // rebuild dictionary from existing keys (editor only)
            Achievements = Enum.GetValues(typeof(AchievementKeys))
                .Cast<AchievementKeys>()
                .ToDictionary(k => k, _ => new AchievementData());
        }
#endif

        #endregion

        #region Progress evaluation  (unchanged trimmed for brevity)

        private (bool, double) ProcessUnlock(AchievementKeys id)
        {
            switch (id)
            {
                case AchievementKeys.FirstSteps:
                    var progress = FopBuildings.NebulaGenerator.NumberPurchased >= 1;
                    return (progress, progress ? 100 : 0);
                case AchievementKeys.CosmicArchitect:
                    var progress2 = FopBuildings.NovaSpire.NumberPurchased >= 1;
                    return (progress2, progress2 ? 100 : 0);
                case AchievementKeys.MasterResearcher:
                    var progress3 = RorBuildings.DarkArchitect.NumberPurchased >= 100;
                    return (progress3, progress3 ? 100 : 0);
                case AchievementKeys.TemporalEngineer:
                    return (ResonanceChamberUnlocked, ResonanceChamberUnlocked ? 100 : 0);
                case AchievementKeys.EvolutionaryLeap:
                    return (EntropyTierSaveData > 4, (double)(EntropyTierSaveData / 5f) * 100);
                case AchievementKeys.ResurgenceInitiate:
                    return (Prestiged, Prestiged ? 100 : 0);
                case AchievementKeys.ContinuumAdept:
                    return (TimesAscended >= 10, TimesAscended / 10f * 100);
                case AchievementKeys.CycleMastery:
                    return (TotalResets >= 50, TotalResets / 50f * 100);
                case AchievementKeys.SwiftCompletion:
                    var progress4 = IdsFastestCompletionTime < 12000;
                    return (progress4, progress4 ? 100 : 0);
                case AchievementKeys.Amplification:
                    return (EnergyAmplifierBuffActive, EnergyAmplifierBuffActive ? 100 : 0);
                case AchievementKeys.CardCollector:
                    var cardsLevelOneOrHigher = 0;
                    foreach (var collection in cardSelector.cardCollections)
                    foreach (var card in collection.Cards)
                        if (card.cardReferences.upgrade.GetCurrentLevel() >= 1)
                            cardsLevelOneOrHigher++;
                    return (cardsLevelOneOrHigher >= 34, cardsLevelOneOrHigher / 34f * 100);
                case AchievementKeys.EmpoweredStrategist:
                    var progress7 = IdsCompletionCount > 0;
                    return (progress7, progress7 ? 100 : 0);
                case AchievementKeys.PerfectPendulum:
                    return (PerfectResonanceScore, PerfectResonanceScore ? 100 : 0);
                case AchievementKeys.EternalVoyager:
                    var progress5 = TimeSpentInRealms.Total > 360000;
                    return (progress5, TimeSpentInRealms.Total / 360000f * 100);
                case AchievementKeys.ChronotonHoarder:
                    return (Chronotons > 1000000, Chronotons / 1000000 * 100);
                case AchievementKeys.InfinityRealized:
                    var progress6 = ScaledTimeSpentInRealms.TemporalRifts >= 144000;
                    return (progress6, ScaledTimeSpentInRealms.TemporalRifts / 144000f * 100);
                default:
                    Debug.Log($"Achievement has no Condition: {id}");
                    return (false, 0);
            }
        }

        #endregion
    }

    public enum AchievementKeys
    {
        FirstSteps,
        CosmicArchitect,
        MasterResearcher,
        TemporalEngineer,
        EvolutionaryLeap,
        ResurgenceInitiate,
        ContinuumAdept,
        CycleMastery,
        SwiftCompletion,
        Amplification,
        CardCollector,
        EmpoweredStrategist,
        PerfectPendulum,
        EternalVoyager,
        ChronotonHoarder,
        InfinityRealized
    }
}