using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;

public class PlayerUIManager : MonoBehaviour
{

    public RectTransform RectTransform { get; private set; }
    public Canvas Canvas { get; private set; }
	public RainerCount UIRainerCount { get; private set; }
	public RectTransform UIGrowTreeFixed { get; private set; }
	public UITreeProgress UITreeProgress { get; private set; }
	public UIFallow UIGetRainer { get; private set; }

    public int RainerCount
    {
        get
        {
            return UIRainerCount.Value;
        }
        set
        {
            UIRainerCount.Value = value;
            if (value == 1)
            {
                UIGrowTreeFixed.gameObject.SetActive(true);
            }
            else if(value == 0)
            {
                UIGrowTreeFixed.gameObject.SetActive(false);
            }

        }
    }

    private void Awake()
    {
        RectTransform   = GetComponent<RectTransform>();
        Canvas          = GetComponent<Canvas>();
        UIRainerCount     = transform.Find("RainerCount").GetComponent<RainerCount>();
        UIGrowTreeFixed   = transform.Find("GrowTreeFixed").GetComponent<RectTransform>();
        UITreeProgress  = transform.Find("TreeProgress").GetComponent<UITreeProgress>();
        UIGetRainer       = transform.Find("GetRainer").GetComponent<UIFallow>();
    }

    void Start ()
    {
        UITreeProgress.camera = Canvas.worldCamera;
        UIGetRainer.camera = Canvas.worldCamera;
    }

}
