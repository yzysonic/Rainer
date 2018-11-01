using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grassland : MonoBehaviour {

    public GameObject Grass;
    //public Vector3 centerPos;    //原点
    public float radius = 1;     //半径
    public float angle = 0;      //分割角度  
    private GameObject obj1,obj2;
    public Camera targetCamera;
    public bool checkGrass;



    void Start()
    {
        //checkGrass = false;
        //CreateGrass();
    }

    void Update()
    {
        //Vector3 p = Camera.main.transform.position;
        //p.y = transform.position.y;
        //transform.LookAt(p);

        //transform.LookAt(transform.position + targetCamera.transform.rotation * Vector3.back, targetCamera.transform.rotation * Vector3.down);
        //obj1.transform.rotation = transform.rotation;
    }

    public void CreateGrass(Vector3 centerPos)
    {
        //centerPos = transform.position;


        //for (int i = 0; i < 240; angle += 6, radius = (float)Random.Range(0.3f,1.8f), i++)
        //{
        //    float x = centerPos.x + radius * Mathf.Cos(angle * 3.14f / 180f);
        //    float z = centerPos.z + radius * Mathf.Sin(angle * 3.14f / 180f);

        //    obj1 = GameObject.Instantiate(Grass);
        //    obj2 = GameObject.Instantiate(Grass);

        //    obj2.transform.Rotate(0, 90, 0);

        //    obj1.transform.position = new Vector3(x, centerPos.y, z);
        //    obj2.transform.position = new Vector3(x, centerPos.y, z);

        //}

        //obj1 = GameObject.Instantiate(Grass, centerPos, Quaternion.identity);

        for (int i = 0; i < 1; i++)
        {
            float x = centerPos.x + (Random.Range(0f, 1.0f) - 0.5f);
            float z = centerPos.z + (Random.Range(0f, 1.0f) - 0.5f);


            obj1 = GameObject.Instantiate(Grass);
            obj2 = GameObject.Instantiate(Grass);

            obj1.AddComponent<GrassGrow>();
            obj2.AddComponent<GrassGrow>();

            obj2.transform.Rotate(0, 90, 0);

            obj1.transform.position = new Vector3(x, -1.0f, z);
            obj2.transform.position = new Vector3(x, -1.0f, z);
        }
    }

}
