using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinText : MonoBehaviour
{
    public PlayerIcon playerIcon;
    private Text joinText;

    // Use this for initialization
    void Start()
    {

        joinText = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerIcon.IsJoin)
        {
            joinText.text = ("×ボタンでキャンセル");
        }
        else
        {
            joinText.text = ("○ボタンで参加");
        }
    }
}
