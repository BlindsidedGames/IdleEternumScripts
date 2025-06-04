using System;

namespace FoundationOfProgressNameSpace
{
    public static class FoundationOfProductionEvents
    {
        public static event Action PrestigeOne;
        public static event Action PrestigeTwo;
        public static event Action UpdateUI;
        public static event Action<float> Produce;

        public static event Action AutomatedBuildingsHideShow;

        public static void OnProduce(float elapsedTime)
        {
            Produce?.Invoke(elapsedTime);
            UpdateUI?.Invoke();
        }

        public static void OnUpdateUI()
        {
            UpdateUI?.Invoke();
        }

        public static void OnPrestigeTwo()
        {
            PrestigeTwo?.Invoke();
            UpdateUI?.Invoke();
        }

        public static void OnPrestigeOne()
        {
            PrestigeOne?.Invoke();
            UpdateUI?.Invoke();
        }

        public static void OnAutomatedBuildingsHideShow()
        {
            AutomatedBuildingsHideShow?.Invoke();
        }
    }
}