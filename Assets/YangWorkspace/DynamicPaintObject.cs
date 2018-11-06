// 参考：http://esprog.hatenablog.com/entry/2016/05/08/212355 (Splatoonの塗りみたいのを再現したい その５)

using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Material))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class DynamicPaintObject : MonoBehaviour
{
    #region SerializedProperties

    [SerializeField, Tooltip("テクスチャペイント用マテリアル")]
    private Material paintMaterial = null;

    [SerializeField, Tooltip("ブラシ用テクスチャ")]
    private Texture2D blushTexture = null;

    [SerializeField, Range(0, 10), Tooltip("ブラシの大きさ")]
    private float blushSize = 4.0f;

    #endregion SerializedProperties

    #region ShaderPropertyID

    private int mainTexturePropertyID;
    private int paintUVPropertyID;
    private int blushTexturePropertyID;
    private int blushScalePropertyID;

    #endregion ShaderPropertyID

    private RenderTexture paintTexture;


    #region UnityEventMethod

    public void Awake()
    {
        InitPropertyID();

        var meshRenderer = GetComponent<MeshRenderer>();
        var material = meshRenderer.material;
        var mainTexture = material.mainTexture;

        //Textureがもともとついてないオブジェクトには非対応
        if (mainTexture == null)
        {
            Debug.LogWarning("[DynamicPaintObject] : テクスチャの設定されていないオブジェクトに適用することはできません");
            Destroy(this);
            return;
        }
        else
        {

            //DynamicPaint用RenderTextureの生成
            paintTexture = new RenderTexture(mainTexture.width, mainTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
            paintTexture.wrapMode = TextureWrapMode.Repeat;
            //メインテクスチャのコピー
            Graphics.Blit(mainTexture, paintTexture);
            //マテリアルのテクスチャをRenderTextureに変更
            material.mainTexture = paintTexture;

            //シェーダーの設定
            paintMaterial.SetTexture(blushTexturePropertyID, blushTexture);
            paintMaterial.SetFloat(blushScalePropertyID, blushSize / 100);
        }
    }

    #endregion UnityEventMethod

    private void InitPropertyID()
    {
        paintUVPropertyID = Shader.PropertyToID("_PaintUV");
        blushTexturePropertyID = Shader.PropertyToID("_Blush");
        blushScalePropertyID = Shader.PropertyToID("_BlushScale");
    }

    public void Paint(Vector2 uv)
    {
        RenderTexture buf = RenderTexture.GetTemporary(paintTexture.width, paintTexture.height);

        //メインテクスチャへのペイント
        paintMaterial.SetVector(paintUVPropertyID, uv);
        Graphics.Blit(paintTexture, buf, paintMaterial);
        Graphics.Blit(buf, paintTexture);

        RenderTexture.ReleaseTemporary(buf);
    }

}