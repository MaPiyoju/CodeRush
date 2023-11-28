using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class toggle : MonoBehaviour
{
    public toggle my;
    private bool isOn;

    public void clikeado()
    {
        if (my.isOn is true)
        {
            my.isOn = false;
        }
        else
        {
            my.isOn = true;
        }
    }
}
