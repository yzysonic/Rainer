using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ground : MonoBehaviour {

    public int tipResolution = 1024;
    public int tipDivision = 8;
    public float blushSize = 4.0f;
    public Material paintMat;
    public Texture2D testTex;

    private new Renderer renderer;
    private RenderTexture renderTex;
    private Material defaultMat;
    private int slicePropertyID;
    private int centerUVPropertyID;

	// Use this for initialization
	void Start () {
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
        renderTex = new RenderTexture(tipResolution, tipResolution, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        renderTex.volumeDepth = tipDivision * tipDivision;
        renderTex.dimension = UnityEngine.Rendering.TextureDimension.Tex2DArray;
        renderTex.useMipMap = true;
        renderTex.wrapMode = TextureWrapMode.Repeat;

        defaultMat = new Material(Shader.Find("Hidden/BlitCopy"));
        RenderTexture.active = renderTex;
        for (var i = 0; i < renderTex.volumeDepth; i++)
        {
            Graphics.SetRenderTarget(renderTex, 0, CubemapFace.Unknown, i);
            Graphics.Blit(tex, defaultMat);
        }

        mat.mainTexture = null;
        mat.shader = Shader.Find("Custom/Ground");
        mat.mainTexture = renderTex;
    }

    private void InitPaintMaterial()
    {
        paintMat.SetFloat("_BlushSize", blushSize);
        slicePropertyID = Shader.PropertyToID("_Slice");
        centerUVPropertyID = Shader.PropertyToID("_CenterUV");
    }


    public void PaintGrass(Vector2 uv)
    {
        int x, y;
        float blushSizeNormalized = blushSize / (tipResolution * tipDivision);
        x = Mathf.FloorToInt((uv.x-blushSizeNormalized) * (tipDivision - 1.0f / tipResolution));
        y = Mathf.FloorToInt((uv.y-blushSizeNormalized) * (tipDivision - 1.0f / tipResolution));
        RenderTip(x, y, uv);
        //RenderTip(x+1, y  , uv);
        //RenderTip(x  , y+1, uv);
        //RenderTip(x+1, y+1, uv);
    }

    private void RenderTip(int x, int y, Vector2 uv)
    {
        uv.x -= (float)x / tipDivision;
        uv.y -= (float)y / tipDivision;

        int slice = y * tipDivision + x;
        paintMat.SetInt(slicePropertyID, slice);
        paintMat.SetVector(centerUVPropertyID, new Vector4(uv.x, uv.y));

        //var tempTex = RenderTexture.GetTemporary(1024, 1024, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);

        //Graphics.Blit(renderTex, tempTex, paintMat);
        Graphics.SetRenderTarget(renderTex, 0, CubemapFace.Unknown, slice);
        Graphics.Blit(testTex, defaultMat);

        //RenderTexture.ReleaseTemporary(tempTex);
    }
}
