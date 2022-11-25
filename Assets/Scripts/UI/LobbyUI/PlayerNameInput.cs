using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField _playerNameInput;
    [SerializeField] private TextMeshProUGUI _errorDisplay;
    [SerializeField] private Button _buttonConfirm;

    private const string _playerPerfsNameKey = "PlayerName";

    public static string DisplayName { get; private set; }

    public Action<bool> OnConfirmName;

    private void Start()
    {
        _buttonConfirm.interactable = false;
        _playerNameInput.onValueChanged.AddListener( ValidateInput );
        _buttonConfirm.onClick.AddListener( ConfirmName );

        if(PlayerPrefs.HasKey(_playerPerfsNameKey)) _playerNameInput.text = PlayerPrefs.GetString(_playerPerfsNameKey);
    }

    private void ValidateInput(string playerName)
    {
        bool isValidName = !string.IsNullOrEmpty(playerName) && !playerName.StartsWith(' ');
        _buttonConfirm.interactable = isValidName;
        _errorDisplay.text = (isValidName) ? "" : "Please, enter correct name!";
    }

    private void ConfirmName()
    {
        DisplayName = _playerNameInput.text;
        PlayerPrefs.SetString(_playerPerfsNameKey, _playerNameInput.text);
        OnConfirmName?.Invoke(true);
    }

    private void OnDestroy()
    {
        _buttonConfirm.onClick.RemoveAllListeners();
        _playerNameInput.onValueChanged.RemoveAllListeners();
    }
}
