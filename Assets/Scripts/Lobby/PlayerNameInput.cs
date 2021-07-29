using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")] [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;


    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";

    // Start is called before the first frame update
    void Start() => SetInputField();

    private void SetInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey))
        {
            return;
        }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        nameInputField.text = defaultName;
        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string defaultName)
    {
        continueButton.interactable = !string.IsNullOrEmpty(defaultName);
    }

    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;
        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }

    // Update is called once per frame
}