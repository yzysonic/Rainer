using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Range(1.0f, 30.0f)]
    public float max_speed = 10.0f;
    [Range(0.0f, 10.0f)]
    public float rotation_speed_scale = 1.0f;
    [Range(0.0f, 0.1f)]
    public float dead_zone = 0.08f;
    [Range(10.0f, 90.0f)]
    public float max_angle = 40.0f;

    private Joycon joycon;
    private CharacterController controller;

    // Use this for initialization
    void Start () {

        //var joycons = JoyconManager.Instance.j;

        //if (joycons != null && joycons.Count > 0)
        //    joycon = joycons[0];

        //controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.Translate(0.0f, 0.0f, 0.5f);
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    transform.Translate(0.0f, 0.0f, -0.5f);
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.Translate(-0.5f, 0.0f, 0.0f);
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    transform.Translate(0.5f, 0.0f, 0.0f);
        //}

        #region Rotate

        //var stick = joycon.GetStick();
        //transform.Rotate(0.0f, stick[0] * rotation_speed_scale, 0.0f);

        #endregion


        #region Move

        //// Joyconの向きのベクトルを計算
        //var raw_vector = Quaternion.Euler(90.0f, 0.0f, 0.0f) * joycon.GetVector() * Vector3.forward;

        //// 移動方向に適用
        //var dir = new Vector3(raw_vector.x, 0.0f, raw_vector.z);

        //// 最大角度を制限
        //var max_value = Mathf.Sin(max_angle * Mathf.Deg2Rad);
        //dir = Vector3.ClampMagnitude(dir, max_value) / max_value;

        //// 回転の適用
        //dir = transform.rotation * dir;

        //// DeadZoneのチェック
        //if (dir.sqrMagnitude >= dead_zone * dead_zone)
        //    // 移動する
        //    controller.SimpleMove(dir * max_speed);

        #endregion


    }
}
