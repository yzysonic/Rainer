using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPlayerControl : Rainer {

    public PlayerIcon playerIcon;
    private Vector3 resetPosition;

    [Range(1.0f, 30.0f)]
    public float max_speed = 10.0f;
    [Range(10.0f, 90.0f)]
    public float max_angle = 40.0f;
    [Range(0.0f, 0.1f)]
    public float dead_zone = 0.08f;

    private Joycon joycon;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        resetPosition = transform.position;
        Model.gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Model.gameObject.activeSelf != playerIcon.IsJoin)
        {
            if (playerIcon.IsJoin)
            {
                Model.gameObject.SetActive(true);
                CharacterController.enabled = true;
                transform.position = resetPosition;
                joycon = playerIcon.Joycon;
            }
            else
            {
                CharacterController.SimpleMove(Vector3.zero);
                CharacterController.Move(Vector3.up);
                if (transform.position.y > resetPosition.y)
                {
                    Model.rotation = Quaternion.identity;
                    Model.gameObject.SetActive(false);
                }
            }
        }

        if (Model.gameObject.activeSelf)
        {
            var vector = Vector3.zero;

            if(joycon != null)
            {
                // Joyconの向きのベクトルを計算
                var raw_vector = joycon.GetAccel();

                // 移動方向に適用
                vector = new Vector3(-raw_vector.y, 0.0f, -raw_vector.z);

                // 最大角度を制限
                var max_value = Mathf.Sin(max_angle * Mathf.Deg2Rad);
                vector = Vector3.ClampMagnitude(vector, max_value) / max_value;

                // DeadZoneのチェック
                if (vector.sqrMagnitude <= dead_zone * dead_zone)
                {
                    vector = Vector3.zero;
                }
            }

            CharacterController.SimpleMove(vector * max_speed);
            base.Update();
        }
    }
}
