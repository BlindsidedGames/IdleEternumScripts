using System;

namespace ChronicleArchivesNamespace.IdleDysonSwarm
{
    public static class IdsEvents
    {
        public static event Action UpdateUI;
        public static event Action Infinity;

        public static void OnUpdateUI()
        {
            UpdateUI?.Invoke();
        }

        public static void OnInfinity()
        {
            Infinity?.Invoke();
        }
    }
}