using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Blindsided.SaveData.ChronicleMinigames
{
    [HideReferenceObjectPicker]
    public class IdleDysonSwarmSaveData
    {
        public float CurrentRuntime;
        public IdleDysonSwarmData IdleDysonSwarmData = new();
        public int InfinityPoints;
        public float BotDistribution = 0.5f;

        public bool WorkerFoldout = true;
        public bool ScienceFoldout;
    }

    [HideReferenceObjectPicker]
    public class IdleDysonSwarmData
    {
        public Dictionary<string, IdleDysonSwarmBuildingSaveData> Buildings = new();
        public double Cash;
        public double Science;
        public double Bots;

        public double PanelsDecayed;

        public float TinkerProgress;
        public bool TinkerActive;
    }

    [HideReferenceObjectPicker]
    public class IdleDysonSwarmBuildingSaveData
    {
        public IdleDysonSwarmBuildingPurchaseData PurchaseData = new();
    }

    [HideReferenceObjectPicker]
    public class IdleDysonSwarmBuildingPurchaseData
    {
        public double Count => Purchased + Created;
        public double Purchased;
        public double Created;
    }
}