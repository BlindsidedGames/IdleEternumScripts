using System;
using System.Collections.Generic;
using Blindsided.SaveData;
using Blindsided.SaveData.ChronicleMinigames;
using Sirenix.OdinInspector;
using UnityEngine;
using UpgradeSystem;
using static Blindsided.SaveData.StaticReferences;
using static ChronicleArchivesNamespace.IdleDysonSwarm.IdsStaticReferences;
using static ChronicleArchivesNamespace.ChronicleArchiveStaticReferences;

namespace ChronicleArchivesNamespace.IdleDysonSwarm
{
    public class IdsProductionManager : SerializedMonoBehaviour, IUpgradable
    {
        public LayerSwitcher layerSwitcher;
        public IdsBotManager BotManager;
        public Tinker Tinker;
        public IdsBuildingController AssemblyLine;
        public IdsBuildingController AiManager;
        public IdsBuildingController Server;
        public IdsBuildingController DataCenter;
        public IdsBuildingController Planet;

        public GameObject IdsTab;
        public List<UpgradeSo> IdsUpgrades;
        public List<UpgradeReferences> autoPurchasedUpgrades;


        private void Start()
        {
            Register();
        }

        private void OnEnable()
        {
            Register();
        }

        private void OnDisable()
        {
            Unregister();
        }

        private void Update()
        {
            if (LayerTab == SaveData.Tab.ChronicleArchives && IdsTab.activeSelf)
                if (TimeScale != 0)
                {
                    var speed = Math.Abs(TimeScale) * Time.deltaTime;
                    BotManager.ProduceCashAndScience(speed);
                    Tinker.Progress(speed);
                    AssemblyLine.Produce(speed);
                    AiManager.Produce(speed);
                    Server.Produce(speed);
                    DataCenter.Produce(speed);
                    Planet.Produce(speed);
                    CurrentRuntime += speed;

                    if (Bots >= RequiredBotsForInfinity)
                    {
                        if (InfinityPoints < 27)
                        {
                            OnInfinityDataToReset = new IdleDysonSwarmData();
                            InfinityPoints++;
                        }
                        else
                        {
                            IdsCompletionCount++;
                            if (CurrentRuntime < IdsFastestCompletionTime) IdsFastestCompletionTime = CurrentRuntime;
                            ChronicleArchiveStaticReferences.IdleDysonSwarmSaveData = new IdleDysonSwarmSaveData();
                            layerSwitcher.SetTab(SaveData.Tab.ChronicleArchives);
                        }

                        IdsEvents.OnInfinity();
                        ResetIdsUpgrades();
                        ApplyInfinityBuffs();
                    }

                    IdsEvents.OnUpdateUI();
                    if (GetStat(StatType.IdsAutoResearch).CachedValue > 0)
                        foreach (var upgrade in autoPurchasedUpgrades)
                            upgrade.AutoPurchaseUpgrade();
                }
        }

        private void ApplyInfinityBuffs()
        {
            AssemblyLine.Purchased += GetStat(StatType.StartingIdsAssemblyLines).CachedValue;
            AiManager.Purchased += GetStat(StatType.StartingIdsAiManagers).CachedValue;
            Server.Purchased += GetStat(StatType.StartingIdsServers).CachedValue;
            DataCenter.Purchased += GetStat(StatType.StartingIdsDataCenters).CachedValue;
            Planet.Purchased += GetStat(StatType.StartingIdsPlanets).CachedValue;
            var upgradeSteps = new (int threshold, Action<int> applyUpgrade)[]
            {
                (5, points => AssemblyLine.Purchased += points - 4),
                (10, points => AiManager.Purchased += points - 9),
                (15, points => Server.Purchased += points - 14),
                (20, points => DataCenter.Purchased += points - 19),
                (25, points => Planet.Purchased += points - 24)
            };

            foreach (var (threshold, applyUpgrade) in upgradeSteps)
                if (InfinityPoints >= threshold)
                    applyUpgrade(InfinityPoints);
        }

        public void ResetIdsUpgrades()
        {
            foreach (var upgrade in IdsUpgrades)
                if (upgrade != null)
                    upgrade.ResetUpgrade();
        }

        [SerializeField] private List<string> tags = new() { "IdsPM" };
        public Dictionary<StatType, UpgradableStat> UpgradableStats = new();

        public List<string> GetTags()
        {
            return tags;
        }

        public UpgradableStat GetStat(StatType statType)
        {
            return UpgradableStats.GetValueOrDefault(statType);
        }

        public void Register()
        {
            UpgradeManager.Instance?.RegisterEntity(this);
        }

        public void Unregister()
        {
            UpgradeManager.Instance?.UnregisterEntity(this);
        }
    }
}