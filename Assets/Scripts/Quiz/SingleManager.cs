using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleManager : MonoBehaviour
{
    public GameObject hint;
    public GameObject rushBtn;

    private void Awake()
    {
        if(Globals.lives < 1)
        {
            hint.SetActive(true);
            rushBtn.GetComponent<Button>().enabled = false;
        }
    }
}
