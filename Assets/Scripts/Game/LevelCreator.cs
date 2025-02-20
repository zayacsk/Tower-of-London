using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [Header("Ring Settings")]
    // ������ �������� ����� � ��� ������� ������ �� ������ ������������ ������ ���������� ��������
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

    // ��������� �������� ������������� ���������� ����� ��� ������
    private int maxMoves;

    private UIManager uiManager;
    private GameManager gameManager;

    private void Awake()
    {
        // ���� ����� �� ������ ����� ���������, ���� �� � ����� ���� ���
        if (towers == null || towers.Length == 0)
        {
            towers = GameObject.FindObjectsOfType<Tower>();
        }

        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager �� ������ �� �����!");
        }

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager �� ������ �� �����!");
        }
    }

    // ������������� �������, ������� ���������� ����� � ������������ ���������� �����, ����� ������ �������
    public void SetLevel(int ringCount)
    {
        totalRings = ringCount;

        // ������������� ������������� ������ � maxMoves � ����������� �� ���������� �����
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
                Debug.LogWarning("���������������� ���������� �����, ������������� �������� �� ���������.");
                currentLevel = LevelId.Level1;
                totalRings = 3;
                maxMoves = 7;
                break;
        }

        // ��������� UI ��� ������������� ���������� �����
        uiManager.UpdateMaxMoves(maxMoves);

        // ������� �������� maxMoves � GameManager
        if (gameManager != null)
        {
            gameManager.MaxMoves = maxMoves;
        }

        CreateLevel();

        uiManager.SetStartImageActive(false);
    }

    // ������ �������, �������� ������ �� ��������� �����
    public void CreateLevel()
    {
        foreach (Tower tower in towers)
        {
            tower.ClearRings();
        }

        Tower startTower = GetStartTower();
        if (startTower == null)
        {
            Debug.LogError($"��������� ����� � fixedXPosition = {startTowerXPosition} �� �������!");
            return;
        }

        // ���������, ��� � ������� �������� ���������� �������� ��� �������� ������
        if (ringPrefabs == null || ringPrefabs.Length < totalRings)
        {
            Debug.LogError("������������ �������� ����� ��� ������� ������!");
            return;
        }

        // ������������ ������ � �������, ��������������� �� ������� � �������
        // ��������������, ��� ringPrefabs[0] � ����� ��������� ������, � ringPrefabs[totalRings-1] � ����� �������
        for (int i = totalRings - 1; i >= 0; i--)
        {
            GameObject ringObj = Instantiate(ringPrefabs[i]);
            ringObj.tag = "Ring";
            Ring ring = ringObj.GetComponent<Ring>();
            if (ring == null)
            {
                Debug.LogError("������ ������ �� �������� ��������� Ring!");
                return;
            }
            // ��������������, ��� ������ ������ ��� ����� �������� id, ������ � ����

            startTower.AddRing(ring);
        }
    }

    // ���� � ���������� ��������� ����� �� ������������� ������� �� ��� X
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