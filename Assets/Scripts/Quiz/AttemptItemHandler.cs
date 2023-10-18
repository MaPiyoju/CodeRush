using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttemptItemHandler : MonoBehaviour
{
    private Image[] _images;

    private void Awake()
    {
        _images = GetComponentsInChildren<Image>();
    }

    public void Mark()
    {
        _images[1].color = new Color(0.2313726F, 0.5921569F, 0.5686275F, 1);
    }

    public void UnMark()
    {
        _images[1].color = Color.white;
    }


}
