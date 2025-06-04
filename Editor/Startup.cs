using UnityEditor;

namespace Editor
{
    [InitializeOnLoad]
    public class StartUp
    {

        #if UNITY_ANDROID && UNITY_EDITOR

        static StartUp()
        {

            PlayerSettings.Android.keystorePass = "Believeinme1020";
            PlayerSettings.Android.keyaliasPass = "Believeinme1020";
        }

        #endif
    }
}