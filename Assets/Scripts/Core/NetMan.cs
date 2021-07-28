using Mirror;
using UnityEngine;


public class NetMan : NetworkManager
{
    public override void OnStartServer()
    {
        print("Server Start");
    }

    public override void OnStopServer()
    {
        print("Server Stop");
    }

// this works as client code on the client side client won't run it
    public override void OnClientConnect(NetworkConnection conn)
    {
        print("connected to server");
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        print("disconnected from server");
    }
}