using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LobbyUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button _buttonStartHost;
    [SerializeField] private Button _buttonStartClient;
    [SerializeField] private Button _buttonBack;
    [SerializeField] private TextMeshProUGUI _errorDisplay;

    public Action OnCreateRoom;
    public Action<bool> OnJoiningRoom;
    public Action<bool> OnGoingBack;

    private void Start()
    {
        _buttonStartClient.onClick.AddListener(JoinRoom);
        _buttonStartHost.onClick.AddListener(CreateRoom);
        _buttonBack.onClick.AddListener(GoBack);
    }

    private void GoBack() => OnGoingBack?.Invoke(false);
    private void JoinRoom() => OnJoiningRoom?.Invoke(true);
    private void CreateRoom() => OnCreateRoom?.Invoke();
    public void DisplayError(string errorMessage) => _errorDisplay.text = errorMessage;

    private void OnDestroy()
    {
        _buttonStartClient.onClick.RemoveAllListeners();
        _buttonStartClient.onClick.RemoveAllListeners();
        _buttonBack.onClick.RemoveAllListeners();
    }
}
