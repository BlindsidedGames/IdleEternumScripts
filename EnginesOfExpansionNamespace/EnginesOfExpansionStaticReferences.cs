using System;
using Blindsided.SaveData;
using static Blindsided.Oracle;

namespace EnginesOfExpansionNamespace
{
    public static class EnginesOfExpansionStaticReferences
    {
        public static EnginesOfExpansionSaveData EnginesOfExpansionSaveDataData =>
            oracle.saveData.EnginesOfExpansionSaveDataData;

        public static EoeResetOnPrestige EoeResetOnPrestigeData
        {
            get => EnginesOfExpansionSaveDataData.ResetOnPrestige;
            set => EnginesOfExpansionSaveDataData.ResetOnPrestige = value;
        }

        public static bool EnergyAmplifierAutoActivate
        {
            get => EnginesOfExpansionSaveDataData.EnergyAmplifierAutoActivate;
            set => EnginesOfExpansionSaveDataData.EnergyAmplifierAutoActivate = value;
        }

        public static AutoButtonCycleState AutoButtonCycleState
        {
            get => EnginesOfExpansionSaveDataData.AutoButtonCycleState;
            set => EnginesOfExpansionSaveDataData.AutoButtonCycleState = value;
        }

        public static bool TemporalAcceleratorTarget
        {
            get => EnginesOfExpansionSaveDataData.TemporalAcceleratorTarget;
            set => EnginesOfExpansionSaveDataData.TemporalAcceleratorTarget = value;
        }

        public static bool EnergyAmplifierUnlocked
        {
            get => EoeResetOnPrestigeData.EnergyAmplifierUnlocked;
            set => EoeResetOnPrestigeData.EnergyAmplifierUnlocked = value;
        }

        public static bool EnergyAmplifierBuffActive
        {
            get => EoeResetOnPrestigeData.EnergyAmplifierBuffActive;
            set => EoeResetOnPrestigeData.EnergyAmplifierBuffActive = value;
        }

        public static double EnergyAmplifierBuffRemainingTime
        {
            get => EoeResetOnPrestigeData.EnergyAmplifierBuffRemainingTime;
            set => EoeResetOnPrestigeData.EnergyAmplifierBuffRemainingTime = value;
        }

        public static double EnergyAmplifierCharge
        {
            get => EoeResetOnPrestigeData.EnergyAmplifierCharge;
            set => EoeResetOnPrestigeData.EnergyAmplifierCharge = value;
        }

        public static bool TemporalAcceleratorUnlocked
        {
            get => EoeResetOnPrestigeData.TemnporalAcceleratorUnlocked;
            set => EoeResetOnPrestigeData.TemnporalAcceleratorUnlocked = value;
        }

        public static double TemporalAcceleratorCharge
        {
            get => EoeResetOnPrestigeData.TemporalAcceleratorCharge;
            set => EoeResetOnPrestigeData.TemporalAcceleratorCharge = value;
        }

        public static bool ResonanceChamberUnlocked
        {
            get => EoeResetOnPrestigeData.ResonanceChamberUnlocked;
            set => EoeResetOnPrestigeData.ResonanceChamberUnlocked = value;
        }

        public static bool ResonanceChamberBuffActive
        {
            get => EoeResetOnPrestigeData.ResonanceChamberBuffActive;
            set => EoeResetOnPrestigeData.ResonanceChamberBuffActive = value;
        }

        public static double ResonanceBuffRemainingTime
        {
            get => EoeResetOnPrestigeData.ResonanceBuffRemainingTime;
            set => EoeResetOnPrestigeData.ResonanceBuffRemainingTime = value;
        }

        public static double ResonanceCharge
        {
            get => EoeResetOnPrestigeData.ResonanceCharge;
            set => EoeResetOnPrestigeData.ResonanceCharge = value;
        }

        public static double ResonanceMultiplier
        {
            get => EoeResetOnPrestigeData.ResonanceMultiplier;
            set => EoeResetOnPrestigeData.ResonanceMultiplier = value;
        }

        public static double TimeCoreLevel
        {
            get => EoeResetOnPrestigeData.TimeCoreLevel;
            set => EoeResetOnPrestigeData.TimeCoreLevel = value;
        }

        public static double TimeCoreProgress
        {
            get => EoeResetOnPrestigeData.TimeCoreProgress;
            set => EoeResetOnPrestigeData.TimeCoreProgress = value;
        }

        public static double Chronotons
        {
            get => EoeResetOnPrestigeData.Chronotons;
            set => EoeResetOnPrestigeData.Chronotons = Math.Max(value, 0);
        }

        public static double ChronotonDrillLevel
        {
            get => EoeResetOnPrestigeData.ChronotonDrillLevel;
            set => EoeResetOnPrestigeData.ChronotonDrillLevel = value;
        }

        public static double ChronotonDrillProgress
        {
            get => EoeResetOnPrestigeData.ChronotonDrillProgress;
            set => EoeResetOnPrestigeData.ChronotonDrillProgress = value;
        }

        public static bool PerfectResonanceScore
        {
            get => EnginesOfExpansionSaveDataData.PerfectResonanceScore;
            set => EnginesOfExpansionSaveDataData.PerfectResonanceScore = value;
        }
    }
}