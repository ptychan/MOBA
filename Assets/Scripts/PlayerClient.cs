using UnityEngine;
using UnityEngine.Networking;

public class PlayerClient : NetworkBehaviour
{
    void Start()
    {
        if (isLocalPlayer)
        {
            CmdRegisterPlayer("Peter", "Daren");
        }
    }

    [Command]
    void CmdRegisterPlayer(string playerName, string heroSelected)
    {
        GameManager.Instance.RegisterPlayer(playerName, heroSelected, connectionToClient);
    }
}
