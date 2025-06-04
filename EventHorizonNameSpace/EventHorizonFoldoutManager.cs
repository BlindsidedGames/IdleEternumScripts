using MPUIKIT;
using UnityEngine;
using UnityEngine.UI;
using static Blindsided.SaveData.StaticReferences;

namespace EventHorizonNameSpace
{
    public class EventHorizonFoldoutManager : MonoBehaviour
    {
        public Button statsFoldoutButton;
        public GameObject statsFoldoutPanel;
        public MPImageBasic statsExpandedImage;

        public Button settingsFoldoutButton;
        public GameObject settingsFoldoutPanel;
        public MPImageBasic settingsExpandedImage;

        public Button shopFoldoutButton;
        public GameObject shopFoldoutPanel;
        public MPImageBasic shopExpandedImage;

        private void Start()
        {
            statsFoldoutButton.onClick.AddListener(() =>
            {
                statsFoldoutPanel.SetActive(!statsFoldoutPanel.activeSelf);
                SavedPreferences.StatsFoldout = statsFoldoutPanel.activeSelf;
                statsExpandedImage.FlipVertical = !statsFoldoutPanel.activeSelf;
            });

            settingsFoldoutButton.onClick.AddListener(() =>
            {
                settingsFoldoutPanel.SetActive(!settingsFoldoutPanel.activeSelf);
                SavedPreferences.SettingsFoldout = settingsFoldoutPanel.activeSelf;
                settingsExpandedImage.FlipVertical = !settingsFoldoutPanel.activeSelf;
            });

            shopFoldoutButton.onClick.AddListener(() =>
            {
                shopFoldoutPanel.SetActive(!shopFoldoutPanel.activeSelf);
                SavedPreferences.ShopFoldout = shopFoldoutPanel.activeSelf;
                shopExpandedImage.FlipVertical = !shopFoldoutPanel.activeSelf;
            });

            statsFoldoutPanel.SetActive(SavedPreferences.StatsFoldout);
            statsExpandedImage.FlipVertical = !statsFoldoutPanel.activeSelf;

            settingsFoldoutPanel.SetActive(SavedPreferences.SettingsFoldout);
            settingsExpandedImage.FlipVertical = !settingsFoldoutPanel.activeSelf;

            shopFoldoutPanel.SetActive(SavedPreferences.ShopFoldout);
            shopExpandedImage.FlipVertical = !shopFoldoutPanel.activeSelf;
        }
    }
}