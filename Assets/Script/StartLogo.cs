using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StartLogo : MonoBehaviour {

    public float countDown = 3.0f;
    public float fadeTime = 0.3f;
    private float timer;
    private Text text;

    public string Text
    {
        get
        {
            return text.text;
        }
        set
        {
            text.text = value;

        }
    }
    public bool Active
    {
        get
        {
            return gameObject.activeSelf;
        }
        set
        {
            gameObject.SetActive(value);
        }
    }

    private void OnEnable()
    {
        timer = countDown;
        text = GetComponent<Text>();
        transform.localScale = Vector3.one * 3.0f;
        StartCoroutine(FadeTo(1.0f, 1.0f));
    }

    // Update is called once per frame
    void Update () {

        timer -= Time.deltaTime;

        if (timer > 0.0f)
            return;

        StartCoroutine(FadeTo(0.0f, 3.0f, () => gameObject.SetActive(false)));

    }

    IEnumerator FadeTo(float alpha, float scale, Action callback = null)
    {
        var color = text.color;
        var lastAlpha = color.a;
        var lastScale = transform.localScale.x;
        var timer = 0.0f;

        Action<float> set_properties = (float progress) =>
        {
            transform.localScale = Vector3.one * Mathf.Lerp(lastScale, scale, progress);
            color.a = Mathf.Lerp(lastAlpha, alpha, progress);
            text.color = color;
        };

        while(timer <= fadeTime)
        {
            set_properties(timer / fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        set_properties(1.0f);

        callback?.Invoke();
    }
}
