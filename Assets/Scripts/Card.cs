using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    [Range(1, 4)]
    public int suit;
    [SerializeField]
    [Range(1, 13)]
    public int value;
    public Tuple<int, int> GetInfo() => new Tuple<int, int>(suit, value);
}
