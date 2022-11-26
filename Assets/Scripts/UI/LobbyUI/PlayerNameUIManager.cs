using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNameUIManager : MonoBehaviour
{
    [SerializeField] private MainMenuUIManager _mainMenuUIManager;

    [Header("UI")]
    [SerializeField] private TMP_InputField _playerNameInputField;
    [SerializeField] private TextMeshProUGUI _errorDisplay;
    [SerializeField] private Button _buttonConfirm;

    private const string _playerPerfsNameKey = "PlayerName";

    public static string DisplayName { get; private set; }

    private void Start()
    {
        _buttonConfirm.interactable = false;
        _playerNameInputField.onValueChanged.AddListener( ValidateInput );
        _buttonConfirm.onClick.AddListener( ConfirmName );

        if (PlayerPrefs.HasKey(_playerPerfsNameKey)) _playerNameInputField.text = PlayerPrefs.GetString(_playerPerfsNameKey);
    }

    private void ValidateInput(string playerName)
    {
        bool isValidName = !string.IsNullOrEmpty(playerName) && !playerName.StartsWith(' ');
        _buttonConfirm.interactable = isValidName;
        _errorDisplay.text = (isValidName) ? "" : "Please, enter correct name!";
    }

    private void ConfirmName()
    {
        DisplayName = _playerNameInputField.text;
        PlayerPrefs.SetString(_playerPerfsNameKey, _playerNameInputField.text);
        _mainMenuUIManager.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _buttonConfirm.onClick.RemoveAllListeners();
        _playerNameInputField.onValueChanged.RemoveAllListeners();
    }
}
