using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UpgradeSystem;

namespace RealmOfResearchNamespace.Upgrades
{
    public class UpgradeEnabledSetter : MonoBehaviour
    {
        private List<UpgradeReferences> _upgrades = new();

        private void OnEnable()
        {
            RealmOfResearchEvents.SetMaxedResearches += SetEnabled;
            //CategorySetter.instance.UpdateExpanders();
        }

        private void OnDisable()
        {
            RealmOfResearchEvents.SetMaxedResearches -= SetEnabled;
        }

        private void SetEnabled()
        {
            _upgrades = FindObjectsByType<UpgradeReferences>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .ToList();
            foreach (var upgrade in _upgrades) upgrade.SetActive();
            //CategorySetter.instance.UpdateExpanders();
        }
    }
}