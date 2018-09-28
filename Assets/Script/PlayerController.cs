using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    public float speed = 7.0F;

    private CharacterController controller;

    void Start()
    {

        // コンポーネントの取得
        controller = GetComponent<CharacterController>();

    }

    void FixedUpdate()
    {

        Vector3 move = speed * (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"));

        controller.SimpleMove(move);

    }

}