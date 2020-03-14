using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    [System.Serializable]
    public struct TeamColor
    {
        public Color slot1;
        public Color slot2;
        public Color slot3;
        public Color slot4;
        public Color slot5;
    }

    public TeamColor teamSunny;
    public TeamColor teamGloomy;

    public struct PlayerInfo
    {
        public string playerName;
        public string heroSelected;
    }

    public class PlayerInfoList : SyncListStruct<PlayerInfo> { }

    //List<PlayerInfo> sunnyPlayers = new List<PlayerInfo>(); << List cannot be synced!
    PlayerInfoList sunnyPlayers = new PlayerInfoList();
    List<NetworkConnection> sunnyConnections = new List<NetworkConnection>();
    
    PlayerInfoList gloomyPlayers = new PlayerInfoList();
    List<NetworkConnection> gloomyConnections = new List<NetworkConnection>();

    [SyncVar]
    private bool gameStarted = false;

    public static GameManager Instance;

    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(Screen.width - 200, 0, 200, 400));

        if (NetworkServer.active && sunnyPlayers.Count > 0)
        {
            if (GUILayout.Button("Start Match"))
                StartMatch();
        }

        GUILayout.Label("-= Team Sunny =-");
        foreach (var player in sunnyPlayers)
        {
            GUILayout.Label(player.playerName + " : " + player.heroSelected);
        }
        GUILayout.Label("-= Team Gloomy =-");
        foreach (var player in gloomyPlayers)
        {
            GUILayout.Label(player.playerName + " : " + player.heroSelected);
        }

        GUI.EndGroup();
    }

    void StartMatch()
    {
        gameStarted = true;

        for (int i = 0; i < sunnyPlayers.Count; ++i)
        {
            var playerInfo = sunnyPlayers[i];
            var spawnPoint = GameObject.Find("Sunny0" + (i + 1).ToString());
            var heroPrefab = HeroCatalog.Instance.FindHeroByName(playerInfo.heroSelected);
            var hero = Instantiate(heroPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            NetworkServer.SpawnWithClientAuthority(hero, sunnyConnections[i]);
        }

        for (int i = 0; i < gloomyPlayers.Count; ++i)
        {
            var playerInfo = gloomyPlayers[i];
            var spawnPoint = GameObject.Find("Gloomy0" + (i + 1).ToString());
            var heroPrefab = HeroCatalog.Instance.FindHeroByName(playerInfo.heroSelected);
            var hero = Instantiate(heroPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            NetworkServer.SpawnWithClientAuthority(hero, gloomyConnections[i]);
        }
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }
    
    public void RegisterPlayer(string playerName, string heroSelected, NetworkConnection clientConnection)
    {
        if (sunnyPlayers.Count >= 5 && gloomyPlayers.Count >= 5)
            return;

        if (sunnyPlayers.Count <= gloomyPlayers.Count)
        {
            sunnyPlayers.Add(new PlayerInfo
            {
                playerName = playerName,
                heroSelected = heroSelected,
            });
            sunnyConnections.Add(clientConnection);
        }
        else
        {
            gloomyPlayers.Add(new PlayerInfo
            {
                playerName = playerName,
                heroSelected = heroSelected
            });
            gloomyConnections.Add(clientConnection);
        }
    }
}
