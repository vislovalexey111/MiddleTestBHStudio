using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUIManager : MonoBehaviour
{
    private NetworkRoomManagerExtended _networkManager;

    [Header("Screens")]
    [SerializeField] private JoinRoomUIManager _joinRoomUIManager;
    [SerializeField] private PlayerNameUIManager _playerNameUIManager;
    [SerializeField] private RoomUIManager _roomUIManager;
    
    [Header("UI Elements")]
    [SerializeField] private Button _buttonStartHost;
    [SerializeField] private Button _buttonStartClient;
    [SerializeField] private Button _buttonBack;
    [SerializeField] private TextMeshProUGUI _errorDisplay;

    private void Start()
    {
        _networkManager = FindObjectOfType<NetworkRoomManagerExtended>();
        _networkManager.OnClientEntered += SwitchToRoom;
        _networkManager.OnBackToMainMenu += SwitchToMainMenu;
        _networkManager.OnStartingHostFailed += HostingFailed;

        _buttonStartClient.onClick.AddListener(JoinAnotherRoom);
        _buttonStartHost.onClick.AddListener(CreateRoom);
        _buttonBack.onClick.AddListener(GoBack);
    }

    private void OnDestroy()
    {
        _buttonStartClient.onClick.RemoveAllListeners();
        _buttonStartClient.onClick.RemoveAllListeners();
        _buttonBack.onClick.RemoveAllListeners();

        _networkManager.OnClientEntered -= SwitchToRoom;
        _networkManager.OnBackToMainMenu -= SwitchToMainMenu;
        _networkManager.OnStartingHostFailed -= HostingFailed;
    }

    private void GoBack()
    {
        _playerNameUIManager.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void SwitchScenes(bool toRoom)
    {
        _roomUIManager.gameObject.SetActive(toRoom);
        gameObject.SetActive(!toRoom);
    }

    private void SwitchToRoom() => SwitchScenes(true);
    private void SwitchToMainMenu() => SwitchScenes(false);
    private void HostingFailed() => _errorDisplay.text = "Serever or Host are already started!";
    private void CreateRoom() => _networkManager.StartHost();
    private void JoinAnotherRoom() => _joinRoomUIManager.gameObject.SetActive(true);
}
