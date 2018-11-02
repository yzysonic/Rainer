using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("MakeTextureArray"))
        {
            MakeTextureArray();
        }

        if(GUILayout.Button("SetRenderTex"))
        {
            MakeRenderTex();
        }
    }

    void MakeTextureArray()
    {
        var test = target as Test;
        var tex = test.tex;
        var texArray = new Texture2DArray(tex.width, tex.height, test.depth, tex.format, tex.mipmapCount > 0);
        var renderTex = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
        renderTex.useMipMap = true;
        try
        {
            for (var i = 0; i < texArray.depth; i++)
            {
                test.gradationMaterial.SetFloat("_Factor", (texArray.depth - i) / texArray.depth);
                Graphics.SetRenderTarget(renderTex, 0, CubemapFace.Unknown, i);
                Graphics.Blit(tex, test.gradationMaterial);
                
                //Graphics.CopyTexture(renderTex, 0, texArray, i);
            }
        }
        catch(System.Exception e)
        {
            RenderTexture.ReleaseTemporary(renderTex);
            return;
        }
        RenderTexture.ReleaseTemporary(renderTex);

        AssetDatabase.CreateAsset(texArray, "Assets/Resources/Textures/texture_array.asset");
        Debug.Log(AssetDatabase.GetAssetPath(texArray));
    }

    void MakeRenderTex()
    {
        var test = target as Test;
        var tex = test.tex;
        var renderTex = new RenderTexture(tex.width, tex.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
        renderTex.dimension = UnityEngine.Rendering.TextureDimension.Tex2DArray;
        renderTex.volumeDepth = test.depth;
        renderTex.useMipMap = true;
        //renderTex.wrapMode = TextureWrapMode.Repeat;
        renderTex.Create();

        for (var i = 0; i < renderTex.volumeDepth; i++)
        {
            var f = i / (float)renderTex.volumeDepth;
            Debug.Log(f);
            test.gradationMaterial.SetFloat("_Factor", f);
            Graphics.SetRenderTarget(renderTex, 0, CubemapFace.Unknown, i);
            Graphics.Blit(tex, test.gradationMaterial);
        }

        //test.gradationMaterial.SetFloat("_Factor", 1.0f);
        //Graphics.SetRenderTarget(renderTex, 0, CubemapFace.Unknown, 0);
        //Graphics.SetRenderTarget(renderTex, 0, CubemapFace.Unknown, 2);
        //Graphics.Blit(tex, test.gradationMaterial);

        test.Material.mainTexture = renderTex;
        //test.Material.SetTexture("_MainTex", renderTex);

        //RenderTexture.active = null;

        //AssetDatabase.CreateAsset(renderTex, "Assets/Resources/Textures/texture_array.asset");
        //Debug.Log(AssetDatabase.GetAssetPath(renderTex));

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