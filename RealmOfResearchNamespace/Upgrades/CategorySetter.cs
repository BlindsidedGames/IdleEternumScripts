using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UpgradeSystem;
using static RealmOfResearchNamespace.RealmOfResearchStaticReferences;

namespace RealmOfResearchNamespace.Upgrades
{
    public class CategorySetter : MonoBehaviour
    {
        public GameObject expander1;
        public List<UpgradeReferences> expander1References;
        public GameObject expander2;
        public List<UpgradeReferences> expander2References;
        public GameObject expander3;
        public List<UpgradeReferences> expander3References;
        public GameObject expander4;
        public List<UpgradeReferences> expander4References;
        public GameObject expander5;
        public List<UpgradeReferences> expander5References;
        public GameObject expander6;
        public List<UpgradeReferences> expander6References;
        public GameObject expander7;
        public List<UpgradeReferences> expander7References;
        public GameObject expander8;
        public List<UpgradeReferences> expander8References;
        public GameObject expander9;
        public List<UpgradeReferences> expander9References;


        public void UpdateExpanders()
        {
            CheckAndHide(expander1, expander1References);
            CheckAndHide(expander2, expander2References);
            CheckAndHide(expander3, expander3References);
            CheckAndHide(expander4, expander4References);
            CheckAndHide(expander5, expander5References);
            CheckAndHide(expander6, expander6References);
            CheckAndHide(expander7, expander7References);
            CheckAndHide(expander8, expander8References);
            CheckAndHide(expander9, expander9References);
        }

        private void CheckAndHide(GameObject expander, List<UpgradeReferences> references)
        {
            if (expander == null || references == null)
                return;

            if (!HideMaxedResearches)
            {
                expander.SetActive(true);
                return;
            }

            var allMaxed = references.All(r => r.upgrade != null && r.upgrade.IsMaxed);

            expander.SetActive(!allMaxed);
        }

        //static singleton
        public static CategorySetter instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
    }
}