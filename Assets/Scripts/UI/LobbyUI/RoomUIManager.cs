using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class RoomUIManager : NetworkBehaviour
{
    [SerializeField] private Button _buttonBack;
    [SerializeField] private Button _buttonReady;
    [SerializeField] private Button _buttonReadyCancel;
    [SerializeField] private Button _buttonStart;
    [SerializeField] private PlayerLabelManager[] _playerLabels;
    [SerializeField] private NetworkRoomManagerExtended _networkManager;

    private void Start()
    {
        _buttonReady.onClick.AddListener(delegate { SwitchReady(true); });
        _buttonReadyCancel.onClick.AddListener(delegate { SwitchReady(false); });
        _buttonStart.onClick.AddListener(delegate { _networkManager.ChangeScene(); });
        _buttonBack.onClick.AddListener(Disconnect);

        _networkManager.OnUpdatePlayerReadyState += UpdatePlayerUI;
        _networkManager.OnPlayersReady += EnableStartButton;
        _networkManager.OnUpdateUI += UpdateUI;

        if (isServer)
        {
            _buttonStart.gameObject.SetActive(true);
            for (int i = 1; i < _playerLabels.Length; i++) _playerLabels[i].ShowButtonRemove();
        }

        _buttonStart.interactable = false;
    }

    private void OnDestroy()
    {
        _buttonBack.onClick.RemoveAllListeners();
        _buttonStart.onClick.RemoveAllListeners();
        _buttonReady.onClick.RemoveAllListeners();
        _buttonReady.onClick.RemoveAllListeners();

        _networkManager.OnUpdatePlayerReadyState -= UpdatePlayerUI;
        _networkManager.OnPlayersReady -= EnableStartButton;
        _networkManager.OnUpdateUI -= UpdateUI;
    }

    private void SwitchReady(bool playerState)
    {
        _buttonReady.gameObject.SetActive(!playerState);
        _buttonReadyCancel.gameObject.SetActive(playerState);
        _networkManager.OnSwitchReadyState?.Invoke(playerState);
    }

    private void Disconnect()
    {
        
    }

    private void UpdateUI()
    {
        for (int i = 0; i < _playerLabels.Length; i++)
            _playerLabels[i].gameObject.SetActive(false);

        for (int i = 0; i < _networkManager.roomSlots.Count; i++)
        {
            _playerLabels[i].gameObject.SetActive(true);
            _playerLabels[i].UpdateLabelInfo(_networkManager.roomSlots[i] as NetworkRoomPlayerExtended);
        }
    }

    private void EnableStartButton(bool buttonState) => _buttonStart.interactable = buttonState;
    private void UpdatePlayerUI(NetworkRoomPlayerExtended player) => _playerLabels[player.index].SetReadyStatus(player.readyToBegin);
}