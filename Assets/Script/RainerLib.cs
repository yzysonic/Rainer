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
            if (instance == null)
            {
                var gameObject = new GameObject(typeof(T).ToString());
                instance = gameObject.AddComponent<T>();
            }

            return instance;
        }
    }

}
