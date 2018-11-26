using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour {

    private bool isJoin = false;
    private Joycon joycon;
    private AudioSource[] audios;

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
            IsJoin = value != null;
            joycon = value;
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
