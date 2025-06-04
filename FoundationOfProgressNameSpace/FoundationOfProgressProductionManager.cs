using System;
using Blindsided.SaveData;
using UnityEngine;
using static Blindsided.SaveData.StaticReferences;

namespace FoundationOfProgressNameSpace
{
    public class FoundationOfProgressProductionManager : MonoBehaviour
    {
        public Building StarCradle;
        public Building NebulaGenerator;
        public Building CelestialFoundry;
        public Building NovaSpire;
        public Building GalacticNexus;
        public Building SingularityLoom;
        public Building EternityBeacon;
        public Building InfinityCrucible;

        private void Update()
        {
            if (LayerTab == SaveData.Tab.FoundationOfProduction)
            {
                if (TimeScale != 0)
                {
                    var speed = Math.Abs(TimeScale) * Time.deltaTime;

                    InfinityCrucible.Produce(speed);
                    EternityBeacon.Produce(speed);
                    SingularityLoom.Produce(speed);
                    GalacticNexus.Produce(speed);
                    NovaSpire.Produce(speed);
                    CelestialFoundry.Produce(speed);
                    NebulaGenerator.Produce(speed);
                    StarCradle.Produce(speed);
                }

                FoundationOfProductionEvents.OnUpdateUI();
            }
        }
    }
}