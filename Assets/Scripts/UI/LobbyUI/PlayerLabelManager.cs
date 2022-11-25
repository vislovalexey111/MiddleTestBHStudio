using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLabelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _readyStateDisplay;
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private Button _buttonRemove;

    private void OnDestroy() => _buttonRemove.onClick.RemoveAllListeners();
    
    public void ShowButtonRemove() => _buttonRemove.gameObject.SetActive(true);
    public void SetReadyStatus(bool state) => _readyStateDisplay.text = state ? "Ready" : "Not Ready";
    public void SetName(string newName) => _playerName.text = newName;

    public void UpdateLabelInfo(NetworkRoomPlayerExtended player)
    {
        if (player == null) return;
        _playerName.text = player.Name;
        SetReadyStatus(player.readyToBegin);
        _buttonRemove.onClick.RemoveAllListeners();
        _buttonRemove.onClick.AddListener( delegate { player.connectionToClient.Disconnect();  } );
    }
}
