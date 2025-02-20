using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private LevelCreator levelCreator;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private Tower destinationTower;
    [SerializeField] private UIManager uiManager;

    [Header("Gameplay")]
    [SerializeField] private int moveCounter = 0;

    public int MoveCounter => moveCounter; // Только для чтения
    public int MaxMoves { get; set; }  // Значение устанавливается из LevelCreator

    private Ring selectedRing;

    private void Awake()
    {
        // Реализация паттерна Singleton для GameManager
        if (Instance == null)
        {
            Instance = this;
            // Если хотите, чтобы объект сохранялся между сценами:
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (inputHandler != null)
        {
            inputHandler.OnRingClicked += HandleRingClicked;
            inputHandler.OnTowerClicked += HandleTowerClicked;
        }
        else
        {
            Debug.LogError("InputHandler не назначен в GameManager!");
        }
    }

    private void HandleRingClicked(Ring ring)
    {
        if (ring.IsTopRing())
        {
            selectedRing = ring;
            Debug.Log("Выбрано кольцо с id: " + ring.Id);
        }
        else
        {
            Debug.Log("Кольцо не является верхним на своей башне.");
        }
    }

    private void HandleTowerClicked(Tower tower)
    {
        if (selectedRing != null)
        {
            Tower currentTower = selectedRing.GetCurrentTower();
            if (currentTower == tower)
            {
                Debug.Log("Выбранное кольцо уже находится на этой башне.");
                selectedRing = null;
                return;
            }
            else if (tower.CanPlaceRing(selectedRing))
            {
                Debug.Log($"Перемещаем кольцо {selectedRing.Id} с {currentTower.gameObject.name} на {tower.gameObject.name}");
                if (currentTower != null)
                {
                    currentTower.RemoveTopRing();
                }
                tower.AddRing(selectedRing);
                selectedRing.GetComponent<Ring>().SetHighlight(false);
                selectedRing = null;

                moveCounter++;
                Debug.Log("Ход: " + moveCounter);

                // Обновляем счётчик ходов в UI
                if (uiManager != null && !uiManager.IsStartImageActive())
                {
                    uiManager.UpdateMoveCounter(moveCounter);
                    uiManager.RestartGameBtn.SetActive(true);
                }

                CheckGameStatus();
            }
            else
            {
                Debug.Log($"Нельзя поместить кольцо {selectedRing.Id} на башню {tower.gameObject.name}");
                selectedRing = null;
            }
        }
        else
        {
            Debug.Log("Кольцо для перемещения не выбрано.");
        }
    }

    // Проверяет, достиг ли игрок условия победы или превысил лимит ходов
    private void CheckGameStatus()
    {
        if (destinationTower != null && destinationTower.RingsCount() == levelCreator.TotalRings)
        {
            // Сохраняем результат только при победе
            SaveManager.SaveLevelResult(levelCreator.CurrentLevel, moveCounter);

            Debug.Log("Поздравляем, вы выиграли! Всего ходов: " + moveCounter);
            uiManager.ShowVictory();
        }
        else if (moveCounter >= MaxMoves)
        {
            Debug.Log("Превышено максимальное количество ходов. Игра окончена.");
            uiManager.ShowGameOver();
        }
    }

    public void MenuBtn()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetGame()
    {
        moveCounter = 0;
        if (uiManager != null)
        {
            uiManager.UpdateMoveCounter(moveCounter);
        }
    }
}