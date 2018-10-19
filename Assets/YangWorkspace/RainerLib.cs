using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance != null)
                return instance;

            var name = typeof(T).ToString();
            var gameObject = GameObject.Find(name);

            if (gameObject != null)
                return instance = gameObject.GetComponent<T>();

            return instance = new GameObject(name).AddComponent<T>();
        }
    }

}
