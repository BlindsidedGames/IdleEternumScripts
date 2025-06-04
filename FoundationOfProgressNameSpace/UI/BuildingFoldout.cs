using MPUIKIT;
using UnityEngine;
using UnityEngine.UI;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;

namespace FoundationOfProgressNameSpace.UI
{
    public class BuildingFoldout : MonoBehaviour
    {
        public Button FoldoutButton;
        public GameObject FoldoutPanel;
        public MPImageBasic expandedImage;

        private void Start()
        {
            FoldoutButton.onClick.AddListener(() =>
            {
                FoldoutPanel.SetActive(!FoldoutPanel.activeSelf);
                BuildingFoldoutPreference = FoldoutPanel.activeSelf;
                expandedImage.FlipVertical = !FoldoutPanel.activeSelf;
            });

            FoldoutPanel.SetActive(BuildingFoldoutPreference);
            expandedImage.FlipVertical = !FoldoutPanel.activeSelf;
        }
    }
}