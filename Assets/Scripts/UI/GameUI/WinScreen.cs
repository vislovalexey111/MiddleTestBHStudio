using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _winnerName;
    [SerializeField] private TextMeshProUGUI _countDown;

    public void SetCountdown(string countDown) => _countDown.text = countDown;
    public void SetWinnerName(string winnerName) => _winnerName.text = winnerName;
}