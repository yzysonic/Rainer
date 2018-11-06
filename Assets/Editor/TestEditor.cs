using UnityEngine;
using UnityEditor;
using System.Collections;



[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Test"))
        {
            Test();
        }

        if(GUILayout.Button("SetRenderTex"))
        {
            MakeRenderTex();
        }
    }

    void Test()
    {
        var test = target as Test;
        Debug.Log(test.Material.GetVector("_MainTex_ST")[0]);
    }

    void MakeRenderTex()
    {
        var test = target as Test;
        var tex = test.Material.mainTexture;
        var st = test.Material.GetVector("_MainTex_ST");
        var renderTex = new RenderTexture(tex.width, tex.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        renderTex.dimension = UnityEngine.Rendering.TextureDimension.Tex2DArray;
        renderTex.volumeDepth = (int)st.x * (int)st.y;
        renderTex.useMipMap = true;
        renderTex.wrapMode = TextureWrapMode.Repeat;
        renderTex.Create();

        for (var i = 0; i < renderTex.volumeDepth; i++)
        {
            var f = i / (float)renderTex.volumeDepth;
            test.gradationMaterial.SetFloat("_Factor", f);
            Graphics.SetRenderTarget(renderTex, 0, CubemapFace.Unknown, i);
            Graphics.Blit(tex, test.gradationMaterial);
        }

        //test.gradationMaterial.SetFloat("_Factor", 1.0f);
        //Graphics.SetRenderTarget(renderTex, 0, CubemapFace.Unknown, 0);
        //Graphics.SetRenderTarget(renderTex, 0, CubemapFace.Unknown, 2);
        //Graphics.Blit(tex, test.gradationMaterial);

        test.Material.mainTexture = null;
        test.meshRenderer.material.shader = Shader.Find("Custom/Ground");
        test.Material.mainTexture = renderTex;

        //var texArray = new Texture2DArray(tex.width, tex.height, test.depth, TextureFormat.ARGB32, tex.mipmapCount > 0);

        //for(var i = 0; i < test.depth; i++)
        //{
        //    Graphics.CopyTexture(renderTex, i, texArray, i);
        //}

        ////RenderTexture.active = null;

        //AssetDatabase.CreateAsset(texArray, "Assets/Resources/Textures/texture_array.asset");
        //Debug.Log(AssetDatabase.GetAssetPath(texArray));

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