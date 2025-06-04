using MPUIKIT;
using UnityEngine;
using UnityEngine.UI;

namespace EnginesOfExpansionNamespace.UI
{
    public class EngineFoldout : MonoBehaviour
    {
        public Button FoldoutButton;
        public GameObject FoldoutPanel;
        public MPImageBasic expandedImage;

        private void Start()
        {
            /*FoldoutButton.onClick.AddListener(() =>
            {
                FoldoutPanel.SetActive(!FoldoutPanel.activeSelf);
                EngineFoldoutPreference = FoldoutPanel.activeSelf;
                expandedImage.FlipVertical = !FoldoutPanel.activeSelf;
            });

            FoldoutPanel.SetActive(EngineFoldoutPreference);
            expandedImage.FlipVertical = !FoldoutPanel.activeSelf;*/
        }
    }
}