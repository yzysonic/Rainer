using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainRumble : MonoBehaviour {

    [Tooltip("PlayerControllerがついていない場合のみ有効")]
    public int joyconNo = 0;

    private Joycon joycon;
    private int a_time = 0;

	// Use this for initialization
	void Start () {

        var manager = JoyconManager.Instance;

        if(joyconNo >= manager.j.Count)
        {
            enabled = false;
            return;
        }

        joycon = manager.j[joyconNo];

        var controller = GetComponent<PlayerController>();
        if (controller)
        {
            joycon = GameSetting.PlayerJoycons[controller.PlayerNo] ?? joycon;
        }

		if(joycon == null)
        {
            enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {

        System.Random ran = new System.Random();
        int l_f = ran.Next(100, 130);
        int h_f = ran.Next(120, 150);
        int t = ran.Next(30, 50);
        a_time += 1;
        int s_time = ran.Next(4, 28);

        if (a_time / s_time == 1)
        {
            joycon.SetRumble(l_f, h_f, 0.1f, t);
            a_time = 0;
        }

    }
}
