using System;
using System.Collections.Generic;
using Blindsided.Utilities;
using Sirenix.OdinInspector;
using TemporalRiftNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;
using static Blindsided.SaveData.SaveData;
using static Blindsided.SaveData.StaticReferences;
using static Blindsided.SaveData.TextColourStrings;
using EventHandler = Blindsided.EventHandler;

namespace TimeManagement
{
    public class TimeManager : SerializedMonoBehaviour, IUpgradable
    {
        public TMP_Text timeText;
        public TMP_Text timeScaleText;
        public TMP_Text maxForwardTimeText;
        public TMP_Text maxBackwardTimeText;

        public Slider timeLine;

        public TMP_Text offlineTimeText;
        public TMP_Text offlineActivateButtonText;
        public Button offlineActivateButton;
        public TMP_Text autoDisableOfflineTimeButtonText;
        public Button autoDisableOfflineTimeButton;

        private float maxTime => (float)UpgradableStats[StatType.MaxTime].CachedValue;
        public float MaxForwardTime => maxTime + (float)UpgradableStats[StatType.MaxForwardsTime].CachedValue;
        public float MaxBackwardTime => -(maxTime + (float)UpgradableStats[StatType.MaxBackwardsTime].CachedValue);
        public Dictionary<StatType, UpgradableStat> UpgradableStats = new();

        public bool DevSpeedLocal => DevSpeed;


        private void OnEnable()
        {
            OverlordEvents.UpdateTimescale += SetTimeScale;
            offlineActivateButton.onClick.AddListener(UpdateActivateButton);
            autoDisableOfflineTimeButton.onClick.AddListener(UpdateAutoDisableOfflineTimeButton);
            EventHandler.AwayFor += AwayFor;
            Register();
        }

        private void OnDisable()
        {
            OverlordEvents.UpdateTimescale -= SetTimeScale;
            offlineActivateButton.onClick.RemoveListener(UpdateActivateButton);
            autoDisableOfflineTimeButton.onClick.RemoveListener(UpdateAutoDisableOfflineTimeButton);
            EventHandler.AwayFor -= AwayFor;
            Unregister();
        }

        private void Start()
        {
            UpdateActivateButtonText();
            UpdateAutoDisableOfflineTimeButtonText();
        }

        private void AwayFor(float time)
        {
            OfflineTime += time;
            UpdateActivateButtonText();
        }

        private void UpdateActivateButton()
        {
            OfflineTimeActive = !OfflineTimeActive;
            UpdateActivateButtonText();
        }

        private void UpdateActivateButtonText()
        {
            offlineActivateButton.interactable = OfflineTime > 0;
            offlineActivateButtonText.text = OfflineTimeActive ? "Active" : "Inactive";
        }

        private void UpdateAutoDisableOfflineTimeButton()
        {
            OfflineTimeAutoDisable = !OfflineTimeAutoDisable;
            UpdateAutoDisableOfflineTimeButtonText();
        }

        private void UpdateAutoDisableOfflineTimeButtonText()
        {
            autoDisableOfflineTimeButtonText.text = OfflineTimeAutoDisable ? "On" : "Off";
        }

        public void IncrementCurrentTime(float time)
        {
            CurrentTime += time;
            CurrentTime = Mathf.Clamp(CurrentTime, MaxBackwardTime, MaxForwardTime);
            SetTimeline();
        }

        private void Update()
        {
            var previousTimeScale = TimeScale;
            var goingForwards = LayerTab == Tab.FoundationOfProduction || LayerTab == Tab.EnginesOfExpansion ||
                                LayerTab == Tab.ChronicleArchives;
            var goingBackwards = LayerTab == Tab.RealmOfResearch || LayerTab == Tab.CollapseOfTime ||
                                 LayerTab == Tab.TemporalRifts;

            var isTimeMaxed = (CurrentTime < MaxForwardTime - 0.1 && !goingBackwards) ||
                              (CurrentTime > MaxBackwardTime + 0.1 && !goingForwards);
            var isOfflineTimeActive = OfflineTimeActive && isTimeMaxed && OfflineTime > 0;
            TimeScale = _tempTimeScale * (isOfflineTimeActive ? (float)OfflineTimeScaleMultiplier : 1) *
                        (isTimeMaxed ? 1 : 0);

            CurrentTime += Time.deltaTime * TimeScale;
            if (isOfflineTimeActive && !IAPManager.InfiniteOTPurchased)
            {
                OfflineTime -= Time.deltaTime * Math.Abs(TimeScale);
                if (OfflineTime <= 0)
                {
                    OfflineTime = 0;
                    OfflineTimeActive = false;
                    UpdateActivateButtonText();
                }
            }

            CurrentTime = Mathf.Clamp(CurrentTime, MaxBackwardTime, MaxForwardTime);
            SetTimeline();
            var timeLineMaxedAndNotPaused = isTimeMaxed && !(goingBackwards || goingForwards);
            timeScaleText.text = TimeScale == 0
                ? timeLineMaxedAndNotPaused ? "Paused" : $"{ColourRed}<b>Void Lull</b>"
                : $"Time Scale | {ColourGreen}{TimeScale:N1}";
            timeText.text = (TimeScale == 0
                ? timeLineMaxedAndNotPaused ? $"{ColourOrange}" : $"{ColourRed}"
                : $"{ColourGreen}") + CalcUtils.FormatTime(CurrentTime, absolutevalue: false);
            if (!Mathf.Approximately(previousTimeScale, TimeScale))
            {
                if (LayerTab == Tab.TemporalRifts) TemporalRiftEvents.OnUpdateTimeRemaining();
                EventHandler.UpdateTextsForTimeScale();
            }
        }

