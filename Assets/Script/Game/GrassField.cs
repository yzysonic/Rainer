using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RainerLib;

public class GrassField : MonoBehaviour {

    public float fieldSize      = 300;
    public int blockDivision    = 128;
    public float grassRadius    = 4.35f;
    public int grassDensity     = 10;
    public int grassScore       = 1;
    public List<Color> playerColors;
    public float switchSpeed    = 1.0f;

    private byte [][]grassMap;
    private int grassRadiusInBlock;
    private int grassSqrRadiusInBlock;
    private ScoreManager scoreManager;
    private Terrain terrain;
    private int[,] detailValue;
    private int opacityPropertyID;
    private Timer timer;

    public Texture2D Texture { get; private set; }
    public float Opacity
    {
        get
        {
            return Ground.Instance.Material.GetFloat(opacityPropertyID);
        }
        set
        {
            Ground.Instance.Material.SetFloat(opacityPropertyID, Mathf.Clamp01(value));
        }
    }


    // Use this for initialization
    public void Awake ()
    {
        scoreManager = ScoreManager.IsCreated ? ScoreManager.Instance : null;
        timer = new Timer();

        // マップの初期化
        grassMap = new byte[blockDivision][];
        for(var i=0;i<blockDivision;i++)
        {
            grassMap[i] = new byte[blockDivision];
        }

        // 半径の計算
        var radius = grassRadius / fieldSize * blockDivision;
        grassRadiusInBlock = Mathf.RoundToInt(radius);
        grassSqrRadiusInBlock = Mathf.RoundToInt(radius * radius);

        // Terrain初期化
        terrain = GetComponentInChildren<Terrain>();
        terrain.terrainData.SetDetailResolution(blockDivision, 16);
        detailValue = new int[1, 1] { { grassDensity } };
        CleanDetailMap();

        // 表示用テクスチャーの初期化
        Texture = new Texture2D(blockDivision, blockDivision);
        Texture.SetPixels(Enumerable.Repeat(Color.clear, blockDivision * blockDivision).ToArray());
        opacityPropertyID = Shader.PropertyToID("_GrassFieldOpaticy");
    }

    private void Start()
    {
        ApplyTexture();
    }

    private void Update()
    {
        Opacity = 0.5f + 0.5f * Mathf.Sin(timer++ * Mathf.PI * switchSpeed);
    }

    void OnDestroy()
    {
        CleanDetailMap();
    }

    private void CleanDetailMap()
    {
        var detailMap = new int[terrain.terrainData.detailWidth, terrain.terrainData.detailHeight];
        for (var i = 0; i < 4; i++)
        {
            terrain.terrainData.SetDetailLayer(0, 0, i, detailMap);
        }
    }

    public void SetGrass(Vector2 uv, int playerNo)
    {
        var posBlock = UVToBlockPos(uv);

        for (int x = posBlock.x - grassRadiusInBlock; x <= posBlock.x + grassRadiusInBlock; x++)
        {
            for (int z = posBlock.y - grassRadiusInBlock; z <= posBlock.y + grassRadiusInBlock; z++)
            {

                if (x < 0 || x >= blockDivision || z < 0 || z >= blockDivision)
                {
                    continue;
                }

                if (grassMap[x][z] > 0)
                {
                    continue;
                }

                var sqrLength = (x - posBlock.x) * (x - posBlock.x) + (z - posBlock.y) * (z - posBlock.y);

                if (sqrLength > grassSqrRadiusInBlock)
                {
                    continue;
                }

                terrain.terrainData.SetDetailLayer(x, z, playerNo, detailValue);
                grassMap[x][z] = (byte)(playerNo + 1);
                scoreManager?.AddScore(playerNo, grassScore);
            }
        }

    }

    public int GetPlayerNoOfGrass(Vector2 uv)
    {
        return GetPlayerNoOfGrass(UVToBlockPos(uv));
    }

    public int GetPlayerNoOfGrass(Int2 blockPos)
    {
        return GetPlayerNoOfGrass(blockPos.x, blockPos.y);
    }

    public int GetPlayerNoOfGrass(int x, int y)
    {
        return grassMap[x][y] - 1;
    }

    public Int2 UVToBlockPos(Vector2 uv)
    {
        return new Int2((Vector2.one - uv) * blockDivision);
    }

    public Vector3 BlockPosToWorldPos(Int2 pos)
    {
        return (new Vector3(pos.x, 0.0f, pos.y) / blockDivision - new Vector3(0.5f, 0.0f, 0.5f)) * fieldSize + transform.position;
    }

    public void ApplyTexture()
    {
        for(var x = 0; x<blockDivision; x++)
        {
            for(var y= 0; y<blockDivision; y++)
            {
                var no = grassMap[x][y]-1;
                if (no >= 0)
                {
                    Texture.SetPixel(x, y, playerColors[no]);
                }
            }
        }
        Texture.Apply();
    }
}
