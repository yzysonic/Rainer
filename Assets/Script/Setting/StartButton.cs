using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {

    private SettingManager manager;
    private Button myButton;
    private Color color;
    private Text startText;


	// Use this for initialization
	void Start () {
        manager = GetComponentInParent<SettingManager>();
        myButton = GetComponent<Button>();
        color = myButton.image.color;
        startText = GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (manager.CanStart)
        {
            color.r = 1f;
            color.g = 0.5f;
            color.b = 0;
            startText.text = "+ボタンでゲームスタート";
        }
        else
        {
            color.r = color.g = color.b = 0.5f;
            startText.text = "プレイヤーの参加を待っています";
        }

        myButton.image.color = color;
    }

}
