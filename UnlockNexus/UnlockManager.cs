using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blindsided;
using Sirenix.OdinInspector;
using TimeManagement;
using UnityEngine;
using static Blindsided.Oracle;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;
using static RealmOfResearchNamespace.RealmOfResearchStaticReferences;
using static Blindsided.SaveData.StaticReferences;
using static EnginesOfExpansionNamespace.EnginesOfExpansionStaticReferences;
using static CollapseOfTimeNamespace.CollapseOfTimeStaticReferences;
using static ChronicleArchivesNamespace.ChronicleArchiveStaticReferences;

namespace UnlockNexus
{
    public class UnlockManager : SerializedMonoBehaviour
    {
        [TabGroup("Unlocks")] public Dictionary<UnlockKeys, UnlockData> BaseUnlocks = new();
        [TabGroup("Unlocks")] public Dictionary<UnlockKeys, UnlockData> ChildUnlocksDict = new();
        [TabGroup("Extra")] public Dictionary<UnlockKeys, UnlockData> UnlocksToProcess = new();
        [TabGroup("Extra")] public Transform unlockCardPrefab;

        private void Start()
        {
            LoadUnlocks();
            SaveUpgrades();
            SetupUnlocks();
            StartCoroutine(ProcessUnlockables());
        }

        private bool ProcessUnlock(UnlockKeys id)
        {
            switch (id)
            {
                case UnlockKeys.ChronoAperture:
                    return CurrentTime <= TimeManager.timeManager.MaxBackwardTime + 0.1;
                case UnlockKeys.FoundationOfResearch:
                    return CurrentTime >= TimeManager.timeManager.MaxForwardTime - 0.1;
                case UnlockKeys.OfflineTime:
                    return OfflineTime >= 300;
                case UnlockKeys.AutoPrestige:
                    return FopBuildings.EternityBeacon.NumberPurchased >= 42;
                // Foundation of Production
                case UnlockKeys.FopPrestige:
                    return FopCurrencies.StellarParticles >= 7000000;
                case UnlockKeys.FopAscend:
                    return FopCurrencies.TemporalShards >= 100;
                // Foundation of Production | Buildings
                case UnlockKeys.CelestialFoundry:
                    return FopBuildings.NebulaGenerator.NumberPurchased >= 100;
                case UnlockKeys.NovaSpire:
                    return FopBuildings.CelestialFoundry.NumberPurchased >= 100;
                case UnlockKeys.GalacticNexus:
                    return FopBuildings.NovaSpire.NumberPurchased >= 100;
                case UnlockKeys.SingularityLoom:
                    return FopBuildings.GalacticNexus.NumberPurchased >= 100;
                case UnlockKeys.EternityBeacon:
                    return FopBuildings.SingularityLoom.NumberPurchased >= 100;
                case UnlockKeys.InfinityCrucible:
                    return FopBuildings.EternityBeacon.NumberPurchased >= 100;
                // Realm of Research | Buildings
                case UnlockKeys.DarkArchitect:
                    return RorBuildings.FractureLoom.NumberPurchased >= 100;
                case UnlockKeys.CollapseEngine:
                    return RorBuildings.DarkArchitect.NumberPurchased >= 100;
                case UnlockKeys.SingularityVault:
                    return RorBuildings.CollapseEngine.NumberPurchased >= 100;
                // EnginesOfExpansion
                case UnlockKeys.EnginesOfExpansion:
                    return ResurgenceEnergy >= 5000;
                case UnlockKeys.EnergyAmplifier:
                    return Chronotons >= 5;
                case UnlockKeys.TemporalAccelerator:
                    return EnergyAmplifierUnlocked;
                case UnlockKeys.ResonanceChamber:
                    return TemporalAcceleratorUnlocked;
                // Collapse Of Time
                case UnlockKeys.CollapseOfTime:
                    return PerfectResonanceScore;
                // Chronicle Archives
                case UnlockKeys.ChronicleArchives:
                    return EntropyTierSaveData >= 4;
                case UnlockKeys.TemporalRifts:
                    return IdsCompletionCount > 0;
                //Tutorial
                case UnlockKeys.Introduction:
                    return introduction;
                case UnlockKeys.Tutorial1:
                    return tutorial1;
                case UnlockKeys.Tutorial2:
                    return tutorial2;
                case UnlockKeys.Tutorial3:
                    return tutorial3;
                case UnlockKeys.Tutorial4:
                    return tutorial4;
                // Lore
                case UnlockKeys.Lore2:
                    return lore2;
                case UnlockKeys.Lore2P2:
                    return lore2P2;
                case UnlockKeys.Lore3:
                    return lore3;
                case UnlockKeys.Lore4:
                    return lore4;
                case UnlockKeys.Lore5:
                    return lore5;
                case UnlockKeys.Lore6:
                    return lore6;
                case UnlockKeys.Lore7Trigger:
                    return ScaledTimeSpentInRealms.TemporalRifts >= 144000;
                case UnlockKeys.Lore7:
                    return lore7;
                default:
                    Debug.Log($"Unlock has no Condition: {id}");
                    return false;
            }
        }

        private IEnumerator ProcessUnlockables()
        {
            yield return 0;

            var keysCopy = UnlocksToProcess.Keys.ToList();
            foreach (var key in keysCopy)
                if (ProcessUnlock(key))
                    Unlock(key);

            yield return new WaitForSeconds(0.2f);
            StartCoroutine(ProcessUnlockables());
        }


