using System;
using Blindsided.SaveData;
using UnityEngine;
using static Blindsided.SaveData.StaticReferences;

namespace RealmOfResearchNamespace
{
    public class RealmOfResearchProductionManager : MonoBehaviour
    {
        public Researcher VoidScribe;
        public Researcher FractureLoom;
        public Researcher DarkArchitect;
        public Researcher CollapseEngine;
        public Researcher SingularityVault;
        public FopConsumptionManager FopConsumptionManager;

        private void Update()
        {
            if (LayerTab != SaveData.Tab.RealmOfResearch || TimeScale == 0) return;
            var speed = Math.Abs(TimeScale) * Time.deltaTime;
            FopConsumptionManager.Drain(speed);
            SingularityVault.Produce(speed);
            CollapseEngine.Produce(speed);
            DarkArchitect.Produce(speed);
            FractureLoom.Produce(speed);
            VoidScribe.Produce(speed);
            RealmOfResearchEvents.OnUpdateUI();
        }
    }
}