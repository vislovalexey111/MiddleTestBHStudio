using System;
using Mirror;

public class NetworkRoomManagerExtended : NetworkRoomManager
{
    public Action OnUpdateUI;
    public Action OnFailedToConnect;
    public Action OnClientEntered;
    public Action OnStartingHostFailed;
    public Action OnBackToMainMenu;
    public Action<bool> OnPlayersReady;
    public Action<bool> OnSwitchReadyState;
    public Action<NetworkRoomPlayerExtended> OnUpdatePlayerReadyState;

    [Server]
    public void ChangeScene()
    {
        PlayerController[] gamePlayers = FindObjectsOfType<PlayerController>();

        if (gamePlayers.Length > 0)
        {
            foreach (PlayerController player in gamePlayers)
            {
                NetworkServer.Destroy(player.gameObject);
            }
        }

        ServerChangeScene(GameplayScene);
    }

    public override void OnRoomStopClient()
    {
        if (networkSceneName == GameplayScene)
            ServerChangeScene(RoomScene);

        OnBackToMainMenu?.Invoke();
    }
    
    //public override void OnRoomServerSceneChanged(string sceneName) { if (sceneName == RoomScene) OnBackToMainMenu?.Invoke(); }
    public override void OnRoomClientExit() { if (!NetworkClient.isConnected) OnFailedToConnect?.Invoke(); }
    public override void OnRoomClientEnter() => OnClientEntered?.Invoke();
    public override void OnRoomServerPlayersReady() => OnPlayersReady?.Invoke(true);
    public override void OnRoomServerPlayersNotReady() => OnPlayersReady?.Invoke(false);
}