        private void SetTimeline()
        {
            var sliderValue = 0.5f;

            if (CurrentTime < 0)
                // Map from MaxBackwardTime (slider = 0) to 0 (slider = 0.5)
                sliderValue = Mathf.Lerp(0f, 0.5f, (CurrentTime - MaxBackwardTime) / (0f - MaxBackwardTime));
            else
                // Map from 0 (slider = 0.5) to MaxForwardTime (slider = 1)
                sliderValue = Mathf.Lerp(0.5f, 1f, CurrentTime / MaxForwardTime);

            timeLine.value = sliderValue;
            SetTimelineRange();
        }

        private void SetTimelineRange()
        {
            maxBackwardTimeText.text =
                $"Max Backward Time | {ColourGreen}{CalcUtils.FormatTime(MaxBackwardTime, shortForm: true)}";
            maxForwardTimeText.text =
                $"Max Forward Time | {ColourGreen}{CalcUtils.FormatTime(MaxForwardTime, shortForm: true)}";
        }

        private float _tempTimeScale;


        private float ZeroTimescale => 0;
        private float FoundationOfProductionTimescale => (float)GetStat(StatType.TimeScale).CachedValue;
        private float EnginesOfExpansionTimescale => (float)GetStat(StatType.TimeScale).CachedValue;
        private float ChronicleArchivesTimescale => (float)GetStat(StatType.TimeScale).CachedValue;

        private float RealmOfResearchTimescale => -
            (float)GetStat(StatType.TimeScale).CachedValue; // Negative time scale for research;

        private float CollapseOfTimescale => -
            (float)GetStat(StatType.TimeScale).CachedValue; // Negative time scale for research;;

        private float TemporalRiftsTimescale => -
            (float)GetStat(StatType.TimeScale).CachedValue; // Negative time scale for research;;;

        public void SetTimeScale()
        {
            switch (LayerTab)
            {
                case Tab.Zero:
                    _tempTimeScale = ZeroTimescale * (DevSpeedLocal ? 10 : 1);
                    break;
                //positive
                case Tab.FoundationOfProduction:
                    _tempTimeScale = FoundationOfProductionTimescale * (DevSpeedLocal ? 10 : 1);
                    break;
                case Tab.EnginesOfExpansion:
                    _tempTimeScale = EnginesOfExpansionTimescale * (DevSpeedLocal ? 10 : 1);
                    break;
                case Tab.ChronicleArchives:
                    _tempTimeScale = ChronicleArchivesTimescale * (DevSpeedLocal ? 10 : 1);
                    break;
                //negative
                case Tab.RealmOfResearch:
                    _tempTimeScale = RealmOfResearchTimescale * (DevSpeedLocal ? 10 : 1);
                    break;
                case Tab.CollapseOfTime:
                    _tempTimeScale = CollapseOfTimescale * (DevSpeedLocal ? 10 : 1);
                    break;
                case Tab.TemporalRifts:
                    _tempTimeScale = TemporalRiftsTimescale * (DevSpeedLocal ? 10 : 1);
                    break;
                default:
                    Debug.Log("NoTab");
                    break;
            }

            EventHandler.UpdateTextsForTimeScale();

            offlineTimeText.text =
                $"<b>Time Gathered</b> | {(IAPManager.InfiniteOTPurchased ? "\u221e" : $"{ColourGreen}{CalcUtils.FormatTime(OfflineTime, shortForm: false)}")}";
        }


        #region Singleton class

        public static TimeManager timeManager;

        private void Awake()
        {
            if (timeManager == null)
                timeManager = this;
            //DontDestroyOnLoad(gameObject);
            else
                Destroy(gameObject);
        }

        #endregion

        [SerializeField] private List<string> tags = new() { "Timeline" };

        public List<string> GetTags()
        {
            return tags;
        }

        public UpgradableStat GetStat(StatType statType)
        {
            return UpgradableStats.GetValueOrDefault(statType);
        }

        public void Register()
        {
            UpgradeManager.Instance?.RegisterEntity(this);
        }

        public void Unregister()
        {
            UpgradeManager.Instance?.UnregisterEntity(this);
        }
    }
}