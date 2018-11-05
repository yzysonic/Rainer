using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

    private new Renderer renderer;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
        InitRenderTexture();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void InitRenderTexture()
    {
        var mat = renderer.material;
        var tex = renderer.material.mainTexture;
        var renderTex = new RenderTexture(tex.width, tex.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        renderTex.volumeDepth = (int)mat.mainTextureScale.x * (int)mat.mainTextureScale.y;
        renderTex.dimension = UnityEngine.Rendering.TextureDimension.Tex2DArray;
        renderTex.useMipMap = true;
        renderTex.wrapMode = TextureWrapMode.Repeat;

        var defaultMat = new Material(Shader.Find("Hidden/BlitCopy"));
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
}
