using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


public class PlayerManager : NetworkBehaviour
{
    private int cardValue;
    private List<int> hand = new List<int>();

    private void Update()
    {
        if (!isLocalPlayer) return;
        // add playable cards for this bisch   
    }

    private void PlayCard()
    {
        foreach (var value in hand)
        {
            if (value == cardValue)
            {
                hand.Remove(cardValue);
            }
        }

        CmdPlayCard();
    }

    [Command]
    public void CmdPlayCard()
    {
        print("played card on client");
    }

    private void SelectCard(int value)
    {
        cardValue = value;
     }
 
    // [syncVar] is server to client only
    
    // client rpc runs on all clients, target rpc runs with associated
    [TargetRpc]
    void Toohigh()
    {
        print("too high");
    }

    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        if (type == "dealt")
        {
            print("hel");
        }
        else if (type == "played")
        {
            print("ply");
        }
    }
}