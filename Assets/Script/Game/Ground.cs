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

    public GrassField GrassField { get; private set; }
    public PaintGrass PaintGrass { get; private set; }

    // Use this for initialization
    void Start () {
        renderer = transform.Find("Plane").GetComponent<Renderer>();
        GrassField = GetComponent<GrassField>();
        PaintGrass = GetComponent<PaintGrass>();

        var mat = renderer.material;
        mat.shader = Shader.Find("Custom/Ground");
        mat.SetTexture("_GrassMask", PaintGrass.RenderTex);
        mat.SetTexture("_GrassTex", grassTex);
        mat.SetFloat("_RangeRadius", GetMoveRangeRadiusInUV());

    }

    public void GrowGrass(Vector2 uv, int playerNo)
    {
        GrassField.SetGrass(uv, playerNo);
        PaintGrass.Paint(uv);
    }

    public Tree GetTree(Vector2 uv, int playerNo)
    {
        var pos = GrassField.UVToBlockPos(uv);

        if(GrassField.GetPlayerNoOfGrass(pos) != playerNo)
        {
            return null;
        }

        Tree tree;
        if(treeMap.TryGetValue(pos, out tree))
        {
            return tree;
        }

        tree = Instantiate(treePrefab, GrassField.BlockPosToWorldPos(pos), Quaternion.identity, transform).GetComponent<Tree>();
        treeMap.Add(pos, tree);
        return tree;

    }

    private float GetMoveRangeRadiusInUV()
    {
        var radius = GetComponentInChildren<MoveRange>().radius;
        var rayPos = transform.position + transform.rotation * new Vector3(radius, 1.0f, 0.0f);
        RaycastHit hitInfo;

        if (Physics.Raycast(rayPos, Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            return Mathf.Abs(hitInfo.textureCoord.x - 0.5f);
        }

        return 0.0f;

    }

}
