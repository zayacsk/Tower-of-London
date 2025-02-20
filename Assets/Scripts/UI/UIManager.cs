using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text moveCounterText;
    [SerializeField] private Text maxMovesText;
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private GameObject finishImage;
    [SerializeField] private GameObject startImage;

    public GameObject RestartGameBtn;

    [Header("Best Results UI")]
    [SerializeField] private Text[] levelResultTexts;

    [SerializeField] private LevelCreator levelCreator;

    private void Start()
    {
        // �������� ��� �������� LevelId (��������, Level1, Level2, Level3)
        LevelCreator.LevelId[] levels = (LevelCreator.LevelId[])Enum.GetValues(typeof(LevelCreator.LevelId));

        // ���������, ��� ������ ������� �������� ������� ���������, ������� �������
        if (levelResultTexts.Length < levels.Length)
        {
            Debug.LogWarning("���������� UI ��������� ������, ��� �������.");
        }

        // ��������� UI ��� ������� ������ � �����
        for (int i = 0; i < levels.Length; i++)
        {
            int bestMoves = SaveManager.GetBestResult(levels[i]);
            if (i < levelResultTexts.Length && levelResultTexts[i] != null)
            {
                levelResultTexts[i].text = FormatResult(levels[i], bestMoves);
            }
        }
    }

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

    // ����� ��� ���������� ������������ startImage
    public void SetStartImageActive(bool isActive)
    {
        if (startImage != null)
        {
            startImage.SetActive(isActive);
        }
        else
        {
            Debug.LogWarning("startImage �� �������� � UIManager.");
        }
    }

    public bool IsStartImageActive()
    {
        return startImage != null && startImage.activeSelf;
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

    private string FormatResult(LevelCreator.LevelId level, int bestMoves)
    {
        if (bestMoves == int.MaxValue)
        {
            return $"{level}: N/A";
        }
        else
        {
            return $"{level}: {bestMoves} moves";
        }
    }

    public void ResetData()
    {
        SaveManager.DeleteSaveData();
        SceneManager.LoadScene(0);
    }

    // ����� ��� ����������� ������
    public void RestartLevel()
    {
        if (levelCreator != null)
        {
            int currentRingCount = levelCreator.TotalRings;

            // ���������� moveCounter ����� GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResetGame();
            }

            // ������ ������� ������ (����� �����)
            levelCreator.SetLevel(currentRingCount);
        }
        else
        {
            Debug.LogError("LevelCreator �� �������� � UIManager.");
        }
    }
}