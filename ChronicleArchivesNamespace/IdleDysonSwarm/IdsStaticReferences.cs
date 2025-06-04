using System;
using System.Collections.Generic;
using Blindsided.SaveData.ChronicleMinigames;
using UnityEngine;
using static Blindsided.Oracle;

namespace ChronicleArchivesNamespace.IdleDysonSwarm
{
    public static class IdsStaticReferences
    {
        public static IdleDysonSwarmData OnInfinityDataToReset
        {
            get => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData;
            set => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData = value;
        }


        public static Dictionary<string, IdleDysonSwarmBuildingSaveData> AllBuildingsSaveData =>
            oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData.Buildings;

        public static int InfinityPoints
        {
            get => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.InfinityPoints;
            set => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.InfinityPoints = value;
        }

        public static double RequiredBotsForInfinity => 4.2E19 / InfinityDivisor;
        public static double InfinityDivisor = 1;

        public static double Cash
        {
            get => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData.Cash;
            set => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData.Cash =
                Math.Max(0, value);
        }

        public static double Science
        {
            get => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData.Science;
            set => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData.Science =
                Math.Max(0, value);
        }


        public static double Bots
        {
            get => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData.Bots;
            set => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData.Bots =
                Math.Max(0, value);
        }

        public static bool WorkerFoldoutPreference
        {
            get => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.WorkerFoldout;
            set => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.WorkerFoldout = value;
        }

        public static bool ScienceFoldoutPreference
        {
            get => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.ScienceFoldout;
            set => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.ScienceFoldout = value;
        }

        public static float BotDistribution
        {
            get => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.BotDistribution;
            set => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.BotDistribution =
                Mathf.Clamp(value, 0, 1);
        }

        public static double PanelsDecayed
        {
            get => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData.PanelsDecayed;
            set => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData.PanelsDecayed =
                value;
        }

        public static float TinkerProgress
        {
            get => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData.TinkerProgress;
            set => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData.TinkerProgress =
                value;
        }

        public static bool TinkerActive
        {
            get => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData.TinkerActive;
            set => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.IdleDysonSwarmData.TinkerActive = value;
        }

        public static float CurrentRuntime
        {
            get => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.CurrentRuntime;
            set => oracle.saveData.ChronicleArchivesSaveDataData.IdleDysonSwarm.CurrentRuntime = value;
        }
    }
}