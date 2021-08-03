using System;
using DataStructs;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Color rulingCardColor;
    private int _value;
    public int GetValue() => _value;

    public void SetValue(int value) => _value = value;

    public void SetInteractable(bool value) => button.interactable = value;
    public void SetCardRuling() => image.color = rulingCardColor;
}