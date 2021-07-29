using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetManLobby _netManLobby = null;

    [SerializeField] private GameObject landingPagePanel = null;
    // Start is called before the first frame update

    public void HostLobby()
    {
        _netManLobby.StartHost();
        landingPagePanel.SetActive(false);
    }
}