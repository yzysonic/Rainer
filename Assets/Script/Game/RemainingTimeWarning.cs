using UnityEngine;
using UnityEngine.UI;
using RainerLib;

public class RemainingTimeWarning : MonoBehaviour {

    public float startTime = 30.0f;
    public float period = 10.0f;
    public float keepShowTime = 10.0f;

    private GameTimer gameTimer;
    private Text text;
    private new Animation animation;
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

        gameTimer.AddEvent(keepShowTime+0.5f, () =>
        {
            transform.localScale = Vector3.zero;
            text.color = Color.red;
            text.fontSize = 172;
        });

        SetTriggerEvent(gameTimer.initTime - keepShowTime);
    }

    // Update is called once per frame
    void Update ()
    {

        if (!timeTrigger)
        {
            return;
        }

        timeTrigger = false;
        var time = (int)gameTimer.RemainingTime + 1;

        if (gameTimer.RemainingTime > keepShowTime)
        {
            text.text = $"残り {time} 秒";
            SetTriggerEvent(period);
        }
        else
        {
            text.text = $"{time}";
            SetTriggerEvent(1.0f);
        }

        animation.Stop();
        animation.Play();
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
