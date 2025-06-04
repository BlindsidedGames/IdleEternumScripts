#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class BuildNumberUtility : EditorWindow
{
    [MenuItem("Tools/Build Number Utility")]
    public static void ShowWindow()
    {
        GetWindow<BuildNumberUtility>("Build Number Utility");
    }

    private void OnGUI()
    {
        GUILayout.Label("Build Number Utility", EditorStyles.boldLabel);

        // Increment Button
        if (GUILayout.Button("Increment Build Number", GUILayout.Height(40))) ChangeBuildNumber(1);

        // Decrement Button
        if (GUILayout.Button("Decrement Build Number", GUILayout.Height(40))) ChangeBuildNumber(-1);
    }

    private void ChangeBuildNumber(int change)
    {
        // Get current build numbers
        var androidBuildNumber = PlayerSettings.Android.bundleVersionCode;
        var iosBuildNumber = int.Parse(PlayerSettings.iOS.buildNumber);

        // Apply changes, ensuring the values don't go below 1
        androidBuildNumber = Mathf.Max(1, androidBuildNumber + change);
        iosBuildNumber = Mathf.Max(1, iosBuildNumber + change);

        // Update PlayerSettings
        PlayerSettings.Android.bundleVersionCode = androidBuildNumber;
        PlayerSettings.iOS.buildNumber = iosBuildNumber.ToString();

        // Force Unity to recognize the change
        var settingsAsset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0];
        EditorUtility.SetDirty(settingsAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Debug logs
        Debug.Log($"Android Build Number Updated: {androidBuildNumber - change} → {androidBuildNumber}");
        Debug.Log($"iOS Build Number Updated: {iosBuildNumber - change} → {iosBuildNumber}");
    }
}
#endif