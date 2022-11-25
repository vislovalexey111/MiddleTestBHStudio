using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [Header("Network Manager")]
    [SerializeField] private NetworkRoomManagerExtended _networkManager;

    [Header("UI Screens")]
    [SerializeField] private LobbyUIManager _lobbyUIManager;
    [SerializeField] private RoomUIManager _roomUIManager;
    [SerializeField] private PlayerNameInput _playerNameInput;
    [SerializeField] private JoinLobbyMenu _joinLobbyMenu;

    private void Start() => EventsSetup();
    private void OnDestroy() => EventsDeactivate();
    private void CreateRoom() => _networkManager.StartHost();
    private void OpenJoinMenu(bool isOpen) => _joinLobbyMenu.gameObject.SetActive(isOpen);
    private void CloseRoomScreen() => SwitchToRoom(false);

    private void JoinRoom(string ipAdress)
    {
        _networkManager.networkAddress = ipAdress;
        _networkManager.StartClient();
    }

    private void NameInputToLobby(bool goForward)
    {
        _lobbyUIManager.gameObject.SetActive(goForward);
        _playerNameInput.gameObject.SetActive(!goForward);
    }

    private void SwitchToRoom(bool toRoom)
    {
        _lobbyUIManager.gameObject.SetActive(!toRoom);
        _roomUIManager.gameObject.SetActive(toRoom);
    }

    private void LaunchRoom()
    {
        //EventsDeactivate();
        OpenJoinMenu(false);
        SwitchToRoom(true);
    }

    private void EventsSetup()
    {
        _joinLobbyMenu.OnJoinRoom += JoinRoom;
        _joinLobbyMenu.OnReadyToJoin += OpenJoinMenu;
        _lobbyUIManager.OnJoiningRoom += OpenJoinMenu;
        _lobbyUIManager.OnCreateRoom += CreateRoom;
        _lobbyUIManager.OnGoingBack += NameInputToLobby;
        _playerNameInput.OnConfirmName += NameInputToLobby;
        _networkManager.OnClientEntered += LaunchRoom;
        _networkManager.OnFailedToConnect += CloseRoomScreen;
        _networkManager.OnFailedToConnect += _joinLobbyMenu.FailedToConnect;
        _networkManager.OnConnecting += _joinLobbyMenu.Connecting;
    }

    private void EventsDeactivate()
    {
        _joinLobbyMenu.OnJoinRoom -= JoinRoom;
        _joinLobbyMenu.OnReadyToJoin -= OpenJoinMenu;
        _lobbyUIManager.OnJoiningRoom -= OpenJoinMenu;
        _lobbyUIManager.OnCreateRoom -= CreateRoom;
        _lobbyUIManager.OnGoingBack -= NameInputToLobby;
        _playerNameInput.OnConfirmName -= NameInputToLobby;
        _networkManager.OnClientEntered -= LaunchRoom;
        _networkManager.OnFailedToConnect -= CloseRoomScreen;
        _networkManager.OnFailedToConnect -= _joinLobbyMenu.FailedToConnect;
        _networkManager.OnConnecting -= _joinLobbyMenu.Connecting;
    }
}
