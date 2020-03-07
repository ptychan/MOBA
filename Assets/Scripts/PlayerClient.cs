using UnityEngine;
using UnityEngine.Networking;

public class PlayerClient : NetworkBehaviour
{
    public GameObject heroPrefab;

    [HideInInspector]
    public GameObject hero;

    void Start()
    {
        if (isLocalPlayer)
        {
            Cmd_SpawnHero();
        }
    }

    [Command]
    void Cmd_SpawnHero()
    {
        var spawnPoint = GameObject.Find("Sunny01");
        hero = Instantiate(heroPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        NetworkServer.SpawnWithClientAuthority(hero, connectionToClient);
    }
}
