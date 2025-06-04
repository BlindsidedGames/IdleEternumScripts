using System;
using System.Collections.Generic;
using Blindsided.SaveData;
using UnityEngine;
using static Blindsided.Oracle;

namespace CollapseOfTimeNamespace
{
    public class CollapseOfTimeStaticReferences
    {
        public static CollapseOfTimeSaveData CollapseOfTimeSaveData =>
            oracle.saveData.CollapseOfTimeSaveDataData;

        public static Dictionary<string, CotUpgradeSaveData> CotUpgrades =>
            oracle.saveData.CollapseOfTimeSaveDataData.CotUpgrades;

        public static double TemporalWorkers
        {
            get => Math.Max(1, oracle.saveData.CollapseOfTimeSaveDataData.TemporalWorkers);
            set => oracle.saveData.CollapseOfTimeSaveDataData.TemporalWorkers = value;
        }

        public static float WorkerDistribution
        {
            get => oracle.saveData.CollapseOfTimeSaveDataData.WorkerDistribution;
            set => oracle.saveData.CollapseOfTimeSaveDataData.WorkerDistribution =
                Mathf.Clamp(value, 0, 1);
        }

        public static bool UpgradeVsGrow
        {
            get => oracle.saveData.CollapseOfTimeSaveDataData.UpgradeVsGrow;
            set => oracle.saveData.CollapseOfTimeSaveDataData.UpgradeVsGrow = value;
        }

        public static int EntropyTierSaveData
        {
            get => oracle.saveData.CollapseOfTimeSaveDataData.EntropyTier;
            set => oracle.saveData.CollapseOfTimeSaveDataData.EntropyTier = value;
        }

        public static double CurrentEntropyProgress
        {
            get => oracle.saveData.CollapseOfTimeSaveDataData.CurrentEntropyProgress;
            set => oracle.saveData.CollapseOfTimeSaveDataData.CurrentEntropyProgress = value;
        }

        public static double researchMultiplier;
        public static int activeResearches;
    }
}