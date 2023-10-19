using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    private float _startTime;
    private float _ellapsedTime;
    private TextMeshProUGUI _timerTxt;

    private bool _timerRunning = false;

    private void Awake()
    {
        _timerTxt = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerRunning)
        {
            _ellapsedTime = Time.time - _startTime;
            System.TimeSpan calcEllapsed = System.TimeSpan.FromSeconds(_ellapsedTime);
            _timerTxt.text = string.Format("{0}:{1}", calcEllapsed.Minutes < 10 ? "0"+calcEllapsed.Minutes.ToString() : calcEllapsed.Minutes, calcEllapsed.Seconds < 10 ? "0"+ calcEllapsed.Seconds.ToString() : calcEllapsed.Seconds);
        }
    }

    public float GetTime()
    {
        return _ellapsedTime;
    }

    public void SetTimer()
    {
        _startTime = Time.time;
        _timerRunning = true;
    }

    public void StopTimer()
    {
        _timerRunning = false;
    }
}
