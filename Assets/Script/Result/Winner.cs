    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Winner : MonoBehaviour
{
    public Text setWinner;
    public Lank lank;
    private string winner;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        winner = lank.setWinner;

        setWinner.text = winner;
    }
}
