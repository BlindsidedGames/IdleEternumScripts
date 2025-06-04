using System;

namespace RealmOfResearchNamespace
{
    public class RealmOfResearchEvents
    {
        public static event Action UpdateUI;
        public static event Action SetMaxedResearches;

        public static void OnUpdateUI()
        {
            UpdateUI?.Invoke();
        }

        public static void OnSetMaxedResearches()
        {
            SetMaxedResearches?.Invoke();
        }
    }
}