using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;

public class PlayerUIManager : MonoBehaviour
{

    public RectTransform RectTransform { get; private set; }
    public Canvas Canvas { get; private set; }
	public RainerCount UIRainerCount { get; private set; }
	public UITreeProgress UITreeProgress { get; private set; }
	public UIFallow UIGrowTree { get; private set; }
	public UIFallow UIGetRainer { get; private set; }
    public UIFallow UIGrowTreeBubble { get; private set; }

    private void Awake()
    {
        RectTransform   = GetComponent<RectTransform>();
        Canvas          = GetComponent<Canvas>();
        UIRainerCount     = transform.Find("RainerCount")?.GetComponent<RainerCount>();
        UIGrowTree   = transform.Find("GrowTree")?.GetComponent<UIFallow>();
        UIGrowTreeBubble   = transform.Find("GrowTreeBubble")?.GetComponent<UIFallow>();
        UITreeProgress  = transform.Find("TreeProgress")?.GetComponent<UITreeProgress>();
        UIGetRainer       = transform.Find("GetRainer")?.GetComponent<UIFallow>();
    }

    void Start ()
    {
        if(UITreeProgress != null)
        {
            UITreeProgress.camera = Canvas.worldCamera;
        }

        if(UIGetRainer != null)
        {
            UIGetRainer.camera = Canvas.worldCamera;
        }

        if (UIGrowTree != null)
        {
            UIGrowTree.camera = Canvas.worldCamera;
        }

        if (UIGrowTreeBubble != null)
        {
            UIGrowTreeBubble.camera = Canvas.worldCamera;
        }
    }

    private void Update()
    {
        if(UIGetRainer != null)
        {

        }
    }

}
