using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetManLobby _netManLobby;

    [Header("Ui")] [SerializeField] private GameObject landingPagePanel = null;

    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable()
    {
        NetManLobby.OnClientConnected += HandleClientConnected;
        NetManLobby.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetManLobby.OnClientConnected -= HandleClientConnected;
        NetManLobby.OnClientDisconnected -= HandleClientDisconnected;
    }

    // takes an ip address : can be localhost,
    // and connects to it starts client if it is connected handle method will be fired
    public void JoinLobby()
    {
        string ipAddress = ipAddressInputField.text;
        _netManLobby.networkAddress = ipAddress;
        _netManLobby.StartClient();
        joinButton.interactable = false;
    }

    void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}