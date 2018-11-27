using UnityEngine;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour {

    public Transform parent;
    public bool alignWithY;

    private void Awake()
    {
        if(parent == null)
        {
            parent = transform;
        }
    }

    private void OnWillRenderObject()
    {
        if (alignWithY)
        {
            parent.rotation = Quaternion.Euler(0.0f, Camera.current.transform.rotation.eulerAngles.y, 0.0f);
        }
        else
        {
            parent.rotation = Camera.current.transform.rotation;
        }
    }

}
