using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using static Blindsided.SaveData.StaticReferences;
using static UpgradeSystem.CurrencyManager;
using EventHandler = Blindsided.EventHandler;

namespace UpgradeSystem
{
    public class UpgradeManager : SerializedMonoBehaviour
    {
        public List<UpgradeSo> availableUpgrades; // Assign in Inspector with Odin
        public List<IUpgradable> Entities = new(); // Populate with buildings, global stats, etc.
        private readonly Dictionary<IUpgradable, HashSet<string>> _entityTagsCache = new();

        public static UpgradeManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            availableUpgrades = Resources.FindObjectsOfTypeAll<UpgradeSo>().ToList();
        }

        private void OnEnable()
        {
            EventHandler.OnResetData += ResetAllUpgrades;
        }

        private void OnDisable()
        {
            ClearAllUpgradesFromEntities();
        }

        private void Start()
        {
            ClearAllUpgradesFromEntities();
            ApplyAllUpgrades();
        }

        public void PurchaseUpgrade(UpgradeSo upgrade)
        {
            UpgradeLevels.TryGetValue(upgrade.guid, out var level);
            if (upgrade.maxPurchases >= 0 && level >= upgrade.maxPurchases) return;
            var cost = upgrade.GetCurrentCost(level);
            if (!CheckAffordability(cost, upgrade.costCurrencyType, true)) return;
            UpgradeLevels[upgrade.guid] = level + 1;
            ApplyUpgrade(upgrade, level + 1);
        }

        public void IncrementUpgradeLevel(UpgradeSo upgrade)
        {
            UpgradeLevels.TryGetValue(upgrade.guid, out var level);
            UpgradeLevels[upgrade.guid] = level + 1;
            ApplyUpgrade(upgrade, level + 1);
        }

        public void AddLevelsToUpgrade(UpgradeSo upgrade, int levels)
        {
            UpgradeLevels.TryGetValue(upgrade.guid, out var level);
            UpgradeLevels[upgrade.guid] = level + levels;
            ApplyUpgrade(upgrade, level + levels);
        }

        public void SetUpgradeLevel(UpgradeSo upgrade, int level)
        {
            UpgradeLevels[upgrade.guid] = level;
            ApplyUpgrade(upgrade, level);
        }

        private void ApplyUpgrade(UpgradeSo upgrade, int level)
        {
            foreach (var effectGroup in upgrade.effectGroups)
            {
                if (effectGroup.tags == null || !effectGroup.tags.Any())
                    continue;

                var affectedEntities = Entities.Where(e => _entityTagsCache[e].Intersect(effectGroup.tags).Any());

                foreach (var entity in affectedEntities)
                foreach (var mod in effectGroup.modifications)
                {
                    var stat = entity.GetStat(mod.statType);
                    if (stat == null) continue;

                    // Find modifier based on upgrade and specific modifier type.
                    var modifier = FindModifier(stat, upgrade, mod.modifierType);
                    if (modifier == null)
                    {
                        modifier = new Modifier { sourceUpgrade = upgrade };
                        if (mod.modifierType == ModifierType.Additive)
                            stat.AddAdditiveModifier(modifier);
                        else
                            stat.AddMultiplicativeModifier(modifier);
                    }

                    modifier.level = level;
                    modifier.value = mod.modifierType == ModifierType.Additive
                        ? mod.baseValue * level
                        : Math.Pow(mod.baseValue, level);
                    stat.CacheStat();
                }
            }
        }

        public void ResetUpgrade(UpgradeSo upgrade)
        {
            UpgradeLevels[upgrade.guid] = 0;
            foreach (var entity in Entities)
            foreach (var statType in Enum.GetValues(typeof(StatType)).Cast<StatType>())
            {
                var stat = entity.GetStat(statType);
                if (stat != null)
                {
                    // Remove any additive modifiers from this upgrade
                    stat.additiveModifiers.RemoveAll(mod => mod.sourceUpgrade == upgrade);
                    // Remove any multiplicative modifiers from this upgrade
                    stat.multiplicativeModifiers.RemoveAll(mod => mod.sourceUpgrade == upgrade);
                    stat.CacheStat();
                }
            }
        }


        private Modifier FindModifier(UpgradableStat stat, UpgradeSo upgrade, ModifierType modifierType)
        {
            if (modifierType == ModifierType.Additive)
                return stat.additiveModifiers.FirstOrDefault(m => m.sourceUpgrade == upgrade);
            return stat.multiplicativeModifiers.FirstOrDefault(m => m.sourceUpgrade == upgrade);
        }


        public void RegisterEntity(IUpgradable entity)
        {
            if (!Entities.Contains(entity))
            {
                Entities.Add(entity);
                var tags = entity.GetTags();
                _entityTagsCache[entity] = tags != null ? new HashSet<string>(tags) : new HashSet<string>();
            }
        }

        [Button]
        private void ApplyAllUpgrades()
        {
            foreach (var upgrade in availableUpgrades)
                if (UpgradeLevels.TryGetValue(upgrade.guid, out var level) && level > 0)
                    ApplyUpgrade(upgrade, level);
        }

        private void ApplyAllUpgradesToEntity(IUpgradable entity)
        {
            var entityTags = _entityTagsCache[entity];
            foreach (var upgrade in availableUpgrades)
                if (UpgradeLevels.TryGetValue(upgrade.guid, out var level) && level > 0)
                    foreach (var effectGroup in upgrade.effectGroups)
                        if (effectGroup.tags != null && effectGroup.tags.Any() &&
                            entityTags.Intersect(effectGroup.tags).Any())
                            foreach (var mod in effectGroup.modifications)
                            {
                                var stat = entity.GetStat(mod.statType);
                                if (stat == null) continue;

                                // Use the updated FindModifier that differentiates by ModifierType.
                                var modifier = FindModifier(stat, upgrade, mod.modifierType);
                                if (modifier == null)
                                {
                                    modifier = new Modifier { sourceUpgrade = upgrade };
                                    if (mod.modifierType == ModifierType.Additive)
                                        stat.AddAdditiveModifier(modifier);
                                    else
                                        stat.AddMultiplicativeModifier(modifier);
                                }

                                modifier.level = level;
                                modifier.value = mod.modifierType == ModifierType.Additive
                                    ? mod.baseValue * level
                                    : Math.Pow(mod.baseValue, level);
                                stat.CacheStat();
                            }
        }


        [Button]
        public void ClearAllUpgradesFromEntities()
        {
            foreach (var entity in Entities)
            foreach (var statType in Enum.GetValues(typeof(StatType)).Cast<StatType>())
            {
                var stat = entity.GetStat(statType);
                if (stat != null)
                {
                    stat.additiveModifiers.Clear();
                    stat.multiplicativeModifiers.Clear();
                    stat.CacheStat();
                }
            }
        }

        public void UnregisterEntity(IUpgradable entity)
        {
            Entities.Remove(entity);
            _entityTagsCache.Remove(entity);
        }

        public void ResetAllUpgrades()
        {
            UpgradeLevels.Clear();
            ClearAllUpgradesFromEntities();
        }
    }
}