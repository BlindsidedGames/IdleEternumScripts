using UnityEngine;

namespace RealmOfResearchNamespace.UI
{
    public class ResetDropdownOptions : MonoBehaviour
    {
        public ResearchDropdown researchDropdown;

        private void OnEnable()
        {
            researchDropdown.SetUpgradeOptions();
        }
    }
}