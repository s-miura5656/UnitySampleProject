﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour
{
    [SerializeField] private TimeCounter timeCounter;
    [SerializeField] private Text counterText;
    void Start()
    {
        timeCounter.OnTimeChanged += time =>
        {
            counterText.text = time.ToString();
        };
    }
}
