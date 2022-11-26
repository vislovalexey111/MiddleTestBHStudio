using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PlayerController))]
public class PlayerUIWrapper : MonoBehaviour
{
    private NetworkRoomManagerExtended _networkManager;

    [Header("Player Parameters")]
    [SerializeField] private int _rematchCountdown = 5;
    [SerializeField] private PlayerController _playerController;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _pointsViewer;
    [SerializeField] private TextMeshProUGUI _identityViewer;
    [SerializeField] private WinScreen _winScreen;

    private void Start()
    {
        _pointsViewer = FindObjectOfType<PointsViewer>().GetComponent<TextMeshProUGUI>();
        _identityViewer = FindObjectOfType<IdentityViewer>().GetComponent<TextMeshProUGUI>();
        _winScreen = FindObjectOfType<WinScreen>(true);
        _networkManager = FindObjectOfType<NetworkRoomManagerExtended>();

        _playerController.OnUpdatePoints += UpdatePoints;
        _playerController.OnWin += ShowWinScreen;

        _identityViewer.text = (_playerController.isServer) ? "Server" : "Client";
    }

    private void OnDestroy()
    {
        _playerController.OnUpdatePoints -= UpdatePoints;
        _playerController.OnWin -= ShowWinScreen;
    }

    private void ShowWinScreen(string winnerName)
    {
        _winScreen.gameObject.SetActive(true);
        _winScreen.SetWinnerName(winnerName);
        StartCoroutine(CountdownForRematch());
    }

    private IEnumerator CountdownForRematch()
    {
        for(int i = 0; i < _rematchCountdown; i++)
        {
            _winScreen.SetCountdown("" + (_rematchCountdown - i));
            yield return new WaitForSeconds(1);
        }

        _networkManager.ChangeScene();
    }

    private void UpdatePoints(int points)
    {
        if(_playerController.isLocalPlayer)
            _pointsViewer.text = points + " point(s)";
    }
}
