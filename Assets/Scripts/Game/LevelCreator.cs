using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [Header("Ring Settings")]
    // Массив префабов колец — для каждого уровня вы должны предоставить нужное количество префабов
    [SerializeField] private GameObject[] ringPrefabs;
    [SerializeField] private int totalRings = 3;
    public int TotalRings
    {
        get { return totalRings; }
    }

    [Header("Level Settings")]
    [SerializeField] private LevelId currentLevel;
    public LevelId CurrentLevel => currentLevel;
    public enum LevelId
    {
        Level1 = 1,
        Level2 = 2,
        Level3 = 3
    }

    [SerializeField] private float startTowerXPosition = -2.5f;

    [Header("Tower Settings")]
    [SerializeField] private Tower[] towers;

    // Локальное значение максимального количества ходов для уровня
    private int maxMoves;

    private UIManager uiManager;
    private GameManager gameManager;

    private void Awake()
    {
        // Если башни не заданы через инспектор, ищем их в сцене один раз
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

        // Устанавливаем идентификатор уровня и maxMoves в зависимости от количества колец
        switch (totalRings)
        {
            case 3:
                currentLevel = LevelId.Level1;
                maxMoves = 12;
                break;
            case 4:
                currentLevel = LevelId.Level2;
                maxMoves = 27;
                break;
            case 5:
                currentLevel = LevelId.Level3;
                maxMoves = 55;
                break;
            default:
                Debug.LogWarning("Неподдерживаемое количество колец, устанавливаем значения по умолчанию.");
                currentLevel = LevelId.Level1;
                totalRings = 3;
                maxMoves = 7;
                break;
        }

        // Обновляем UI для максимального количества ходов
        uiManager.UpdateMaxMoves(maxMoves);

        // Передаём значение maxMoves в GameManager
        if (gameManager != null)
        {
            gameManager.MaxMoves = maxMoves;
        }

        CreateLevel();

        uiManager.SetStartImageActive(false);
    }

    // Создаёт уровень, добавляя кольца на стартовую башню
    public void CreateLevel()
    {
        foreach (Tower tower in towers)
        {
            tower.ClearRings();
        }

        Tower startTower = GetStartTower();
        if (startTower == null)
        {
            Debug.LogError($"Стартовая башня с fixedXPosition = {startTowerXPosition} не найдена!");
            return;
        }

        // Проверяем, что в массиве префабов достаточно объектов для текущего уровня
        if (ringPrefabs == null || ringPrefabs.Length < totalRings)
        {
            Debug.LogError("Недостаточно префабов колец для данного уровня!");
            return;
        }

        // Инстанцируем кольца в порядке, соответствующем их порядку в массиве
        // Предполагается, что ringPrefabs[0] – самое маленькое кольцо, а ringPrefabs[totalRings-1] – самое большое
        for (int i = totalRings - 1; i >= 0; i--)
        {
            GameObject ringObj = Instantiate(ringPrefabs[i]);
            ringObj.tag = "Ring";
            Ring ring = ringObj.GetComponent<Ring>();
            if (ring == null)
            {
                Debug.LogError("Префаб кольца не содержит компонент Ring!");
                return;
            }
            // Предполагается, что каждый префаб уже имеет заданный id, размер и цвет

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