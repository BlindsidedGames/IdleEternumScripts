using System;
using System.Collections.Generic;
using Blindsided.SaveData;
using UnityEngine;
using UpgradeSystem;
using static Blindsided.SaveData.StaticReferences;
using static CollapseOfTimeNamespace.CollapseOfTimeStaticReferences;

namespace CollapseOfTimeNamespace
{
    public class CollapseOfTimeProductionManager : MonoBehaviour
    {
        private readonly List<CotUpgradeReferences> _cotUpgradeReferencesListToProcess = new();
        public EntropyNexus entropyNexus;

        private void Update()
        {
            activeResearches = _cotUpgradeReferencesListToProcess.Count;
            if (LayerTab == SaveData.Tab.CollapseOfTime)
            {
                if (TimeScale != 0)
                {
                    var speed = Math.Abs(TimeScale / activeResearches *
                                         entropyNexus.ResearchBuff) * Time.deltaTime;
                    foreach (var cotUpgradeReferences in _cotUpgradeReferencesListToProcess)
                        cotUpgradeReferences.IncrementTimeInvested(speed);
                    entropyNexus.Produce(Math.Abs(TimeScale * Time.deltaTime));
                }

                entropyNexus.SetTexts();
            }
        }


        public void AddCotUpgradeReference(CotUpgradeReferences cotUpgradeReferences)
        {
            _cotUpgradeReferencesListToProcess.Add(cotUpgradeReferences);
        }

        public void RemoveCotUpgradeReference(CotUpgradeReferences cotUpgradeReferences)
        {
            _cotUpgradeReferencesListToProcess.Remove(cotUpgradeReferences);
        }

        public static CollapseOfTimeProductionManager Instance;

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