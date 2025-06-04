using System;
using Blindsided.SaveData;
using MPUIKIT;
using Sirenix.OdinInspector;
using TimeManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;
using EventHandler = Blindsided.EventHandler;
using static ChronicleArchivesNamespace.ChronicleArchiveStaticReferences;
using static Blindsided.SaveData.TextColourStrings;


namespace ChronicleArchivesNamespace
{
    public class CardReferences : MonoBehaviour
    {
        [SerializeField] public string guid;
        public CardSaveDataClass cardSaveData;
        public TMP_Text cardName;
        public TMP_Text cardLevel;
        public TMP_Text cardDescription;
        public MPImage cardImage;
        public MPImage cardLevelFill;
        public TMP_Text cardLevelText;

        public Button flipButton;
        public GameObject front;
        public GameObject back;

        private bool _flipStatus;
        public bool updateTimeScale;

        public UpgradeSo upgrade;

        private void OnEnable()
        {
            EventHandler.OnLoadData += OnLoadData;
        }

        private void OnDisable()
        {
            EventHandler.OnLoadData -= OnLoadData;
        }

        private void OnLoadData()
        {
            SetUpgradeData();
            SetTexts();
        }

        [Button]
        public void SetName()
        {
            cardName.text = upgrade.name;
            gameObject.name = upgrade.name + "_Card";
        }

        private void Start()
        {
            SetStatus();
            flipButton.onClick.AddListener(FlipCard);
            SetUpgradeData();
            SetTexts();
            cardLevelFill.fillAmount = cardSaveData.CardCount / (float)Math.Floor(upgrade.GetCurrentCost());
        }

        public void EarnCards(int amount = 1)
        {
            cardSaveData.CardCount += amount;
            CheckForLevelUp();
            SetTexts();
        }

        private void CheckForLevelUp()
        {
            var currentCost = (long)Math.Floor(upgrade.GetCurrentCost());
            while (cardSaveData.CardCount >= currentCost)
            {
                cardSaveData.CardCount -= currentCost;
                currentCost = (long)Math.Floor(upgrade.GetCurrentCost());
                UpgradeManager.Instance.IncrementUpgradeLevel(upgrade);
            }

            if (updateTimeScale) TimeManager.timeManager.SetTimeScale();
            cardLevelFill.fillAmount = cardSaveData.CardCount / (float)currentCost;
        }

        private void SetTexts()
        {
            cardName.text = upgrade.name;
            cardDescription.text = upgrade.description;
            cardLevel.text = $"Level | {ColourGreen}{upgrade.GetCurrentLevel().ToString()}{EndColour}";

            var currentCost = (long)Math.Floor(upgrade.GetCurrentCost());
            cardLevelText.text =
                $"{ColourGreen}{cardSaveData.CardCount.ToString()}{EndColour} / {ColourHighlight}{currentCost.ToString()}{EndColour}";
        }

        private void FlipCard()
        {
            _flipStatus = !_flipStatus;
            SetStatus();
        }

        private void SetStatus()
        {
            front.SetActive(!_flipStatus);
            back.SetActive(_flipStatus);
        }

        private void SetUpgradeData()
        {
            CardSaveData.TryAdd(guid, new CardSaveDataClass());
            cardSaveData = CardSaveData[guid];
        }

        [Button]
        public void CreateGuid()
        {
            guid = Guid.NewGuid().ToString();
        }
    }
}