using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeHandler : MonoBehaviour
{
    private static TextMeshProUGUI _text;
    private static string _prefix = "x";

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.text = _prefix + Globals.lives.ToString();
    }

    public static void UpdateLife()
    {
        _text.text = _prefix + Globals.lives.ToString();
    }
}
