using System.Collections;
using UnityEngine;
using RainerLib;

public class CameraTopViewAnimation : MonoBehaviour {

    public float playTime = 1.0f;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    public Vector3 targetPos;
    public Vector3 targetRot;

    private Vector3 initPos;
    private Quaternion initRot;
    private Quaternion targetQua;
    private Timer timer;
    private float progress;

	// Use this for initialization
	void Start () {
        initPos = transform.position;
        initRot = transform.rotation;
        targetQua = Quaternion.Euler(targetRot);
        timer = new Timer(playTime);
        progress = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {

        progress = curve.Evaluate(timer.Progress);
        transform.position = Vector3.Lerp(initPos, targetPos, progress);
        transform.rotation = Quaternion.Lerp(initRot, targetQua, progress);

        if (timer.TimesUp())
        {
            enabled = false;
        }

        timer++;
    }
}
