using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRange : MonoBehaviour {

    public float radius;

    void OnDrawGizmosSelected()
    {
        //ギズモの色を変更
        Gizmos.color = new Color32(145, 244, 139, 210);
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