        private void Unlock(UnlockKeys key)
        {
            UnlocksToProcess[key].Unlock();
            SaveUpgrades();
            EventHandler.UnlockNexusEvent();
            UnlocksToProcess[key].CheckChildUnlocks();
            UnlocksToProcess.Remove(key);
        }

        public bool CheckIfUnlocked(UnlockKeys id)
        {
            if (BaseUnlocks.TryGetValue(id, out var unlock))
                return unlock.Unlocked;

            if (ChildUnlocksDict.TryGetValue(id, out var childUnlock))
                return childUnlock.Unlocked;

            Debug.Log($"Unlock not found: {id}");
            return false;
        }


        private void SetupUnlocks()
        {
            foreach (var entry in BaseUnlocks)
            {
                var unlock = entry.Value;
                if (unlock.Unlocked)
                {
                    unlock.Setup();
                    unlock.CheckChildUnlocks();
                    continue;
                }

                UnlocksToProcess.TryAdd(unlock.ID, unlock);
                unlock.Setup();
            }
        }

        public void CheckUnlock(UnlockKeys id)
        {
            if (ChildUnlocksDict.TryGetValue(id, out var unlock))
            {
                if (unlock.Unlocked)
                {
                    unlock.Unlock();
                    unlock.CheckChildUnlocks();
                }
                else
                {
                    UnlocksToProcess.TryAdd(unlock.ID, unlock);
                    unlock.Setup();
                }
            }
        }

        private void SaveUpgrades()
        {
            foreach (var entry in BaseUnlocks)
            {
                var unlock = entry.Value;
                if (!oracle.saveData.Unlocks.TryAdd(unlock.ID, unlock.Unlocked))
                    oracle.saveData.Unlocks[unlock.ID] = unlock.Unlocked;
            }

            foreach (var entry in ChildUnlocksDict)
            {
                var unlock = entry.Value;
                if (!oracle.saveData.Unlocks.TryAdd(unlock.ID, unlock.Unlocked))
                    oracle.saveData.Unlocks[unlock.ID] = unlock.Unlocked;
            }
        }

        private void LoadUnlocks()
        {
            foreach (var entry in BaseUnlocks)
            {
                var unlock = entry.Value;
                if (oracle.saveData.Unlocks.TryGetValue(unlock.ID, out var unlocked))
                    unlock.Unlocked = unlocked;
            }

            foreach (var entry in ChildUnlocksDict)
            {
                var unlock = entry.Value;
                if (oracle.saveData.Unlocks.TryGetValue(unlock.ID, out var unlocked))
                    unlock.Unlocked = unlocked;
            }
        }

        [Button]
        public void SetUnlocksIDS()
        {
            foreach (var unlock in BaseUnlocks) unlock.Value.ID = unlock.Key;
            foreach (var unlock in ChildUnlocksDict) unlock.Value.ID = unlock.Key;
        }

        public enum UnlockKeys
        {
            FoundationOfResearch,
            FopPrestige,
            FopAscend,
            CelestialFoundry,
            NovaSpire,
            GalacticNexus,
            SingularityLoom,
            EternityBeacon,
            InfinityCrucible,
            DarkArchitect,
            CollapseEngine,
            SingularityVault,
            ChronoAperture,
            OfflineTime,
            Introduction,
            Tutorial1,
            Tutorial2,
            AutoPrestige,
            EnginesOfExpansion,
            EnergyAmplifier,
            TemporalAccelerator,
            ResonanceChamber,
            CollapseOfTime,
            ChronicleArchives,
            TemporalRifts,
            Tutorial3,
            Tutorial4,
            Lore2,
            Lore2P2,
            Lore3,
            Lore4,
            Lore5,
            Lore6,
            Lore7,
            Lore7Trigger
        }

        public bool introduction;
        public bool tutorial1;
        public bool tutorial2;
        public bool tutorial3;
        public bool tutorial4;
        public bool lore2;
        public bool lore2P2;
        public bool lore3;
        public bool lore4;
        public bool lore5;
        public bool lore6;
        public bool lore7;

        public void SetTutorialActive(bool active)
        {
            introduction = true;
            tutorial1 = active;
            tutorial2 = active;
            tutorial3 = active;
            tutorial4 = active;
        }

        public void ActivateTutorialOne()
        {
            tutorial1 = true;
        }

        public void ActivateTutorialTwo()
        {
            tutorial2 = true;
        }

        public void ActivateTutorialThree()
        {
            tutorial3 = true;
        }

        public void ActivateTutorialFour()
        {
            tutorial4 = true;
        }

        public void ActivateLoreTwo()
        {
            lore2 = true;
        }

        public void ActivateLoreTwoP2()
        {
            lore2P2 = true;
        }

        public void ActivateLoreThree()
        {
            lore3 = true;
        }

        public void ActivateLoreFour()
        {
            lore4 = true;
        }

        public void ActivateLoreFive()
        {
            lore5 = true;
        }

        public void ActivateLoreSix()
        {
            lore6 = true;
        }

        public void ActivateLoreSeven()
        {
            lore7 = true;
        }

        #region Singleton class

        public static UnlockManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        #endregion
    }
}