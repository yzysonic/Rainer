using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawn : MonoBehaviour
{

    public GameObject grass;

    [Range(0.0f, 10.0f), Tooltip("草が出るまでの時間")]
    public static float spawn_delay;

    private bool[][] spawn_flag;
    private static DynamicPaintObject paint_object;
    private Queue<Vector2> spawn_queue;
	// Use this for initialization
	void Start () {
        //spawn_flag = new bool[11][];
        //for(int i=0;i<11;i++)
        //{
        //    spawn_flag[i] = new bool[11];
        //    for(int j=0;j<11;j++)
        //    {
        //        spawn_flag[i][j] = false;
        //    }
        //}

        //var grass_group = new GameObject();
        //grass_group.name = "GrassGroup";
        //grass_group.transform.SetParent(transform);

        //for (int i=0;i<101;i++)
        //{
        //    for (int j = 0; j < 101; j++)
        //    {
        //        Instantiate(grass, new Vector3((i-50) * 0.8f, 0.26f, (j-50) * 0.8f), Quaternion.identity).transform.SetParent(grass_group.transform);
        //    }
        //}

        paint_object = GetComponent<DynamicPaintObject>();
        spawn_queue = new Queue<Vector2>(60);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		if(spawn_queue.Count > 30)
        {
            paint_object.Paint(spawn_queue.Dequeue());
        }
	}

    public void Spawn(RaycastHit hitInfo)
    {
        if (hitInfo.collider != null && hitInfo.collider.gameObject == gameObject)
        {
            paint_object.Paint(hitInfo.textureCoord);
            //spawn_queue.Enqueue(hitInfo.textureCoord);
        }
    }
}