using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Blindsided.SaveData
{
    [HideReferenceObjectPicker]
    public class CollapseOfTimeSaveData
    {
        public Dictionary<string, CotUpgradeSaveData> CotUpgrades = new();
        public double TemporalWorkers;
        public float WorkerDistribution = 0.9f;
        public bool UpgradeVsGrow = true; // false = upgrade, true = grow
        public int EntropyTier;
        public double CurrentEntropyProgress;
    }

    public class CotUpgradeSaveData
    {
        public double TimeInvested;
        public bool IsActive;
    }
}