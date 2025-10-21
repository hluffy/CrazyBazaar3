using UnityEngine;
using UnityEditor;

public class MeshSimplifier : EditorWindow
{
    private Mesh highPolyMesh;
    private float simplificationRatio = 0.5f;
    
    [MenuItem("Tools/Mesh Simplifier")]
    public static void ShowWindow()
    {
        GetWindow<MeshSimplifier>("Mesh Simplifier");
    }
    
    void OnGUI()
    {
        highPolyMesh = (Mesh)EditorGUILayout.ObjectField(
            "High Poly Mesh", highPolyMesh, typeof(Mesh), false);
            
        simplificationRatio = EditorGUILayout.Slider(
            "Simplification Ratio", simplificationRatio, 0.01f, 1f);
            
        if (GUILayout.Button("Generate Low Poly") && highPolyMesh != null)
        {
            GenerateLowPolyMesh();
        }
    }
    
    void GenerateLowPolyMesh()
    {
        Mesh lowPolyMesh = Instantiate(highPolyMesh);
        
        // 简化顶点
        Vector3[] vertices = lowPolyMesh.vertices;
        int[] triangles = lowPolyMesh.triangles;
        
        // 根据比率减少顶点数
        int targetVertexCount = Mathf.FloorToInt(vertices.Length * simplificationRatio);
        int targetTriangleCount = Mathf.FloorToInt(triangles.Length * simplificationRatio);
        
        // 重新构建网格（简化版）
        // 实际应用中需要更复杂的算法
        
        // 保存新网格
        string path = EditorUtility.SaveFilePanelInProject(
            "Save Low Poly Mesh", "LowPoly_" + highPolyMesh.name, "asset", "");
            
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(lowPolyMesh, path);
            AssetDatabase.SaveAssets();
        }
    }
}
