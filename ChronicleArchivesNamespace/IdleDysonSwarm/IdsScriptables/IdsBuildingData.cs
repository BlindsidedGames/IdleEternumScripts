using System.Collections.Generic;
using Blindsided.SaveData.ChronicleMinigames;
using Sirenix.OdinInspector;
using UnityEngine;
using UpgradeSystem;
using static ChronicleArchivesNamespace.IdleDysonSwarm.IdsStaticReferences;
using EventHandler = Blindsided.EventHandler;

namespace ChronicleArchivesNamespace.IdleDysonSwarm.IdsScriptables
{
    [CreateAssetMenu(fileName = "Ids Balance", menuName = "Game/Ids Building Data")]
    public class IdsBuildingData : SerializedScriptableObject
    {
        public string BuildingName;
        [FoldoutGroup("Save Data")] public IdleDysonSwarmBuildingSaveData BuildingSaveData;
        [FoldoutGroup("Produces")] public IdsBuildingData ProducedBuilding;
        [FoldoutGroup("Balance")] public float CostExponent = 1.06f;
        [FoldoutGroup("Balance")] public float BaseCost = 10;
        public List<string> tags = new() { "IdsBuilding" };
        public Dictionary<StatType, UpgradableStat> UpgradableStats = new();


        private void Awake()
        {
            SetupSoStats();
        }

        public void CacheStats()
        {
            foreach (var stat in UpgradableStats) stat.Value.CacheStat();
        }

        private void SetupSoStats()
        {
            UpgradableStats.Clear();
            UpgradableStats.Add(StatType.IdsCostMultiplier, new UpgradableStat { baseValue = 1 });
            UpgradableStats.Add(StatType.IdsProduction, new UpgradableStat { baseValue = 1 });
        }


        [Button]
        public void ApplyBaseStats()
        {
            UpgradableStats.TryAdd(StatType.IdsCostMultiplier, new UpgradableStat { baseValue = 1 });
            UpgradableStats.TryAdd(StatType.IdsProduction, new UpgradableStat { baseValue = 1 });
        }

        private void OnEnable()
        {
            EventHandler.OnSaveData += SaveBuildingData;
            EventHandler.OnLoadData += LoadBuildingData;
            EventHandler.OnResetData += ResetBuildingData;
            IdsEvents.Infinity += ResetBuildingData;
        }

        private void OnDisable()
        {
            EventHandler.OnSaveData -= SaveBuildingData;
            EventHandler.OnLoadData -= LoadBuildingData;
            EventHandler.OnResetData -= ResetBuildingData;
            IdsEvents.Infinity -= ResetBuildingData;
        }

        private void SaveBuildingData()
        {
            if (!AllBuildingsSaveData.TryAdd(BuildingName, BuildingSaveData))
                AllBuildingsSaveData[BuildingName] = BuildingSaveData;
        }

        private void LoadBuildingData()
        {
            if (AllBuildingsSaveData.TryGetValue(BuildingName, out var buildingData)) BuildingSaveData = buildingData;
        }

        private void ResetBuildingData()
        {
            BuildingSaveData = new IdleDysonSwarmBuildingSaveData();
        }
    }
}