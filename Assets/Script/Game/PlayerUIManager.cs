using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour {

    public PlayerController playerController;

    public RectTransform RectTransform { get; private set; }
    public Canvas Canvas { get; private set; }
	public RainerCount RainerCount { get; private set; }
	public RectTransform GrowTreeFixed { get; private set; }
	public UIFallow GrowTree { get; private set; }
	public UIFallow GetRainer { get; private set; }

    private void Awake()
    {
        RectTransform   = GetComponent<RectTransform>();
        Canvas          = GetComponent<Canvas>();
        RainerCount     = transform.Find("RainerCount").GetComponent<RainerCount>();
        GrowTreeFixed   = transform.Find("GrowTreeFixed").GetComponent<RectTransform>();
        GrowTree        = transform.Find("GrowTree").GetComponent<UIFallow>();
        GetRainer       = transform.Find("GetRainer").GetComponent<UIFallow>();
    }

    void Start ()
    {
        GrowTree.camera = Canvas.worldCamera;
        GetRainer.camera = Canvas.worldCamera;
    }

}
