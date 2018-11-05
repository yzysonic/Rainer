using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ground : MonoBehaviour {

    public int tipResolution = 1024;
    public int tipDivision = 8;
    public float blushSize = 4.0f;
    public Material paintMat;
    public Texture2D grassTex;
    public int sliceTest = 0;

    private new Renderer renderer;
    private RenderTexture renderTex;
    private Material defaultMat;
    private int slicePropertyID;
    private int centerUVPropertyID;
    private float blushSizeNormalized;

    // Use this for initialization
    void Start () {
        blushSizeNormalized = blushSize / (tipResolution * tipDivision);
        renderer = GetComponent<Renderer>();
        InitRenderTexture();
        InitPaintMaterial();
    }


    // Update is called once per frame
    void Update () {
		
	}

    private void InitRenderTexture()
    {
        var mat = renderer.material;
        var tex = renderer.material.mainTexture;

        //mat.mainTextureScale = Vector2.one * tipDivision;

        renderTex = new RenderTexture(tipResolution, tipResolution, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        renderTex.volumeDepth = tipDivision * tipDivision;
        renderTex.dimension = UnityEngine.Rendering.TextureDimension.Tex2DArray;
        renderTex.useMipMap = true;
        renderTex.wrapMode = TextureWrapMode.Repeat;
        renderTex.filterMode = FilterMode.Point;

        defaultMat = new Material(Shader.Find("Hidden/BlitCopy"));
        //RenderTexture.active = renderTex;
        //for (var i = 0; i < renderTex.volumeDepth; i++)
        //{
        //    Graphics.SetRenderTarget(renderTex, 0, CubemapFace.Unknown, i);
        //    Graphics.Blit(tex, defaultMat);
        //}

        //mat.mainTexture = null;
        mat.shader = Shader.Find("Custom/Ground");

        //// UVToIndex    = (Resolution-1) / (Resolution/Division) 
        ////              = (TipResolution*Division-1) * (Division/(TipResolution*Division)) 
        ////              = Division-1/TipResolution
        ////mat.SetFloat("_UVToIndex", (1.0f - 1.0f / (tipResolution * tipDivision)));
        //mat.SetFloat("_UVToIndex", 1);
        ////mat.SetFloat("_UVToTipUV", (tipResolution * tipDivision - 1.0f) / (tipResolution) / tipDivision);
        //mat.SetFloat("_UVToTipUV", 1);

        mat.SetFloat("_Division", tipDivision);
        mat.SetTexture("_GrassMask", renderTex);
        mat.SetTexture("_GrassTex", grassTex);
        //mat.mainTexture = renderTex;
    }

    private void InitPaintMaterial()
    {
        paintMat.SetFloat("_BlushSize", blushSizeNormalized*tipDivision);
        slicePropertyID = Shader.PropertyToID("_Slice");
        centerUVPropertyID = Shader.PropertyToID("_CenterUV");
    }


    public void PaintGrass(Vector2 uv)
    {
        int x, y;
        x = Mathf.FloorToInt((uv.x-blushSizeNormalized) * (tipDivision - 1.0f / tipResolution));
        y = Mathf.FloorToInt((uv.y-blushSizeNormalized) * (tipDivision - 1.0f / tipResolution));
        RenderTip(x, y, uv);
        //RenderTip(x + 1, y, uv);
        //RenderTip(x, y + 1, uv);
        //RenderTip(x + 1, y + 1, uv);
    }

    private void RenderTip(int x, int y, Vector2 uv)
    {
        uv.x = uv.x * tipDivision - x;
        uv.y = uv.y * tipDivision - y;

        int slice = y * tipDivision + x;
        paintMat.SetInt(slicePropertyID, slice);
        paintMat.SetVector(centerUVPropertyID, new Vector4(uv.x, uv.y));
        paintMat.SetTexture("_MainTex", renderTex);

        var tempTex = RenderTexture.GetTemporary(tipResolution, tipResolution, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);

        Graphics.Blit(renderTex, tempTex, paintMat);
        Graphics.SetRenderTarget(renderTex, 0, CubemapFace.Unknown, slice);
        Graphics.Blit(tempTex, defaultMat);

        RenderTexture.ReleaseTemporary(tempTex);
    }
}
