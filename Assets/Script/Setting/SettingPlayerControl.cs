using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tree = RainerLib.Tree;

public class SettingPlayerControl : PlayerController {

    public bool IsJoin;
    private Vector3 resetPosition;

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
        if (Model.gameObject.activeSelf != IsJoin)
        {
            if (IsJoin)
            {
                Model.gameObject.SetActive(true);
                Model.rotation = Quaternion.identity;
                CharacterController.enabled = true;
                transform.position = resetPosition;
            }
            else
            {
                CharacterController.SimpleMove(Vector3.zero);
                //CharacterController.Move(Vector3.zero);
                transform.Translate(Vector3.up);
                while (followers.Count > 0)
                {
                    followers.Pop().SetFree();
                }
                if (transform.position.y > resetPosition.y)
                {
                    Model.gameObject.SetActive(false);
                }
            }
        }

        if (IsJoin)
        {
            UpdateInput();

            // 移動する
            CharacterController.Move(Vector3.down);
            CharacterController.SimpleMove(MoveInputLocal * max_speed);

            if (GetActionDown(ActionButton.PopRainer))
            {
                PopRainer();
            }
            if (GetActionDown(ActionButton.PushRainer))
            {
                PushRainer();
            }

            UpdateModel();

        }
    }

    protected override Seed ThrowSeed()
    {
        transform.parent.GetComponentsInChildren<Seed>()?.ToList().ForEach(s => s.StartFadeOut());
        return base.ThrowSeed();
    }
}
