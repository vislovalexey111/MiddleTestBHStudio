using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _errorMessage;
    [SerializeField] private TMP_InputField _ipAdressInputField;
    [SerializeField] private Button _joinButton;
    [SerializeField] private Button _cancelButton;

    public Action<bool> OnReadyToJoin;
    public Action<string> OnJoinRoom;

    private void OnEnable()
    {
        _ipAdressInputField.onValueChanged.AddListener(ValidateIPAdress);
        _joinButton.onClick.AddListener(JoinRoom);
        _cancelButton.onClick.AddListener(CancelJoinRoom);
    }

    private void OnDisable()
    {
        _joinButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();
    }

    private void ValidateIPAdress(string ipAdress)
    {
        bool isIpAdressValid = !string.IsNullOrEmpty(ipAdress) && !ipAdress.StartsWith(' ');
        _joinButton.interactable = isIpAdressValid;
        _errorMessage.text = isIpAdressValid ? "" : "Please, enter correct ipAdress";
    }

    private void CancelJoinRoom() => gameObject.SetActive(false);
    private void JoinRoom() => OnJoinRoom?.Invoke(_ipAdressInputField.text);
    public void Connecting() => _errorMessage.text = "Connecting...";
    public void FailedToConnect() => _errorMessage.text = "Failed! Check if Server or Host is active!";
}
