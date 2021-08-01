using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    // components of game object
    [SerializeField] private Text initials;
    [SerializeField] private Image indicator;

    [SerializeField] private Image iconBackground;

    // fast customization of colors and look
    [SerializeField] private bool team = true;
    [SerializeField] private Color teamBlueColor;
    [SerializeField] private Color teamOrangeColor;
    [SerializeField] private Color indicatorOffColor;
    [SerializeField] private Color indicatorOnColor;
    [SerializeField] private Color indicatorWarningColor;

    private float _timeout = 0;
    private float _timeLeft = 0;

    private void Update()
    {
        if (_timeLeft > 0)
        {
            if (_timeLeft / _timeout < 0.5f) indicator.color = indicatorWarningColor;

            indicator.fillAmount = Mathf.Max(_timeLeft / _timeout, 0);
            _timeLeft -= Time.deltaTime;
        }
    }

    public void StartTimer(float timeout)
    {
        _timeout = timeout;
        _timeLeft = timeout;
        indicator.color = indicatorOnColor;
    }

    public void ClearTimer()
    {
        _timeLeft = 0;
        indicator.color = indicatorOffColor;
        indicator.fillAmount = 1f;
    }


    public void SetInit(string initial)
    {
        initials.text = initial;
        iconBackground.color = team ? teamBlueColor : teamOrangeColor;
    }
}