using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Test : MonoBehaviour
{
    public Texture2D tex;

    private void Start()
    {
        //var mat = GetComponent<MeshRenderer>().material;
        //var paintTexture = Enumerable.Repeat(new RenderTexture(tex.width, tex.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default), 9).ToList();
        //paintTexture.ForEach(pt => Graphics.Blit(tex, pt));
        //mat.texture
        //var renderTex = RenderTexture.GetTemporary(128, 128,);
        renderTex.dimension = UnityEngine.Rendering.TextureDimension.Tex2DArray;
        renderTex.volumeDepth = 9;
        //Graphics.SetRenderTarget(renderTex, 0, CubemapFace.Unknown, )
    }
}