using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoinRoomUIManager : MonoBehaviour
{
    private NetworkRoomManagerExtended _networkManager;

    [SerializeField] private MainMenuUIManager _mainMenuUIManager;

    [Header("UI elements")]
    [SerializeField] private TextMeshProUGUI _errorMessage;
    [SerializeField] private TMP_InputField _ipAdressInputField;
    [SerializeField] private Button _joinButton;
    [SerializeField] private Button _cancelButton;

    private void FailedToConnect() => _errorMessage.text = "Failed! Check if Server or Host is active!";

    private void Start()
    {
        _networkManager = FindObjectOfType<NetworkRoomManagerExtended>();
        _networkManager.OnFailedToConnect += FailedToConnect;
        _networkManager.OnClientEntered += CancelJoinRoom;

        _ipAdressInputField.onValueChanged.AddListener(ValidateIPAdress);
        _joinButton.onClick.AddListener(JoinRoom);
        _cancelButton.onClick.AddListener(CancelJoinRoom);
    }

    private void OnDestroy()
    {
        _ipAdressInputField.onValueChanged.RemoveAllListeners();
        _joinButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();

        _networkManager.OnFailedToConnect -= FailedToConnect;
        _networkManager.OnClientEntered -= CancelJoinRoom;
    }

    private void ValidateIPAdress(string ipAdress)
    {
        bool isIpAdressValid = !string.IsNullOrEmpty(ipAdress) && !ipAdress.StartsWith(' ');
        _joinButton.interactable = isIpAdressValid;
        _errorMessage.text = isIpAdressValid ? "" : "Please, enter correct ipAdress";
    }

    private void JoinRoom()
    {
        _networkManager.networkAddress = _ipAdressInputField.text;
        _networkManager.StartClient();
    }

    private void CancelJoinRoom()
    {
        _ipAdressInputField.text = "localhost";
        _errorMessage.text = "";
        gameObject.SetActive(false);
    }
}