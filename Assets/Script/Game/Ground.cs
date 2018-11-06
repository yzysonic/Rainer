﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ground : MonoBehaviour {

    public int tipResolution = 1024;
    public int tipDivision = 8;
    public float blushSize = 4.0f;
    public Material paintMat;
    public Texture2D grassTex;

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

    private void InitRenderTexture()
    {
        var mat = renderer.material;
        var tex = renderer.material.mainTexture;

        renderTex = new RenderTexture(tipResolution, tipResolution, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        renderTex.volumeDepth = tipDivision * tipDivision;
        renderTex.dimension = UnityEngine.Rendering.TextureDimension.Tex2DArray;
        renderTex.useMipMap = true;
        renderTex.wrapMode = TextureWrapMode.Repeat;
        //renderTex.filterMode = FilterMode.Point;

        defaultMat = new Material(Shader.Find("Hidden/BlitCopy"));

        mat.shader = Shader.Find("Custom/Ground");
        mat.SetFloat("_Division", tipDivision);
        mat.SetTexture("_GrassMask", renderTex);
        mat.SetTexture("_GrassTex", grassTex);
        mat.SetFloat("_RangeRadius", GetMoveRangeRadiusInUV());
    }

    private void InitPaintMaterial()
    {
        paintMat.SetTexture("_MainTex", renderTex);
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

        var tempTex = RenderTexture.GetTemporary(tipResolution, tipResolution, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);

        Graphics.Blit(renderTex, tempTex, paintMat);
        Graphics.SetRenderTarget(renderTex, 0, CubemapFace.Unknown, slice);
        Graphics.Blit(tempTex, defaultMat);

        RenderTexture.ReleaseTemporary(tempTex);
    }

    private float GetMoveRangeRadiusInUV()
    {
        var radius = GetComponentInChildren<MoveRange>().radius;
        var rayPos = transform.position + transform.rotation * new Vector3(radius, 1.0f, 0.0f);
        RaycastHit hitInfo;

        if (Physics.Raycast(rayPos, Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            return Mathf.Abs(hitInfo.textureCoord.x -0.5f);
        }

        return 0.0f;

    }
}