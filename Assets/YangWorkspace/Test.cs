using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Test : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Material gradationMaterial;
    public Material Material
    {
        get
        {
            return meshRenderer.material;
        }

    }

    private void Start()
    {
    }

}