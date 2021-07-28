using System;
using DataStructs;
using UnityEngine;

public class Card : MonoBehaviour
{
    private int _value;
    public int GetValue() => _value;

    public void SetValue(int value)
    {
        _value = value;
    }
}