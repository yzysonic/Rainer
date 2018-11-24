using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinText : MonoBehaviour
{
    private PlayerIcon playerIcon;
    private Text joinText;

    // Use this for initialization
    void Start()
    {
        playerIcon = GetComponentInParent<PlayerIcon>();
        joinText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIcon.IsJoin)
        {
            joinText.text = ("Ⓑ 退出");
        }
        else
        {
            joinText.text = ("Ⓐ 参加");
        }
    }
}
