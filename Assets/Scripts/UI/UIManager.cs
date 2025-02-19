using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text moveCounterText;
    [SerializeField] private Text maxMovesText;
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private GameObject finishImage;

    // ��������� ����� �������� �����
    public void UpdateMoveCounter(int moves)
    {
        if (moveCounterText != null)
        {
            moveCounterText.text = "Steps: " + moves;
        }
    }

    // ��������� ����� ������������� ���������� �����
    public void UpdateMaxMoves(int maxMoves)
    {
        if (maxMovesText != null)
        {
            maxMovesText.text = "Max steps: " + maxMoves;
        }
    }

    // ���������� ����� ���������
    public void ShowGameOver()
    {
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(true);
        }
    }

    // ���������� ����� ������.
    public void ShowVictory()
    {
        if (finishImage != null)
        {
            finishImage.SetActive(true);
        }
    }

    // �������� ��� �������������� UI ��������
    public void HideAllUI()
    {
        if (gameOverImage != null)
            gameOverImage.SetActive(false);
        if (finishImage != null)
            finishImage.SetActive(false);
    }
}