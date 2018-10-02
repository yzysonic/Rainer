using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawn : MonoBehaviour {

    public GameObject grass;
    private bool[][] spawn_flag;

	// Use this for initialization
	void Start () {
        spawn_flag = new bool[11][];
        for(int i=0;i<11;i++)
        {
            spawn_flag[i] = new bool[11];
            for(int j=0;j<11;j++)
            {
                spawn_flag[i][j] = false;
            }
        }

        var grass_group = new GameObject();
        grass_group.name = "GrassGroup";
        grass_group.transform.SetParent(transform);

        for (int i=0;i<101;i++)
        {
            for (int j = 0; j < 101; j++)
            {
                Instantiate(grass, new Vector3((i-50) * 0.8f, 0.26f, (j-50) * 0.8f), Quaternion.identity).transform.SetParent(grass_group.transform);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "RainArea")
            return;

        //var pos = collision.contacts[0].point;
        //var x = (int)(pos.x / 10) + 5;
        //var y = (int)(pos.y / 10) + 5;

        //if (!spawn_flag[y][x])
        //{
        //    Instantiate(grass, pos, Quaternion.identity);
        //    spawn_flag[y][x] = true;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "RainArea")
            return;

        //var pos = other..contacts[0].point;
        //var x = (int)(pos.x / 10) + 5;
        //var y = (int)(pos.y / 10) + 5;

        //if (!spawn_flag[y][x])
        //{
        //    Instantiate(grass, pos, Quaternion.identity);
        //    spawn_flag[y][x] = true;
        //}

    }

}
