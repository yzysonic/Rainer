using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = RainerLib.Tree;

public class SettingPlayerControl : PlayerController {

    public bool IsJoin;
    private Vector3 resetPosition;
    private Seed lastSeed;

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
                if (transform.position.y > resetPosition.y)
                {
                    Model.gameObject.SetActive(false);
                    while(followers.Count > 0)
                    {
                        followers.Pop().SetFree();
                    }
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
        if (lastSeed != null)
        {
            PlayerUITrigger.RemoveFromList(uiTrigger.NearTrees, lastSeed?.Tree.gameObject);
            lastSeed.StartFadeOut();
        }
        return lastSeed = base.ThrowSeed();
    }
}
