using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PaintGrass : MonoBehaviour {

    [SerializeField] private int grassMaskResolution = 1024;
    [SerializeField] private float grassBlushSize = 4.0f;
    [SerializeField] private float fadeOutRange = 1.0f;
    [SerializeField] private Material paintMat;

    private int centerUVPropertyID;
    private int blushSizePropertyID;
    private int fadeOutRadiusPropertyID;
    private float lengthToUV;
    private float blushSizeNormalized;
    private float fadeOutRadiusNormalized;

    public RenderTexture RenderTex { get; private set; }

    public void Start()
    {
        lengthToUV = Ground.Instance.LengthToUV;
        blushSizeNormalized = grassBlushSize * lengthToUV;
        fadeOutRadiusNormalized = (grassBlushSize-fadeOutRange) * lengthToUV;
        InitRenderTexture();
        InitPaintMaterial();
    }

    public void InitRenderTexture()
    {
        RenderTex = new RenderTexture(grassMaskResolution, grassMaskResolution, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        RenderTex.useMipMap = true;
        var clear = new Texture2D(grassMaskResolution, grassMaskResolution);
        clear.SetPixels(Enumerable.Repeat(Color.clear, grassMaskResolution * grassMaskResolution).ToArray());
        clear.Apply();
        Graphics.Blit(clear, RenderTex);
    }

    public void InitPaintMaterial()
    {
        centerUVPropertyID = Shader.PropertyToID("_CenterUV");
        blushSizePropertyID = Shader.PropertyToID("_BlushSize");
        fadeOutRadiusPropertyID = Shader.PropertyToID("_FadeOutRadius");
        paintMat.SetTexture("_MainTex", RenderTex);
        paintMat.SetFloat(blushSizePropertyID, blushSizeNormalized);
        paintMat.SetFloat(fadeOutRadiusPropertyID, fadeOutRadiusNormalized);
        Ground.Instance.Material.SetTexture("_GrassMask", RenderTex);
    }


    public void Paint(Vector2 uv)
    {
        paintMat.SetVector(centerUVPropertyID, new Vector4(uv.x, uv.y));

        var tempTex = RenderTexture.GetTemporary(grassMaskResolution, grassMaskResolution, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);

        Graphics.Blit(RenderTex, tempTex, paintMat);
        Graphics.Blit(tempTex, RenderTex);

        RenderTexture.ReleaseTemporary(tempTex);
    }

    public void Paint(Vector2 uv, float radius)
    {
        paintMat.SetVector(centerUVPropertyID, new Vector4(uv.x, uv.y));
        paintMat.SetFloat(blushSizePropertyID, radius * lengthToUV);
        paintMat.SetFloat(fadeOutRadiusPropertyID, (radius-fadeOutRange) * lengthToUV);

        var tempTex = RenderTexture.GetTemporary(grassMaskResolution, grassMaskResolution, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);

        Graphics.Blit(RenderTex, tempTex, paintMat);
        Graphics.Blit(tempTex, RenderTex);

        RenderTexture.ReleaseTemporary(tempTex);

        paintMat.SetFloat(blushSizePropertyID, blushSizeNormalized);
        paintMat.SetFloat(fadeOutRadiusPropertyID, fadeOutRadiusNormalized);
    }


}
