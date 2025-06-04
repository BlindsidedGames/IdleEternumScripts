using System;
using Blindsided.SaveData;
using UnityEngine;
using UnityEngine.UI;
using static Blindsided.SaveData.StaticReferences;

namespace TemporalRiftNamespace
{
    public class TemporalRiftsProductionManager : MonoBehaviour
    {
        public EssenceSynthesis EssenceSynthesis;
        public Button TemporalRiftsButton;

        private void Start()
        {
            TemporalRiftsButton.onClick.AddListener(TemporalRiftEvents.OnUpdateTimeRemaining);
        }

        private void Update()
        {
            if (LayerTab == SaveData.Tab.TemporalRifts)
            {
                if (TimeScale != 0)
                {
                    var speed = Math.Abs(TimeScale) * Time.deltaTime;
                    EssenceSynthesis.UpdateTimers(speed);
                }

                EssenceSynthesis.UpdateAllVisuals();
            }
        }


        public static TemporalRiftsProductionManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}