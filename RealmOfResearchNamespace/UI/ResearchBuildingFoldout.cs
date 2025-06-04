using MPUIKIT;
using UnityEngine;
using UnityEngine.UI;
using static RealmOfResearchNamespace.RealmOfResearchStaticReferences;

namespace RealmOfResearchNamespace.UI
{
    public class ResearchBuildingFoldout : MonoBehaviour
    {
        public Button FoldoutButton;
        public GameObject FoldoutPanel;
        public MPImageBasic expandedImage;

        private void Start()
        {
            FoldoutButton.onClick.AddListener(() =>
            {
                FoldoutPanel.SetActive(!FoldoutPanel.activeSelf);
                ResearchBuildingFoldoutPreference = FoldoutPanel.activeSelf;
                expandedImage.FlipVertical = !FoldoutPanel.activeSelf;
            });

            FoldoutPanel.SetActive(ResearchBuildingFoldoutPreference);
            expandedImage.FlipVertical = !FoldoutPanel.activeSelf;
        }
    }
}