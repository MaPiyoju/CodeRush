using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DailyTipManager : MonoBehaviour
{
    public TextMeshProUGUI tipTxt;
    async void Awake()
    {
        FirestoreManager dbManager = new FirestoreManager();
        List<Dictionary<string, object>> items = await dbManager.ReadDataAsync("tips");
        string[] _tips = new string[items.Count];
        int cont = 0;
        foreach (var item in items)
        {
            _tips[cont] = item["tip"].ToString();
            cont++;
        }

        var random = new System.Random();
        tipTxt.text = _tips[random.Next(0,_tips.Length)];
    }
}
