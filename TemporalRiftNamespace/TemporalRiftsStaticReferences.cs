using System.Collections.Generic;
using Blindsided.SaveData;
using static Blindsided.Oracle;
using Math = System.Math;

namespace TemporalRiftNamespace
{
    public class TemporalRiftsStaticReferences
    {
        public static TemporalRiftSaveData TemporalRiftSaveData => oracle.saveData.TemporalRiftSaveData;

        public static Dictionary<EssenceType, EssenceSaveData> SavedEssenceConverters =>
            oracle.saveData.TemporalRiftSaveData.SavedEssenceConverters;

        public static Dictionary<string, TemporalRiftUpgradeSaveData> TemporalRiftUpgrades =>
            oracle.saveData.TemporalRiftSaveData.TemporalRiftUpgrades;

        public static double StellarParticles
        {
            get => oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.Prestige.ResurgenceResets.Currencies
                .StellarParticles;
            set =>
                oracle.saveData.FoundationOfProgressSaveDataData.Ascendancy.Prestige.ResurgenceResets.Currencies
                    .StellarParticles = Math.Max(value, 0.0); // Ensures value is not less than 0
        }

        public static double EntropyFragments
        {
            get => oracle.saveData.RealmOfResearchSaveDataData.Currencies.EntropyFragments;
            set => oracle.saveData.RealmOfResearchSaveDataData.Currencies.EntropyFragments =
                Math.Max(value, 0.0); // Ensures value is not less than 0
        }

        public static double Chronotons
        {
            get => oracle.saveData.EnginesOfExpansionSaveDataData.ResetOnPrestige.Chronotons;
            set => oracle.saveData.EnginesOfExpansionSaveDataData.ResetOnPrestige.Chronotons =
                Math.Max(value, 0.0); // Ensures value is not less than 0
        }

        public static double TemporalWorkers
        {
            get => oracle.saveData.CollapseOfTimeSaveDataData.TemporalWorkers;
            set => oracle.saveData.CollapseOfTimeSaveDataData.TemporalWorkers =
                Math.Max(value, 0.0); // Ensures value is not less than 0
        }

        public static double Chronicles
        {
            get => oracle.saveData.ChronicleArchivesSaveDataData.Chronicles;
            set => oracle.saveData.ChronicleArchivesSaveDataData.Chronicles = Math.Max((long)value, 0);
        }

        public static double EternumEssence
        {
            get => oracle.saveData.TemporalRiftSaveData.EternumEssence;
            set => oracle.saveData.TemporalRiftSaveData.EternumEssence =
                Math.Max(value, 0.0); // Ensures value is not less than 0
        }

        public static float RecalibrationTime
        {
            get => oracle.saveData.TemporalRiftSaveData.RecalibrationTime;
            set => oracle.saveData.TemporalRiftSaveData.RecalibrationTime = Math.Max(value, 0.0f);
        }

        public static float CurrentRecalibrationTimeTotal
        {
            get => oracle.saveData.TemporalRiftSaveData.CurrentRecalibrationTimeTotal;
            set => oracle.saveData.TemporalRiftSaveData.CurrentRecalibrationTimeTotal = Math.Max(value, 0.0f);
        }

        public static bool Recalibrating
        {
            get => oracle.saveData.TemporalRiftSaveData.Recalibrating;
            set => oracle.saveData.TemporalRiftSaveData.Recalibrating = value;
        }

        public static double MatrixFragmentsStabilizing
        {
            get => oracle.saveData.TemporalRiftSaveData.MatrixFragmentsStabilizing;
            set => oracle.saveData.TemporalRiftSaveData.MatrixFragmentsStabilizing =
                Math.Max(value, 0.0); // Ensures value is not less than 0
        }

        public static double StabilizedMatrixFragments
        {
            get => oracle.saveData.TemporalRiftSaveData.StabilizedMatrixFragments;
            set => oracle.saveData.TemporalRiftSaveData.StabilizedMatrixFragments =
                Math.Max(value, 0.0); // Ensures value is not less than 0
        }
    }
}