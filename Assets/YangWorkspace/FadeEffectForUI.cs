using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Graphic))]
sealed public class FadeEffectForUI : MonoBehaviour
{
 //   sealed private class Timer : RainerLib.Timer
 //   {
 //       private FadeEffectForUI owner;
 //       public Timer(FadeEffectForUI owner)
 //       {
 //           this.owner = owner;
 //       }

 //       protected override void UpdateProgress()
 //       {
 //           Progress = owner.curve.Evaluate(elapsed / interval);
 //       }

 //       public static Timer operator++(Timer timer)
 //       {
 //           timer.Step();
 //           return timer;
 //       }
 //   }

 //   public float fadeInTime             = 0.3f;
 //   public float fadeOutTime            = 0.3f;
 //   public Color fadeInColor            = Color.white;
 //   public Color fadeOutColor           = new Color(1.0f, 1.0f, 1.0f, 0.0f);
 //   public Vector3 fadeInScale          = Vector3.one;
 //   public Vector3 fadeOutScale         = Vector3.one;
 //   public AnimationCurve fadeInCurve   = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
 //   public AnimationCurve fadeOutCurve  = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

 //   [SerializeField]
 //   private Graphic graphic;
 //   private Timer timer;
 //   private Color lastColor;
 //   private Color targetColor;
 //   private Vector3 lastScale;
 //   private Vector3 targetScale;
 //   private AnimationCurve curve;
 //   private Action callback;

 //   public Graphic Graphic { get { return graphic; } }
 //   public float Progress { get; private set; }

 //   private void OnValidate()
 //   {
 //       if (graphic == null)
 //           graphic = GetComponent<Graphic>();
 //   }

 //   void Update () {

 //       if (Progress > 1.0f)
 //           return;

 //       timer++;
 //       graphic.color = Color.Lerp(lastColor, targetColor, Progress);
 //       graphic.rectTransform.localScale = Vector3.Lerp(lastScale, targetScale, Progress);

 //       if (Progress >= 1.0f)
 //           callback?.Invoke();
	//}

 //   FadeEffectForUI()
 //   {
 //       timer = new Timer(this);
 //   }

 //   void FadeIn(Action callback = null)
 //   {
 //       targetColor = fadeInColor;
 //       targetScale = fadeInScale;
 //       curve = fadeInCurve;
 //       timer.Reset(fadeInTime);
 //       this.callback = callback;
 //   }

}
