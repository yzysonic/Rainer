using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour {


    private JoinState joinState;
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
            StartCoroutine(SetJoycon(value));
        }
    }
	// Use this for initialization
	void Start () {

        joinState = GetComponentInChildren<JoinState>();

	}
	
	// Update is called once per frame
	void LateUpdate () {

        joinState.isJoin = IsJoin;

        if(Joycon?.GetButtonDown(GameSetting.Button.Join) ?? false)
        {
            //StartCoroutine(UnsetJoycon());
            Joycon = null;
        }
    }

    IEnumerator UnsetJoycon()
    {
        var joycon = Joycon;
        Joycon = null;
        yield return new WaitForEndOfFrame();
        joycon.UnbindPlayer();
    }

    IEnumerator SetJoycon(Joycon joycon)
    {
        for(var i=0;i<2;i++)
        {
            yield return null;
        }
        this.joycon?.UnbindPlayer();
        this.joycon = joycon;
    }

}
