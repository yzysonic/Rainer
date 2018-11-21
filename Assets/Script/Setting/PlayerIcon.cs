using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour {

    private Joycon joycon;
    public bool IsJoin { get; set; } = false;
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
            SetJoycon(value);
        }
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void LateUpdate () {
        
        if(Joycon?.GetButtonDown(GameSetting.Button.Cancel) ?? false)
        {
            //StartCoroutine(UnsetJoycon());
            Joycon = null;
        }
    }

    void UnsetJoycon()
    {
        Joycon = null;
        joycon.UnbindPlayer();
    }

    void SetJoycon(Joycon joycon)
    {
        this.joycon?.UnbindPlayer();
        this.joycon = joycon;
    }

}
