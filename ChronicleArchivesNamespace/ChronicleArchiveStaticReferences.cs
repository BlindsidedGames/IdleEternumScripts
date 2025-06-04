using System.Collections.Generic;
using Blindsided.SaveData;
using Blindsided.SaveData.ChronicleMinigames;
using static Blindsided.Oracle;

namespace ChronicleArchivesNamespace
{
    public static class ChronicleArchiveStaticReferences
    {
        public static ChronicleArchivesSaveData ChronicleArchivesSaveData =>
            oracle.saveData.ChronicleArchivesSaveDataData;

        public static long Chronicles
        {
            get => ChronicleArchivesSaveData.Chronicles;
            set => ChronicleArchivesSaveData.Chronicles = value;
        }

        public static IdleDysonSwarmSaveData IdleDysonSwarmSaveData
        {
            get => ChronicleArchivesSaveData.IdleDysonSwarm;
            set => ChronicleArchivesSaveData.IdleDysonSwarm = value;
        }

        public static long IdsCompletionCount
        {
            get => ChronicleArchivesSaveData.IdsCompletionCount;
            set => ChronicleArchivesSaveData.IdsCompletionCount = value;
        }

        public static float IdsFastestCompletionTime
        {
            get => ChronicleArchivesSaveData.IdsFastestCompletionTime;
            set => ChronicleArchivesSaveData.IdsFastestCompletionTime = value;
        }

        public static float IdsGeneratorProgress
        {
            get => ChronicleArchivesSaveData.IdsGeneratorProgress;
            set => ChronicleArchivesSaveData.IdsGeneratorProgress = value;
        }

        public static Dictionary<string, CardSaveDataClass> CardSaveData
        {
            get => ChronicleArchivesSaveData.CardSaveData;
            set => ChronicleArchivesSaveData.CardSaveData = value;
        }
    }
}