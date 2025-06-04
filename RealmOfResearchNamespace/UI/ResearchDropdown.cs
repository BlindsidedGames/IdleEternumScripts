using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static RealmOfResearchNamespace.RealmOfResearchStaticReferences;

namespace RealmOfResearchNamespace.UI
{
    public class ResearchDropdown : MonoBehaviour
    {
        public TMP_Dropdown dropdown;
        public GameObject[] researchObjects;
        public GameObject[] enabledGetterObjects;
        private readonly List<GameObject> _availableResearchObjects = new();

        private void Start()
        {
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            StartCoroutine(InitialDropdownSetting());
        }

        private IEnumerator InitialDropdownSetting()
        {
            yield return new WaitForEndOfFrame();
            dropdown.value = UpgradeDropdownIndex;
        }

        public void SetUpgradeOptions()
        {
            dropdown.options.Clear();
            _availableResearchObjects.Clear();
            for (var index = 0; index < enabledGetterObjects.Length; index++)
            {
                var researchObject = enabledGetterObjects[index];
                if (researchObject.activeSelf)
                {
                    var option = new TMP_Dropdown.OptionData(researchObject.name);
                    dropdown.options.Add(option);
                    _availableResearchObjects.Add(researchObjects[index]);
                }
            }
        }

        private void OnDropdownValueChanged(int arg0)
        {
            foreach (var researchObject in researchObjects) researchObject.SetActive(false);
            if (arg0 >= 0 && arg0 < _availableResearchObjects.Count) _availableResearchObjects[arg0].SetActive(true);
            UpgradeDropdownIndex = arg0;
        }
    }
}