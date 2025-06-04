using System;
using Blindsided.SaveData;
using EnginesOfExpansionNamespace.Engines;
using UnityEngine;
using static Blindsided.SaveData.StaticReferences;

namespace EnginesOfExpansionNamespace
{
    public class EnginesOfExpansionProductionManager : MonoBehaviour
    {
        public TimeCore timeCore;
        public ChronotonDrill chronotonDrill;
        public EnergyAmplifier energyAmplifier;
        public TemporalAccelerator temporalAccelerator;
        public ResonanceChamber resonanceChamber;

        private void Update()
        {
            if (LayerTab == SaveData.Tab.EnginesOfExpansion)
                if (TimeScale != 0)
                {
                    var speed = Math.Abs(TimeScale) * Time.deltaTime;
                    timeCore.Produce(speed);
                    chronotonDrill.Produce(speed);
                    energyAmplifier.Produce(speed);
                    temporalAccelerator.Produce(Time.deltaTime);
                    resonanceChamber.Produce(speed);
                }

            timeCore.UpdateUI();
            chronotonDrill.UpdateUI();
            energyAmplifier.UpdateUI();
            temporalAccelerator.UpdateUI();
            resonanceChamber.UpdateUI();
        }
    }
}