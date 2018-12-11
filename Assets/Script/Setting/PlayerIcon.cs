using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour {

    public JoyconModel joyconModel;
    public SettingPlayerControl playerModel;

    private bool isJoin = false;
    private Joycon joycon;
    private AudioSource[] audios;
    private Color color;
    private int colorIndex;

    public bool IsJoin
    {
        get
        {
            return isJoin;
        }
        set
        {
            if(isJoin == value)
            {
                return;
            }

            audios[value ? 0 : 1].Play();
            isJoin = value;
            joyconModel.gameObject.SetActive(isJoin);
            playerModel.IsJoin = IsJoin;
        }
    }
    public int PlayerNo { get; set; }
    public Joycon Joycon
    {
        get
        {
            return joycon;
        }
        set
        {
            joycon?.UnbindPlayer();
            joycon = value;
            joyconModel.Joycon = joycon;
            playerModel.Joycon = joycon;
            IsJoin = joycon != null;
            ColorIndex = joycon?.ColorIndex ?? -1;
            playerModel.PlayerNo = joycon?.ColorIndex ?? int.Parse(playerModel.gameObject.name.Substring(6, 1)) - 1;
        }
    }

    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            color = value;
            GetComponentInChildren<Text>().color = isJoin ? color : Color.black;
            joyconModel.GetComponent<Renderer>().material.color = color;
            playerModel.Color = color;
        }
    }

    public int ColorIndex
    {
        get
        {
            return colorIndex;
        }
        set
        {
            colorIndex = value;
            Color = colorIndex >= 0 ? GameSetting.DefaultPlayerColors[colorIndex] : Color.gray;
        }
    }

	// Use this for initialization
	void Start () {
        audios = GetComponents<AudioSource>();
    }
	
	// Update is called once per frame
	void LateUpdate () {
        
        if(Joycon?.GetButtonDown(GameSetting.JoyconButton.Cancel) ?? false)
        {
            Joycon = null;
        }
    }

}
