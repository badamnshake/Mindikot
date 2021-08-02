using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

// network manager for lobby which lets the players join and passes control onto another manager later
public class NetManLobby : NetworkManager
{
    [SerializeField] private int minPlayers = 2;
    [Scene] [SerializeField] private string menuScene = string.Empty;
    [Scene] [SerializeField] private string gameScene = string.Empty;

    [Header("Room")] [SerializeField] private NetworkRoomPlayer roomPlayerPrefab = null;
    [Header("Room")] [SerializeField] private NetworkGamePlayer gamePlayerPrefab = null;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public static event Action OnServerStopped;

    public static event Action GameStarts;
    public List<NetworkRoomPlayer> RoomPlayers { get; } = new List<NetworkRoomPlayer>();
    public List<NetworkGamePlayer> GamePlayers { get; } = new List<NetworkGamePlayer>();

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        OnClientDisconnected?.Invoke();
    }

    // when a client connects to Me netman
    public override void OnServerConnect(NetworkConnection conn)
    {
        // if (numPlayers >= maxConnections)
        // {
        //     conn.Disconnect();
        //     return;
        // }

        // when not in lobby don't let another client connect
        // if (SceneManager.GetActiveScene().name != menuScene)
        // {
        //     conn.Disconnect();
        // }
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        // if (SceneManager.GetActiveScene().name == menuScene)
        // {
        bool isLeader = RoomPlayers.Count == 0;


        NetworkRoomPlayer roomPlayer = Instantiate(roomPlayerPrefab);
        roomPlayer.IsLeader = isLeader;
        NetworkServer.AddPlayerForConnection(conn, roomPlayer.gameObject);
        // }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayer>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        RoomPlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers)
        {
            return false;
        }

        foreach (var player in RoomPlayers)
        {
            if (!player.isReady)
            {
                return false;
            }
        }

        return true;
    }

    public void StartGame()
    {
        // if (SceneManager.GetActiveScene().name == menuScene)
        if (!IsReadyToStart())
        {
            return;
        }

        print("start the game");
        ServerChangeScene(gameScene);
        GameStarts?.Invoke();
    }

    public override void ServerChangeScene(string newSceneName)
    {
        // From menu to game
        // if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith("Scene_Map"))
        for (int i = RoomPlayers.Count - 1; i >= 0; i--)
        {
            var conn = RoomPlayers[i].connectionToClient;
            var gameplayerInstance = Instantiate(gamePlayerPrefab);
            gameplayerInstance.SetDisplayName(RoomPlayers[i].displayName);

            NetworkServer.Destroy(conn.identity.gameObject);

            NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
        }

        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        // if (sceneName.StartsWith("Scene_Map"))
        // {
        print(GamePlayers.Count);
        GameObject.FindWithTag("gameHandler").GetComponent<GameHandler>().SetPlayers(GamePlayers);
        GameObject.FindWithTag("gameHandler").GetComponent<GameHandler>().StartGame();
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        OnServerReadied?.Invoke(conn);
    }
}