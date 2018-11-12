using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GrassField : MonoBehaviour {

    public float fieldSize      = 300;
    public int blockDivision    = 128;
    public float grassRadius    = 4.35f;
    public int grassDensity     = 10;
    public int grassScore       = 1;

    private byte [][]grassMap;
    private int grassRadiusInBlock;
    private int grassSqrRadiusInBlock;
    private ScoreManager scoreManager;
    private Terrain terrain;
    private int[,] detailValue;


    // Use this for initialization
    void Start ()
    {

        grassMap = new byte[blockDivision][];
        for(var i=0;i<blockDivision;i++)
        {
            grassMap[i] = new byte[blockDivision];
        }

        var radius = grassRadius / fieldSize * blockDivision;
        grassRadiusInBlock = Mathf.RoundToInt(radius);
        grassSqrRadiusInBlock = Mathf.RoundToInt(radius * radius);

        scoreManager = ScoreManager.Instance;
        terrain = GetComponentInChildren<Terrain>();

        ClearDetailMap();

        detailValue = new int[1, 1] { { grassDensity } };
    }


    public void SetGrass(Vector2 uv, int playerNo)
    {
        var posBlock = (Vector2.one-uv) * blockDivision;
        var posBlockX = (int)posBlock.x;
        var posBlockZ = (int)posBlock.y;

        for (int a = 0; a < grassRadiusInBlock * 2+1; a++)
        {

            for (int b = 0; b < grassRadiusInBlock * 2+1; b++)
            {

                int bx = posBlockX - grassRadiusInBlock + a;
                int bz = posBlockZ - grassRadiusInBlock + b;

                if (!(bx >= 0 && bx < blockDivision && bz >= 0 && bz < blockDivision))
                {
                    continue;
                }

                if (grassMap[bx][bz] > 0)
                {
                    continue;
                }

                var sqrLength = (bx - posBlockX) * (bx - posBlockX) + (bz - posBlockZ) * (bz - posBlockZ);

                if (sqrLength > grassSqrRadiusInBlock)
                {
                    continue;
                }

                var grassPosW = (new Vector3(bx, 0.0f, bz) / blockDivision - Vector3.one * 0.5f) * fieldSize;
                grassPosW.y = 0.338f;

                terrain.terrainData.SetDetailLayer(bx, bz, playerNo, detailValue);
                grassMap[bx][bz] = (byte)(playerNo + 1);
                scoreManager.AddScore(playerNo, grassScore);
            }
        }

    }

    private void ClearDetailMap()
    {
        var detailMap = new int[terrain.terrainData.detailWidth, terrain.terrainData.detailHeight];
        for(var i=0;i<4;i++)
        {
            terrain.terrainData.SetDetailLayer(0, 0, i, detailMap);
        }
    }

    private void OnDestroy()
    {
        ClearDetailMap();
    }

}
