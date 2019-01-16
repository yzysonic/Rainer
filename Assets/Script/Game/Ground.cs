using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;
using Tree = RainerLib.Tree;

[RequireComponent(typeof(GrassField), typeof(PaintGrass))]
public class Ground : Singleton<Ground> {

    [SerializeField] private Texture2D grassTex;
    [SerializeField, Range(0.1f, 10.0f)] private float tilingScale = 1.0f;
    [SerializeField, Range(0.001f, 0.01f)] private float redLineWidth = 0.003f;

    private new Renderer renderer;
    private Dictionary<Int2, Tree> treeMap = new Dictionary<Int2, Tree>();
    private RaycastHit hitInfo;

    public GrassField GrassField { get; private set; }
    public PaintGrass PaintGrass { get; private set; }
    public MoveRange MoveRange { get; private set; }
    public int LayerMask { get; private set; }
    public Material Material { get; private set; }
    public float LengthToUV { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        renderer = transform.Find("Plane").GetComponent<Renderer>();
        GrassField = GetComponent<GrassField>();
        PaintGrass = GetComponent<PaintGrass>();
        MoveRange = GetComponentInChildren<MoveRange>();
        LayerMask = UnityEngine.LayerMask.GetMask(UnityEngine.LayerMask.LayerToName(gameObject.layer));
        Material = renderer.material;
        LengthToUV = CastToGetLengthInUV(1.0f);
    }

    // Use this for initialization
    void Start ()
    {
        Material.shader = Shader.Find("Custom/Ground");
        Material.SetTexture("_GrassTex", grassTex);
        Material.SetTexture("_GrassField", GrassField.Texture);
        Material.SetFloat("_GrassTilingScale", tilingScale);
        if (MoveRange != null)
        {
            Material.SetFloat("_RangeRadius", GetMoveRangeRadiusInUV());
            Material.SetFloat("_RangeLineWidth", redLineWidth);
        }
        else
        {
            Material.SetFloat("_RangeRadius", 0.0f);
            Material.SetFloat("_RangeLineWidth", 0.0f);
        }
        StartCoroutine(InitGrass());
    }

    IEnumerator InitGrass()
    {
        yield return new WaitForEndOfFrame();
        PaintGrass.Init();
        Material.SetTexture("_GrassMask", PaintGrass.RenderTex);
    }

    public void GrowGrass(Vector2 uv, int playerNo)
    {
        GrassField.SetGrass(uv, playerNo);
        PaintGrass.Paint(uv);
    }

    public void GrowGrass(Vector2 uv, int playerNo, float radius)
    {
        GrassField.SetGrass(uv, playerNo, radius);
        PaintGrass.Paint(uv, radius);
    }

    /// <summary>
    /// 指定された座標から直下にレイキャストしてUVを取得する。レイが当たらなかった場合(0,0)を返す。
    /// </summary>
    /// <param name="pos">レイを発射するワールド座標</param>
    /// <param name="heightOffset">レイを発射する高さを調整</param>
    /// <returns>グランド空間でのUV座標</returns>
    public Vector2 GetUV(Vector3 rayPos, float heightOffset = 0.0f)
    {
        rayPos.y += heightOffset;

        if (Physics.Raycast(rayPos, Vector3.down, out hitInfo, Mathf.Infinity, LayerMask))
        {
            return hitInfo.textureCoord;
        }

        return Vector2.zero;
    }

    public void ResetGrass()
    {
        GrassField.Awake();
        PaintGrass.Init();
        Material.SetTexture("_GrassMask", PaintGrass.RenderTex);
    }

    public void SetGrassMask(Texture texture)
    {
        Material.SetTexture("_GrassMask", texture);
    }

    public float CastToGetLengthInUV(float length)
    {
        var rayPos = transform.position + transform.rotation * new Vector3(length, 1.0f, 0.0f);
        return Mathf.Abs(GetUV(rayPos).x - 0.5f);
    }

    private float GetMoveRangeRadiusInUV()
    {
        return CastToGetLengthInUV(MoveRange.radius);
    }

}
