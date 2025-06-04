using UnityEngine;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;

namespace FoundationOfProgressNameSpace.Prestige
{
    public class BuildingActivator : MonoBehaviour
    {
        public GameObject ascendancyPanel;

        public GameObject prestigePanel;

        /*public GameObject StarCradle;
        public GameObject NebulaGenerator;
        public GameObject CelestialFoundry;
        public GameObject NovaSpire;
        public GameObject GalacticNexus;*/
        public GameObject singularityLoom;
        public GameObject eternityBeacon;
        public GameObject infinityCrucible;

        private void Start()
        {
            if (Ascended)
            {
                ActivateBuildings();
                return;
            }

            InvokeRepeating(nameof(ActivateBuildings), 0, 0.1f);
        }

        private void ActivateBuildings()
        {
            ascendancyPanel.SetActive(Ascended || AdditionalBuildings);
            prestigePanel.SetActive(Prestiged || StarCradles > 1e7);

            singularityLoom.SetActive(AdditionalBuildings);
            eternityBeacon.SetActive(AdditionalBuildings);
            infinityCrucible.SetActive(AdditionalBuildings);
        }
    }
}