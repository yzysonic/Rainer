using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using RainerLib;

public class GameTimer : Singleton<GameTimer> {

    class PeriodEvent
    {
        public float period;
        public int repeat;
        public Action callback;
        public float lastTime;

        public PeriodEvent(float period, int repeat, Action callback, float lastTime)
        {
            this.period = period;
            this.repeat = repeat;
            this.callback = callback;
            this.lastTime = lastTime;
        }
    }

    #region Fields

    [Range(0, 300)]
    public int initTime = 60;

    private bool started = false;
    private Text text;
    private float remainingTime;
    private CircularGauge gauge;
    private List<PeriodEvent> periodEvents = new List<PeriodEvent>();

    #endregion

    #region Properties

    /// <summary>
    /// 残り時間（秒単位）
    /// </summary>
    public float RemainingTime
    {
        get
        {
            return started? remainingTime : initTime;
        }
        set
        {
            var displayTime = (int)(value+0.99f);
            text.text = displayTime.ToString();
            gauge.Values[0] = initTime - value;
            gauge.Values[1] = value;
            remainingTime = value;
        }
    }
    /// <summary>
    /// 時間切れ
    /// </summary>
    public bool TimesUp
    {
        get
        {
            return RemainingTime <= 0.0f;
        }
    }

    #endregion

    #region UnityMethods

    private void OnValidate()
    {
        GetComponentInChildren<Text>().text = initTime.ToString();
    }

    // Use this for initialization
    void Start () {
        text = GetComponentInChildren<Text>();
        gauge = GetComponentInChildren<CircularGauge>();
        RemainingTime = initTime;
        started = true;
    }
	
	// Update is called once per frame
	void Update () {
        RemainingTime -= Time.deltaTime;
        UpdateReriodEvent();
    }

    #endregion

    #region Methods

    /// <summary>
    /// 周期イベントを追加する。
    /// </summary>
    /// <param name="period">実行する周期（秒単位）</param>
    /// <param name="callback">イベントが発生した際に実行するもの</param>
    /// <param name="repeat">繰り返し回数。負数を指定すると無限に実行される(デフォルト)。</param>
    public void AddPeriodEvent(float period, Action callback, int repeat = -1)
    {
        if (period <0.0f || callback == null)
            return;

        periodEvents.Add(new PeriodEvent(period, repeat, callback, RemainingTime));
    }

    /// <summary>
    /// タイミングイベントを追加する。
    /// </summary>
    /// <param name="executeTime">実行する時間。（秒単位）</param>
    /// <param name="callback">イベントが発生した際に実行するもの</param>
    public void AddEvent(float executeTime, Action callback)
    {
        periodEvents.Add(new PeriodEvent(RemainingTime - executeTime, 1, callback, RemainingTime));
    }

    private void UpdateReriodEvent()
    {
        foreach (var periodEvent in periodEvents.ToArray())
        {
            if ((periodEvent.lastTime - RemainingTime) < periodEvent.period)
            {
                continue;
            }

            periodEvent.callback();
            periodEvent.repeat--;

            if (periodEvent.repeat == 0)
            {
                periodEvents.Remove(periodEvent);
                continue;
            }

            periodEvent.lastTime = RemainingTime;
        }
    }


    #endregion

}
