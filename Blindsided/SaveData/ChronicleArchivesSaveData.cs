using System.Collections.Generic;
using Blindsided.SaveData.ChronicleMinigames;
using Sirenix.OdinInspector;

namespace Blindsided.SaveData
{
    [HideReferenceObjectPicker]
    public class ChronicleArchivesSaveData
    {
        public long Chronicles = 0;
        public Dictionary<string, CardSaveDataClass> CardSaveData = new();

        public IdleDysonSwarmSaveData IdleDysonSwarm = new();
        public long IdsCompletionCount;
        public float IdsFastestCompletionTime = float.MaxValue;
        public float IdsGeneratorProgress;
    }

    public class CardSaveDataClass
    {
        public long CardCount;
    }
}