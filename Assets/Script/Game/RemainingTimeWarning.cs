using UnityEngine;
using UnityEngine.UI;
using RainerLib;

public class RemainingTimeWarning : MonoBehaviour {

    public float startTime = 30.0f;
    public float period = 10.0f;
    public float keepShowTime = 10.0f;
    public Texture[] textures;

    private GameTimer gameTimer;
    private new Animation animation;
    private bool timeTrigger;
    private RawImage image;
    private int textureId;

    void Start()
    {
        gameTimer = GameTimer.Instance;
        animation = GetComponent<Animation>();
        image = gameObject.GetComponent<RawImage>();
        textureId = 0;

        gameTimer.AddEvent(startTime, () =>
        {
            image.enabled = true;
            transform.localScale = Vector3.zero;
            SetTrigger();
        });

        gameTimer.AddEvent(0.0f, () => {
            gameObject.SetActive(false);
        });

        gameTimer.AddEvent(keepShowTime+0.5f, () =>
        {
            transform.localScale = Vector3.zero;
            image.color = Color.red;
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
        image.texture = textures[textureId];
        image.SetNativeSize();

        if (gameTimer.RemainingTime > keepShowTime)
        {
            SetTriggerEvent(period);
        }
        else
        {
            SetTriggerEvent(1.0f);
        }

        animation.Stop();
        animation.Play();
        textureId++;
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
