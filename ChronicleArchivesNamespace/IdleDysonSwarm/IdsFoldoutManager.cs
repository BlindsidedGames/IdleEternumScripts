using MPUIKIT;
using UnityEngine;
using UnityEngine.UI;
using static ChronicleArchivesNamespace.IdleDysonSwarm.IdsStaticReferences;

namespace ChronicleArchivesNamespace.IdleDysonSwarm
{
    public class IdsFoldoutManager : MonoBehaviour
    {
        public Button workerFoldoutButton;
        public GameObject workerFoldoutPanel;
        public MPImageBasic workerExpandedImage;

        public Button scienceFoldoutButton;
        public GameObject scienceFoldoutPanel;
        public MPImageBasic scienceExpandedImage;

        private void Start()
        {
            workerFoldoutButton.onClick.AddListener(() =>
            {
                workerFoldoutPanel.SetActive(!workerFoldoutPanel.activeSelf);
                WorkerFoldoutPreference = workerFoldoutPanel.activeSelf;
                workerExpandedImage.FlipVertical = !workerFoldoutPanel.activeSelf;
            });

            scienceFoldoutButton.onClick.AddListener(() =>
            {
                scienceFoldoutPanel.SetActive(!scienceFoldoutPanel.activeSelf);
                ScienceFoldoutPreference = scienceFoldoutPanel.activeSelf;
                scienceExpandedImage.FlipVertical = !scienceFoldoutPanel.activeSelf;
            });

            scienceFoldoutPanel.SetActive(ScienceFoldoutPreference);
            scienceExpandedImage.FlipVertical = !scienceFoldoutPanel.activeSelf;

            workerFoldoutPanel.SetActive(WorkerFoldoutPreference);
            workerExpandedImage.FlipVertical = !workerFoldoutPanel.activeSelf;
        }
    }
}