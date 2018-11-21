using UnityEngine;
using UnityEngine.UI;
using RainerLib;

public class RemainingTimeWarning : MonoBehaviour {

    public float startTime = 30.0f;
    public float period = 10.0f;
    public float keepShowTime = 10.0f;

    private GameTimer gameTimer;
    private Text text;
    private Animation animation;
    private bool timeTrigger;

    void Start()
    {
        text = GetComponent<Text>();
        gameTimer = GameTimer.Instance;
        animation = GetComponent<Animation>();

        gameTimer.AddEvent(startTime, () =>
        {
            text.enabled = true;
            transform.localScale = Vector3.zero;
            SetTrigger();
        });

        gameTimer.AddEvent(0.0f, () => {
            gameObject.SetActive(false);
        });

        //gameTimer.AddEvent(keepShowTime, () =>
        //{
        //    text.color = Color.red;
        //    text.fontSize = 172;
        //});
    }

    // Update is called once per frame
    void Update ()
    {

        if (!timeTrigger)
        {
            return;
        }

        timeTrigger = false;

        if (gameTimer.RemainingTime > keepShowTime)
        {
            text.text = $"残り {(int)gameTimer.RemainingTime + 1} 秒";
            animation.Play();
            SetTriggerEvent(period);
        }
        else
        {
            text.text = $"{(int)gameTimer.RemainingTime+1}";
            animation.Play("RemainingTimeWarning2");
            SetTriggerEvent(1.0f);
        }
	}

    void SetTrigger()
    {
        timeTrigger = true;
    }

    void SetTriggerEvent(float period)
    {
        gameTimer.AddPeriodEvent(period, SetTrigger, 1);
    }

}
