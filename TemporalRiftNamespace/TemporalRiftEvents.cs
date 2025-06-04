using System;

namespace TemporalRiftNamespace
{
    public static class TemporalRiftEvents
    {
        public static event Action HexSelected;
        public static event Action UpdateTimeRemaining;

        public static void OnHexSelected()
        {
            HexSelected?.Invoke();
        }

        public static void OnUpdateTimeRemaining()
        {
            UpdateTimeRemaining?.Invoke();
        }
    }
}