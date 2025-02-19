using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCreator : MonoBehaviour
{
    [Header("Ring Settings")]
    [SerializeField] private GameObject ringPrefab;
    [SerializeField] private Color[] ringColors;
    [SerializeField] private int totalRings = 3;
    public int TotalRings
    {
        get { return totalRings; }
    }
    [SerializeField] private float startTowerXPosition = -2.5f;

    [Header("Tower Settings")]
    [SerializeField] private Tower[] towers;

    [Header("Canvas Setup")]
    [SerializeField] private GameObject startImage;

    // Локальное значение максимального количества ходов для уровня
    private int maxMoves;

    private UIManager uiManager;
    private GameManager gameManager;

    private void Awake()
    {
        if (towers == null || towers.Length == 0)
        {
            towers = GameObject.FindObjectsOfType<Tower>();
        }

        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager не найден на сцене!");
        }

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager не найден на сцене!");
        }
    }

    // Устанавливает уровень, выбирая количество колец и максимальное количество ходов, затем создаёт уровень
    public void SetLevel(int ringCount)
    {
        totalRings = ringCount;

        // Определяем maxMoves в зависимости от количества колец.
        switch (totalRings)
        {
            case 3:
                maxMoves = 10;
                break;
            case 4:
                maxMoves = 22;
                break;
            case 5:
                maxMoves = 55;
                break;
            default:
                Debug.LogWarning("Неподдерживаемое количество колец, устанавливаем значения по умолчанию.");
                totalRings = 3;
                maxMoves = 7;
                break;
        }

        // Обновляем UI для максимального количества ходов.
        uiManager?.UpdateMaxMoves(maxMoves);

        // Передаём значение максимального количества ходов в GameManager.
        if (gameManager != null)
        {
            gameManager.MaxMoves = maxMoves;
        }

        CreateLevel();
        if (startImage != null)
        {
            startImage.SetActive(false);
        }
    }

    // Создаёт уровень, добавляя кольца на стартовую башню
    public void CreateLevel()
    {
        Tower startTower = GetStartTower();
        if (startTower == null)
        {
            Debug.LogError($"Стартовая башня с fixedXPosition = {startTowerXPosition} не найдена!");
            return;
        }

        // Создаем кольца от большего к меньшему (большие кольца располагаются снизу)
        for (int i = totalRings - 1; i >= 0; i--)
        {
            GameObject ringObj = Instantiate(ringPrefab);
            ringObj.tag = "Ring";
            Ring ring = ringObj.GetComponent<Ring>();
            if (ring == null)
            {
                Debug.LogError("Префаб кольца не содержит компонент Ring!");
                return;
            }
            ring.Id = i;

            float scaleFactor = 1.0f + i * 0.3f;
            ringObj.transform.localScale = new Vector3(scaleFactor, ringObj.transform.localScale.y, scaleFactor);

            // Задаем цвет кольца, если массив цветов заполнен
            if (ringColors != null && ringColors.Length > 0)
            {
                Renderer rend = ringObj.GetComponent<Renderer>();
                if (rend != null)
                {
                    // Используем индекс кольца по модулю длины массива, чтобы цвета повторялись, если колец больше, чем цветов.
                    rend.material.color = ringColors[i % ringColors.Length];
                }
            }

            startTower.AddRing(ring);
        }
    }

    // Ищет и возвращает стартовую башню по фиксированной позиции по оси X
    private Tower GetStartTower()
    {
        foreach (Tower tower in towers)
        {
            if (Mathf.Approximately(tower.FixedXPosition, startTowerXPosition))
            {
                return tower;
            }
        }
        return null;
    }
}