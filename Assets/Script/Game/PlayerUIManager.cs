using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;

public class PlayerUIManager : MonoBehaviour
{

    public RectTransform RectTransform { get; private set; }
    public Canvas Canvas { get; private set; }
	public RainerCount RainerCount { get; private set; }
	public RectTransform GrowTreeFixed { get; private set; }
	public UITreeProgress UITreeProgress { get; private set; }
	public UIFallow GetRainer { get; private set; }

    private void Awake()
    {
        RectTransform   = GetComponent<RectTransform>();
        Canvas          = GetComponent<Canvas>();
        RainerCount     = transform.Find("RainerCount").GetComponent<RainerCount>();
        GrowTreeFixed   = transform.Find("GrowTreeFixed").GetComponent<RectTransform>();
        UITreeProgress  = transform.Find("TreeProgress").GetComponent<UITreeProgress>();
        GetRainer       = transform.Find("GetRainer").GetComponent<UIFallow>();
    }

    void Start ()
    {
        UITreeProgress.camera = Canvas.worldCamera;
        GetRainer.camera = Canvas.worldCamera;
    }

}
