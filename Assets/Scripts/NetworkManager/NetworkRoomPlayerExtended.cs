using Mirror;

public class NetworkRoomPlayerExtended : NetworkRoomPlayer
{
    private NetworkRoomManagerExtended _networkManager;

    [SyncVar(hook = nameof(HandleNameChange))] public string Name;
    [Command] private void CmdChangeName(string newName) => Name = newName;

    private void SwitchState(bool state)
    {
        if (isLocalPlayer)
            CmdChangeReadyState(state);
    }

    public override void OnClientEnterRoom()
    {
        _networkManager = FindObjectOfType<NetworkRoomManagerExtended>();
        _networkManager.OnSwitchReadyState += SwitchState;

        if (isLocalPlayer) CmdChangeName(PlayerNameInput.DisplayName);
    }

    public override void OnClientExitRoom()
    {
        _networkManager.OnSwitchReadyState -= SwitchState;
        _networkManager.OnUpdateUI?.Invoke();
    }

    // SyncVar Handlers
    private void HandleNameChange(string _, string newName) => _networkManager.OnUpdateUI?.Invoke();
    public override void ReadyStateChanged(bool _, bool newReadyState) => _networkManager.OnUpdatePlayerReadyState?.Invoke(this);
}