using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class RoomUIManager : MonoBehaviour
{
    private NetworkRoomManagerExtended _networkManager;

    [Header("UI Elements")]
    [SerializeField] private Button _buttonToMainMenu;
    [SerializeField] private Button _buttonReady;
    [SerializeField] private Button _buttonCancel;
    [SerializeField] private Button _buttonStart;
    [SerializeField] private RoomPlayerLabelManager[] _roomPlayerLabels;
    

    private void Start()
    {
        _networkManager = FindObjectOfType<NetworkRoomManagerExtended>();
        _networkManager.OnUpdatePlayerReadyState += UpdatePlayerUI;
        _networkManager.OnPlayersReady += EnableStartButton;
        _networkManager.OnUpdateUI += UpdateUI;

        _buttonReady.onClick.AddListener(delegate { SwitchReady(true); });
        _buttonCancel.onClick.AddListener(delegate { SwitchReady(false); });
        _buttonStart.onClick.AddListener(_networkManager.ChangeScene);
        _buttonToMainMenu.onClick.AddListener(Disconnect);

        if (NetworkClient.isHostClient)
        {
            _buttonStart.gameObject.SetActive(true);
            for (int i = 1; i < _roomPlayerLabels.Length; i++) _roomPlayerLabels[i].ShowButtonRemove();
        }

        _buttonStart.interactable = false;
    }

    private void OnDestroy()
    {
        _buttonToMainMenu.onClick.RemoveAllListeners();
        _buttonStart.onClick.RemoveAllListeners();
        _buttonReady.onClick.RemoveAllListeners();
        _buttonCancel.onClick.RemoveAllListeners();

        _networkManager.OnUpdatePlayerReadyState -= UpdatePlayerUI;
        _networkManager.OnPlayersReady -= EnableStartButton;
        _networkManager.OnUpdateUI -= UpdateUI;
    }

    private void SwitchReady(bool playerState)
    {
        _buttonReady.gameObject.SetActive(!playerState);
        _buttonCancel.gameObject.SetActive(playerState);
        _networkManager.OnSwitchReadyState?.Invoke(playerState);
    }

    private void UpdateUI()
    {
        for (int i = 0; i < _roomPlayerLabels.Length; i++)
            _roomPlayerLabels[i].gameObject.SetActive(false);

        for (int i = 0; i < _networkManager.roomSlots.Count; i++)
        {
            _roomPlayerLabels[i].gameObject.SetActive(true);
            _roomPlayerLabels[i].UpdateLabelInfo(_networkManager.roomSlots[i] as NetworkRoomPlayerExtended);
        }
    }
    private void Disconnect()
    {
        if (NetworkClient.isHostClient)
            _networkManager.StopHost();
        else
            _networkManager.StopClient();
    }

    private void EnableStartButton(bool buttonState) => _buttonStart.interactable = buttonState;
    private void UpdatePlayerUI(NetworkRoomPlayerExtended player) => _roomPlayerLabels[player.index].SetReadyStatus(player.readyToBegin);
}