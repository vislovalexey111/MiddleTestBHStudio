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
    private void CloseRoomScreen() => _lobbyUIManager.gameObject.SetActive(true);

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

    private void ChangeToRoom()
    {
        EventsDeactivate();
        OpenJoinMenu(false);
        _lobbyUIManager.gameObject.SetActive(false);
        _roomUIManager.gameObject.SetActive(true);
    }

    private void EventsSetup()
    {
        _joinLobbyMenu.OnJoinRoom += JoinRoom;
        _joinLobbyMenu.OnReadyToJoin += OpenJoinMenu;
        _lobbyUIManager.OnJoiningRoom += OpenJoinMenu;
        _lobbyUIManager.OnCreateRoom += CreateRoom;
        _lobbyUIManager.OnGoingBack += NameInputToLobby;
        _playerNameInput.OnConfirmName += NameInputToLobby;
        _networkManager.OnClientEntered += ChangeToRoom;
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
        _networkManager.OnClientEntered -= ChangeToRoom;
        _networkManager.OnFailedToConnect -= CloseRoomScreen;
        _networkManager.OnFailedToConnect -= _joinLobbyMenu.FailedToConnect;
        _networkManager.OnConnecting -= _joinLobbyMenu.Connecting;
    }
}
