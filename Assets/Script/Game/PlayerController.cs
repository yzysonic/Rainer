using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {

    public enum ControllType
    {
        Joycon,
        KeyboardMouse,
        None
    }

    public Canvas canvas;
    public ControllType controllType;

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
    private Animator animator;
    private RainerCount rainerCount;
    private Stack<GameObject> followers;
    private Transform model;
    private Vector3 moveDir;

    public int PlayerNo { get;  private set; }

    private void Awake()
    {
        moveDir = Vector3.zero;
        followers = new Stack<GameObject>();
        PlayerNo = int.Parse(gameObject.name.Substring(6, 1)) - 1;
    }

    void Start ()
    {
        joycon      = GameSetting.PlayerJoycons[PlayerNo] ?? (JoyconManager.Instance.j.Count > PlayerNo ? JoyconManager.Instance.j[PlayerNo] : null);
        controller  = GetComponent<CharacterController>();
        model       = transform.GetChild(0);
        animator    = model.GetComponent<Animator>();
        rainerCount = canvas.transform.Find("RainerCount").GetComponent<RainerCount>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        // Joycon
        if(joycon != null && controllType == ControllType.Joycon)
        {

            #region Rotate

            var stick = joycon.GetStick();
            transform.Rotate(0.0f, stick[0] * rotation_speed_scale, 0.0f);

            #endregion


            #region Move

            // Joyconの向きのベクトルを計算
            var raw_vector = Quaternion.Euler(90.0f, 0.0f, 0.0f) * joycon.GetVector() * Vector3.forward;

            // 移動方向に適用
            moveDir = new Vector3(raw_vector.x, 0.0f, raw_vector.z);

            // 最大角度を制限
            var max_value = Mathf.Sin(max_angle * Mathf.Deg2Rad);
            moveDir = Vector3.ClampMagnitude(moveDir, max_value) / max_value;

            #endregion

        }
        // キーボード・マウス
        else if(controllType == ControllType.KeyboardMouse)
        {
            // カメラ回転
            if(Input.GetMouseButton(0))
            {
                transform.Rotate(0.0f, Input.GetAxis("Mouse X") * rotation_speed_scale, 0.0f);
            }

            moveDir = new Vector3(Input.GetAxis("Horizontal"),0.0f,  Input.GetAxis("Vertical"));


            // レインナー操作
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (followers.Count > 0)
                {
                    followers.Pop().GetComponent<RainerController>().SetIdle(gameObject.transform.position);
                }
            }

        }


        // DeadZoneのチェック
        if (moveDir.sqrMagnitude >= dead_zone * dead_zone)
        {
            // 回転の適用
            moveDir = transform.rotation * moveDir;
            // モデルの向きを設定
            model.rotation = Quaternion.LookRotation(-moveDir);
        }
        else
        {
            moveDir = Vector3.zero;
        }


        // 移動する
        controller.SimpleMove(moveDir * max_speed);
        animator.SetFloat("speed", controller.velocity.magnitude);

    }

    // 当たり判定
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var target = hit.gameObject;
        if (target.layer == LayerMask.NameToLayer("RainerIdle"))
        {
            target.GetComponent<RainerController>().SetFollow(gameObject);
            followers.Push(target);
            rainerCount.Value++;
        }
    }
}
