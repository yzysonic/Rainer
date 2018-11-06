using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grassland : MonoBehaviour {

    public GameObject Grass;
    private GameObject obj1,obj2;


    public void CreateGrass(Vector3 centerPos)
    {

        for (int i = 0; i < 1; i++)
        {
            float x = centerPos.x + (Random.Range(0f, 1.0f) - 0.5f);
            float z = centerPos.z + (Random.Range(0f, 1.0f) - 0.5f);


            obj1 = Instantiate(Grass);
            obj2 = Instantiate(Grass);

            obj1.AddComponent<GrassGrow>();
            obj2.AddComponent<GrassGrow>();

            obj2.transform.Rotate(0, 90, 0);

            obj1.transform.position = new Vector3(x, -1.0f, z);
            obj2.transform.position = new Vector3(x, -1.0f, z);

            obj1.transform.SetParent(transform);
            obj2.transform.SetParent(transform);
        }
    }

}
