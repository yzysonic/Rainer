using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBGMController : MonoBehaviour {

    private BGMPlayer player;
    private GameTimer timer;

    private void Awake()
    {
        player = BGMPlayer.Instance;
        timer = GameTimer.Instance;
    }

    private void Start()
    {
        var changeTime = GameObject.Find("RemainingTimeWarning").GetComponent<RemainingTimeWarning>().startTime + 1.0f;
        timer.AddEvent(changeTime, () =>
        {
            player.AudioFades[0].stopAfterOut = true;
            player.AudioFades[0].Out();
            player.AudioSources[1].PlayScheduled(AudioSettings.dspTime + 1.0f);
            player.AudioSources[2].PlayScheduled(AudioSettings.dspTime + player.AudioSources[1].clip.length + 1.5f);
        });

    }

    // Update is called once per frame
    void Update () {
		
	}
}
