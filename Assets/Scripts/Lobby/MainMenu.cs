using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetManLobby netManLobby = null;

    [SerializeField] private GameObject landingPagePanel = null;
    // Start is called before the first frame update

    public void HostLobby()
    {
        netManLobby.StartHost();
        landingPagePanel.SetActive(false);
        // TODO: set message showing ip address of the host
    }
}