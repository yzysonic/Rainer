using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGrass : MonoBehaviour {

    [SerializeField] private int grassMaskResolution = 1024;
    [SerializeField] private float grassBlushSize = 15.0f;
    [SerializeField] private Material paintMat;

    private int centerUVPropertyID;
    private float blushSizeNormalized;

    public RenderTexture RenderTex { get; private set; }


    // Use this for initialization
    void Awake () {
        blushSizeNormalized = grassBlushSize / grassMaskResolution;
        InitRenderTexture();
        InitPaintMaterial();
    }

    private void InitRenderTexture()
    {
        RenderTex = new RenderTexture(grassMaskResolution, grassMaskResolution, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        RenderTex.useMipMap = true;
        //renderTex.filterMode = FilterMode.Point;
    }

    private void InitPaintMaterial()
    {
        paintMat.SetTexture("_MainTex", RenderTex);
        paintMat.SetFloat("_BlushSize", blushSizeNormalized);
        centerUVPropertyID = Shader.PropertyToID("_CenterUV");
    }


    public void Paint(Vector2 uv)
    {
        paintMat.SetVector(centerUVPropertyID, new Vector4(uv.x, uv.y));

        var tempTex = RenderTexture.GetTemporary(grassMaskResolution, grassMaskResolution, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);

        Graphics.Blit(RenderTex, tempTex, paintMat);
        Graphics.Blit(tempTex, RenderTex);

        RenderTexture.ReleaseTemporary(tempTex);
    }


}
