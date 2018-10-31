using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("MakeTexture"))
        {
            MakeTexture();
        }
    }

    void MakeTexture()
    {
        var test = target as Test;
        var tex = test.tex;
        var texArray = new Texture2DArray(tex.width, tex.height, 9, tex.format, tex.mipmapCount>0);
        for (var i = 0; i < texArray.depth; i++)
            Graphics.CopyTexture(tex, 0, texArray, i);
        AssetDatabase.CreateAsset(texArray, "Assets/Resources/Textures/texture_array.asset");
        Debug.Log(AssetDatabase.GetAssetPath(texArray));

    }
}

public class CreateMaterialExample : MonoBehaviour
{
    [MenuItem("Test/Create Material")]
    static void CreateMaterial()
    {
        // Create a simple material asset

        Material material = new Material(Shader.Find("Specular"));
        AssetDatabase.CreateAsset(material, "Assets/MyMaterial.mat");

        // Print the path of the created asset
        Debug.Log(AssetDatabase.GetAssetPath(material));
    }
}