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
        // Получаем все значения LevelId (например, Level1, Level2, Level3)
        LevelCreator.LevelId[] levels = (LevelCreator.LevelId[])Enum.GetValues(typeof(LevelCreator.LevelId));

        // Проверяем, что массив текстов содержит столько элементов, сколько уровней
        if (levelResultTexts.Length < levels.Length)
        {
            Debug.LogWarning("Количество UI элементов меньше, чем уровней.");
        }

        // Обновляем UI для каждого уровня в цикле
        for (int i = 0; i < levels.Length; i++)
        {
            int bestMoves = SaveManager.GetBestResult(levels[i]);
            if (i < levelResultTexts.Length && levelResultTexts[i] != null)
            {
                levelResultTexts[i].text = FormatResult(levels[i], bestMoves);
            }
        }
    }

    // Обновляет текст счетчика ходов
    public void UpdateMoveCounter(int moves)
    {
        if (moveCounterText != null)
        {
            moveCounterText.text = "Steps: " + moves;
        }
    }

    // Обновляет текст максимального количества ходов
    public void UpdateMaxMoves(int maxMoves)
    {
        if (maxMovesText != null)
        {
            maxMovesText.text = "Max steps: " + maxMoves;
        }
    }

    // Метод для управления отображением startImage
    public void SetStartImageActive(bool isActive)
    {
        if (startImage != null)
        {
            startImage.SetActive(isActive);
        }
        else
        {
            Debug.LogWarning("startImage не назначен в UIManager.");
        }
    }

    public bool IsStartImageActive()
    {
        return startImage != null && startImage.activeSelf;
    }

    // Отображает экран проигрыша
    public void ShowGameOver()
    {
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(true);
        }
    }

    // Отображает экран победы.
    public void ShowVictory()
    {
        if (finishImage != null)
        {
            finishImage.SetActive(true);
        }
    }

    // Скрывает все дополнительные UI элементы
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

    // Метод для перезапуска уровня
    public void RestartLevel()
    {
        if (levelCreator != null)
        {
            int currentRingCount = levelCreator.TotalRings;

            // Сбрасываем moveCounter через GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResetGame();
            }

            // Создаём уровень заново (сброс колец)
            levelCreator.SetLevel(currentRingCount);
        }
        else
        {
            Debug.LogError("LevelCreator не назначен в UIManager.");
        }
    }
}