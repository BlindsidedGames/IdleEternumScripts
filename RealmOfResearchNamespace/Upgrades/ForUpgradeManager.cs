using System.Collections.Generic;
using FoundationOfProgressNameSpace;
using UnityEngine;
using UpgradeSystem;
using static Blindsided.Oracle;

namespace RealmOfResearchNamespace.Upgrades
{
    public class ForUpgradeManager : MonoBehaviour
    {
        public List<ForUpgrade> allUpgrades;
        public Building[] allBuildings;
        public Building starCradle => allBuildings[0];
        public Building nebulaGenerator => allBuildings[1];
        public Building celestialFoundry => allBuildings[2];
        public Building novaSpire => allBuildings[3];
        public Building galacticNexus => allBuildings[4];
        public Building singularityLoom => allBuildings[5];
        public Building eternityBeacon => allBuildings[6];
        public Building infinityCrucible => allBuildings[7];

        public Transform upgradeParent;
        public Transform upgradePrefab;

        public void CreateUpgrades()
        {
            ResetUpgrades();
            LoadUpgrades();
            SetupUpgrades();
        }


        public void SetupUpgrades()
        {
            foreach (var upgrade in allUpgrades)
            {
                var ur = Instantiate(upgradePrefab, upgradeParent).GetComponent<UpgradeReferences>();
                upgrade.instantiatedReference = ur.gameObject;
                //ur.SetTexts();
                upgrade.ApplyUpgrade();
                upgrade.unlocked = false;
                upgrade.SetActive();
            }
        }

        public void ResetUpgrades()
        {
            foreach (var upgrade in allUpgrades) upgrade.owned = 0;
        }

        public void SaveUpgrades()
        {
            foreach (var upgrade in allUpgrades)
                if (!oracle.saveData.RealmOfResearchSaveDataData.UpgradesOwned.TryAdd(upgrade.upgradeName,
                        upgrade.owned))
                    oracle.saveData.RealmOfResearchSaveDataData.UpgradesOwned[upgrade.upgradeName] = upgrade.owned;
        }

        public void LoadUpgrades()
        {
            foreach (var upgrade in allUpgrades)
                if (oracle.saveData.RealmOfResearchSaveDataData.UpgradesOwned.TryGetValue(upgrade.upgradeName,
                        out var owned))
                    upgrade.owned = owned;
        }

        #region Singleton class

        public static ForUpgradeManager forUpgradeManager;

        private void Awake()
        {
            if (forUpgradeManager == null)
                forUpgradeManager = this;
            //DontDestroyOnLoad(gameObject);
            else
                Destroy(gameObject);
        }

        #endregion
    }
}