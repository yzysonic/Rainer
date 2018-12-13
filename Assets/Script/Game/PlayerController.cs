using System;
using System.Collections.Generic;
using UnityEngine;
using Tree = RainerLib.Tree;


public class PlayerController : Rainer {

    public enum ControllType
    {
        Joycon,
        KeyboardMouse,
        Controller,
        None
    }

    public enum ActionButton : byte
    {
        PopRainer = 0x1,
        PushRainer = 0x2,
    }

    public GameObject seedPerfab;
    public float seedSpeed = 2.0f;
    public PlayerUIManager uiManager;
    public ControllType controllType;

    [Range(1.0f, 30.0f)]
    public float max_speed = 10.0f;
    [Range(0.0f, 10.0f)]
    public float rotation_speed = 3.0f;
    [Range(0.0f, 10.0f)]
    public float auto_rotation_speed = 1.0f;
    [Range(0.0f, 0.1f)]
    public float dead_zone = 0.08f;
    [Range(10.0f, 90.0f)]
    public float max_angle = 40.0f;

    protected Stack<RainerController> followers;
    protected PlayerUITrigger uiTrigger;
    private byte buttonBuffer;
    private Action startAction;

    public Joycon Joycon { get; set; }
    public Vector3 MoveInput { get; private set; }
    public Vector3 MoveInputLocal { get; private set; }
    public Vector3 RotateInput { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        followers = new Stack<RainerController>();
        PlayerNo = int.Parse(gameObject.name.Substring(6, 1)) - 1;
    }

    protected override void Start ()
    {
        base.Start();
        Joycon      = GameSetting.PlayerJoycons[PlayerNo] ?? (JoyconManager.Instance.j.Count > PlayerNo ? JoyconManager.Instance.j[PlayerNo] : null);
        if(MinimapIcon != null)
        {
            MinimapIcon.GetComponent<Renderer>().material.color = GameSetting.PlayerColors[PlayerNo];
        }
        uiTrigger   = GetComponentInChildren<PlayerUITrigger>();
        uiTrigger.enabled = true;
        startAction?.Invoke();
    }
	
	// Update is called once per frame
	protected virtual void Update ()
    {
        UpdateInput();

        // カメラ回転
        var rotate =
            RotateInput * rotation_speed + // 入力による回転
            Vector3.up * Vector3.Dot(MoveInputLocal, Vector3.right) * auto_rotation_speed; // 移動による自動回転

        transform.Rotate(rotate * Time.deltaTime * 60.0f);

        // 移動する
        CharacterController.SimpleMove(MoveInput * max_speed);

        if(GetActionDown(ActionButton.PopRainer))
        {
            PopRainer();
        }

        if (GetActionDown(ActionButton.PushRainer))
        {
            PushRainer(uiTrigger.NearestRainer);
        }

        UpdateModel();
    }

    public void UpdateInput()
    {
        buttonBuffer = 0;

        switch (controllType)
        {
            case ControllType.Joycon:

                if (Joycon == null)
                {
                    break;
                }

                #region CameraRotate

                var stick = Joycon.GetStick();
                RotateInput = new Vector3(0.0f, stick[0], 0.0f);

                #endregion

                #region Move

                // Joyconの向きのベクトルを計算
                var raw_vector = Joycon.GetAccel();

                // 移動方向に適用
                MoveInput = new Vector3(-raw_vector.y, 0.0f, -raw_vector.z);

                // 最大角度を制限
                var max_value = Mathf.Sin(max_angle * Mathf.Deg2Rad);
                MoveInput = Vector3.ClampMagnitude(MoveInput, max_value) / max_value;

                #endregion

                #region Action
                if (Joycon.GetButtonDown(GameSetting.JoyconButton.PushRainer))
                {
                    SetActionDown(ActionButton.PushRainer);
                }
                if (Joycon.GetButtonDown(GameSetting.JoyconButton.PopRainer))
                {
                    SetActionDown(ActionButton.PopRainer);
                }
                #endregion

                break;


            case ControllType.KeyboardMouse:

                // カメラ回転
                if (Input.GetMouseButton(0))
                {
                    RotateInput = new Vector3(0.0f, Input.GetAxis("Mouse X"), 0.0f);
                }
                else
                {
                    RotateInput = Vector3.Lerp(RotateInput, Vector3.zero, Time.deltaTime * 10.0f);
                }

                // 移動
                MoveInput = new Vector3(Input.GetAxis("KeyMoveX"), 0.0f, Input.GetAxis("KeyMoveY"));

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetActionDown(ActionButton.PopRainer);
                }

                if(Input.GetKeyDown(KeyCode.Return))
                {
                    SetActionDown(ActionButton.PushRainer);
                }

                break;


            case ControllType.Controller:

                // カメラ回転
                RotateInput = new Vector3(0.0f, Input.GetAxis("JoyCameraX"), 0.0f);

                // 移動
                MoveInput = new Vector3(Input.GetAxis("JoyMoveX"), 0.0f, Input.GetAxis("JoyMoveY"));

                if(Input.GetButtonDown("JoyAction1"))
                {
                    SetActionDown(ActionButton.PopRainer);
                }

                if(Input.GetButtonDown("JoyAction2"))
                {
                    SetActionDown(ActionButton.PushRainer);
                }

                break;
        }

        // DeadZoneのチェック
        if (MoveInput.sqrMagnitude <= dead_zone * dead_zone)
        {
            MoveInput = Vector3.zero;
        }

        MoveInputLocal = MoveInput;

        // 回転の適用
        MoveInput = transform.rotation * MoveInput;


    }

    public void PushRainer(RainerController rainer)
    {
        if(!(rainer?.SetFollow(this) ?? false))
        {
            return;
        }

        followers.Push(rainer);

        if(uiManager?.UIRainerCount != null)
        {
            uiManager.UIRainerCount.Value++;
        }

    }

    public void PushRainer()
    {
        PushRainer(uiTrigger.NearestRainer);
    }

    public RainerController PopRainer()
    {
        if (followers.Count == 0)
        {
            return null;
        }

        var tree = uiTrigger.NearestTree ?? ThrowSeed().Tree;

        if (tree == null)
        {
            return null;
        }

        var rainer = followers.Pop();
        rainer.SetGrowTree(tree);

        if (uiManager?.UIRainerCount != null)
        {
            uiManager.UIRainerCount.Value--;
        }

        return rainer;
    }

    private void SetActionDown(ActionButton action)
    {
        buttonBuffer |= (byte)action;
    }

    protected virtual Seed ThrowSeed()
    {
        var seed = Instantiate(seedPerfab, transform.position - Model.forward * 0.5f, Quaternion.identity).GetComponent<Seed>();
        seed.transform.parent = transform.parent;
        //seed.GetComponent<Rigidbody>().AddForce(CharacterController.velocity + Model.rotation * new Vector3(1.0f, 1.0f, -1.0f) * seedSpeed, ForceMode.VelocityChange);
        return seed;
    }

    public bool GetActionDown(ActionButton action)
    {
        return (buttonBuffer & (byte)action) != 0;
    }

    public void AddStartAction(Action action)
    {
        startAction += action;
    }

}
