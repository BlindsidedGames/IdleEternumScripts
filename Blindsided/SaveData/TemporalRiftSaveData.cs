using System.Collections.Generic;
using Sirenix.OdinInspector;
using TemporalRiftNamespace;

namespace Blindsided.SaveData
{
    [HideReferenceObjectPicker]
    public class TemporalRiftSaveData
    {
        public Dictionary<EssenceType, EssenceSaveData> SavedEssenceConverters = new();
        public Dictionary<string, TemporalRiftUpgradeSaveData> TemporalRiftUpgrades = new();
        public double EternumEssence;
        public float RecalibrationTime;
        public float CurrentRecalibrationTimeTotal;
        public bool Recalibrating;
        public double MatrixFragmentsStabilizing;
        public double StabilizedMatrixFragments;
    }

    public class TemporalRiftUpgradeSaveData
    {
        public string UpgradeGuid;
        public double TimeInvested;
        public bool IsActive;
        public bool IsEmpowered;
    }
}