using UnityEngine;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour {

    public Transform parent;

    private void Awake()
    {
        if(parent == null)
        {
            parent = transform;
        }
    }

    private void OnWillRenderObject()
    {
        parent.rotation = Camera.current.transform.rotation;
    }

}
