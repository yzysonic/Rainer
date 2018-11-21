using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;
using Tree = RainerLib.Tree;

[RequireComponent(typeof(GrassField), typeof(PaintGrass))]
public class Ground : Singleton<Ground> {

    [SerializeField] private Texture2D grassTex;
    [SerializeField] private GameObject treePrefab;

    private new Renderer renderer;
    private Dictionary<Int2, Tree> treeMap = new Dictionary<Int2, Tree>();
    private RaycastHit hitInfo;

    public GrassField GrassField { get; private set; }
    public PaintGrass PaintGrass { get; private set; }
    public MoveRange MoveRange { get; private set; }
    public int LayerMask { get; private set; }

    protected override void Awake()
    {
        renderer = transform.Find("Plane").GetComponent<Renderer>();
        GrassField = GetComponent<GrassField>();
        PaintGrass = GetComponent<PaintGrass>();
        MoveRange = GetComponentInChildren<MoveRange>();
        LayerMask = UnityEngine.LayerMask.GetMask(UnityEngine.LayerMask.LayerToName(gameObject.layer));

    }

    // Use this for initialization
    void Start () {

        var mat = renderer.material;
        mat.shader = Shader.Find("Custom/Ground");
        mat.SetTexture("_GrassMask", PaintGrass.RenderTex);
        mat.SetTexture("_GrassTex", grassTex);
        if (MoveRange != null)
        {
            mat.SetFloat("_RangeRadius", GetMoveRangeRadiusInUV());
        }
        else
        {
            mat.SetFloat("_RangeRadius", 0.0f);
            mat.SetFloat("RangeLineWidth", 0.0f);
        }


    }

    public void GrowGrass(Vector2 uv, int playerNo)
    {
        GrassField.SetGrass(uv, playerNo);
        PaintGrass.Paint(uv);
    }

    public Tree GetTree(Vector2 uv, int playerNo)
    {
        var pos = GrassField.UVToBlockPos(uv);

        // 既存の木を返す
        if(treeMap.ContainsKey(pos))
        {
            return treeMap[pos];
        }

        // 新しい木を返す
        var tree = Instantiate(treePrefab, GrassField.BlockPosToWorldPos(pos), Quaternion.identity, transform).GetComponent<Tree>();
        treeMap.Add(pos, tree);
        return tree;

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

    private float GetMoveRangeRadiusInUV()
    {
        var rayPos = transform.position + transform.rotation * new Vector3(MoveRange.radius, 1.0f, 0.0f);

        return Mathf.Abs(GetUV(rayPos).x - 0.5f);

    }

}
