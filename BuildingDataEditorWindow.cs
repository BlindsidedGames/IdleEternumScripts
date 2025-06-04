#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using FoundationOfProgressNameSpace.BuildingScriptables;
using RealmOfResearchNamespace.ResearchScriptables;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

public class DataEditorWindow : OdinMenuEditorWindow
{
    [MenuItem("Tools/Data Editor")]
    private static void OpenWindow()
    {
        var window = GetWindow<DataEditorWindow>("Data Editor");
        window.ForceMenuTreeRebuild();
        window.Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        // ------------------------------------------------------------
        // Load and sort all BuildingData
        // ------------------------------------------------------------
        var buildingGuids = AssetDatabase.FindAssets("t:BuildingData");
        var buildingDataList = new List<BuildingData>();
        foreach (var guid in buildingGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<BuildingData>(path);
            if (asset != null) buildingDataList.Add(asset);
        }

        buildingDataList = buildingDataList.OrderBy(b => b.sortOrder).ToList();

        foreach (var buildingData in buildingDataList) tree.Add($"Layer 1/{buildingData.buildingName}", buildingData);

        // ------------------------------------------------------------
        // Load and sort all ResearchData
        // ------------------------------------------------------------
        var researchGuids = AssetDatabase.FindAssets("t:ResearchData");
        var researchDataList = new List<ResearchData>();
        foreach (var guid in researchGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<ResearchData>(path);
            if (asset != null) researchDataList.Add(asset);
        }

        researchDataList = researchDataList.OrderBy(r => r.sortOrder).ToList();

        foreach (var researchData in researchDataList)
            tree.Add($"Layer -1/{researchData.researcherName}", researchData);

        return tree;
    }
}
#endif