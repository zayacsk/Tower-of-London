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

    public int MoveCounter => moveCounter; // ������ ��� ������
    public int MaxMoves { get; set; }  // �������� ��������������� �� LevelCreator

    private Ring selectedRing;

    private void Awake()
    {
        // ���������� �������� Singleton ��� GameManager
        if (Instance == null)
        {
            Instance = this;
            // ���� ������, ����� ������ ���������� ����� �������:
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
            Debug.LogError("InputHandler �� �������� � GameManager!");
        }
    }

    private void HandleRingClicked(Ring ring)
    {
        if (ring.IsTopRing())
        {
            selectedRing = ring;
            Debug.Log("������� ������ � id: " + ring.Id);
        }
        else
        {
            Debug.Log("������ �� �������� ������� �� ����� �����.");
        }
    }

    private void HandleTowerClicked(Tower tower)
    {
        if (selectedRing != null)
        {
            Tower currentTower = selectedRing.GetCurrentTower();
            if (currentTower == tower)
            {
                Debug.Log("��������� ������ ��� ��������� �� ���� �����.");
                selectedRing = null;
                return;
            }
            else if (tower.CanPlaceRing(selectedRing))
            {
                Debug.Log($"���������� ������ {selectedRing.Id} � {currentTower.gameObject.name} �� {tower.gameObject.name}");
                if (currentTower != null)
                {
                    currentTower.RemoveTopRing();
                }
                tower.AddRing(selectedRing);
                selectedRing.GetComponent<Ring>().SetHighlight(false);
                selectedRing = null;

                moveCounter++;
                Debug.Log("���: " + moveCounter);

                // ��������� ������� ����� � UI
                if (uiManager != null && !uiManager.IsStartImageActive())
                {
                    uiManager.UpdateMoveCounter(moveCounter);
                    uiManager.RestartGameBtn.SetActive(true);
                }

                CheckGameStatus();
            }
            else
            {
                Debug.Log($"������ ��������� ������ {selectedRing.Id} �� ����� {tower.gameObject.name}");
                selectedRing = null;
            }
        }
        else
        {
            Debug.Log("������ ��� ����������� �� �������.");
        }
    }

    // ���������, ������ �� ����� ������� ������ ��� �������� ����� �����
    private void CheckGameStatus()
    {
        if (destinationTower != null && destinationTower.RingsCount() == levelCreator.TotalRings)
        {
            // ��������� ��������� ������ ��� ������
            SaveManager.SaveLevelResult(levelCreator.CurrentLevel, moveCounter);

            Debug.Log("�����������, �� ��������! ����� �����: " + moveCounter);
            uiManager.ShowVictory();
        }
        else if (moveCounter >= MaxMoves)
        {
            Debug.Log("��������� ������������ ���������� �����. ���� ��������.");
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