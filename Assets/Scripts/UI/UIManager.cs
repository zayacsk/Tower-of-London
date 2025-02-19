using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text moveCounterText;
    [SerializeField] private Text maxMovesText;
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private GameObject finishImage;

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
}