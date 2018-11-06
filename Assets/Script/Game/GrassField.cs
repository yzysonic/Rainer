using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassField : MonoBehaviour {

    public float FieldX, FieldZ;
    private float bFieldSizeX, bFieldSizeZ;
    public Vector3 [][]bPos;
    public bool [][]bChk;
    public Vector3 fieldCenterPos, centerPos;
    public grassland grassLand;
    public Transform player;


    // Use this for initialization
    void Start () {

        centerPos = new Vector3(0, 0, 0);

        bPos = new Vector3[100][];
        for(int i = 0; i < 100; i++)
        {
            bPos[i] = new Vector3[100];
        }
        bChk = new bool[100][];
        for(int i = 0; i < 100; i++)
        {
            bChk[i] = new bool[100];
        }

        FieldX = 100;
        FieldZ = 100;

        bFieldSizeX = FieldX / 100;
        bFieldSizeZ = FieldZ / 100;

        SetBlockPos();
    }
	
	// Update is called once per frame
	void Update () {

        //if(Input.GetKeyDown(KeyCode.W))
        //{
        //    centerPos.z++;
        //}
        //else if(Input.GetKeyDown(KeyCode.S))
        //{
        //    centerPos.z--;
        //}

        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    centerPos.x++;
        //}
        //else if(Input.GetKeyDown(KeyCode.D))
        //{
        //    centerPos.x--;
        //}

        centerPos = player.position;

        SetCheckBlock(centerPos, 5);

    }

    public void SetBlockPos()
    {
        fieldCenterPos = new Vector3(-50, 0, -50);
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {


                float x = fieldCenterPos.x + (bFieldSizeX / 2 + i * bFieldSizeX);
                float z = fieldCenterPos.z + (bFieldSizeZ / 2 + j * bFieldSizeZ);

                bPos[i][j] = new Vector3(x, fieldCenterPos.y, z);
                bChk[i][j] = false;

            }
        }
    }

    public void SetCheckBlock(Vector3 pos, float Radius)
    {
        int blockLength = Mathf.RoundToInt(Radius);

        for (int a = 0; a < blockLength * 2; a++)
        {
            for (int b = 0; b < blockLength * 2; b++)
            {

                float bx = pos.x - blockLength + (a);
                float bz = pos.z - blockLength + (b);
                var grassPosW = new Vector3(bx, -1.0f, bz);
                var grassPosB = grassPosW + new Vector3(50.0f, -1.0f, 50.0f);


                if (!(grassPosB.x >= 0 && grassPosB.x < 100 && grassPosB.z >= 0 && grassPosB.z < 100))
                    continue;

                if (bChk[(int)grassPosB.x][(int)grassPosB.z] == true)
                    continue;

                if ((grassPosW - pos).sqrMagnitude >= (Radius * Radius))
                    continue;

                grassLand.CreateGrass(grassPosW);
                bChk[(int)grassPosB.x][(int)grassPosB.z] = true;
                

            }
        }
    }


}
