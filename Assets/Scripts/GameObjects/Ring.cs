using UnityEngine;

public class Ring : MonoBehaviour
{
    [Header("Ring Settings")]
    [SerializeField] private int id;
    [SerializeField] private Material highlightedMaterial;

    private Material originalMaterial;
    private Renderer ringRenderer;

    public int Id { get => id; set => id = value; }

    private Tower currentTower;

    private void Awake()
    {
        ringRenderer = GetComponent<Renderer>();
        if (ringRenderer != null)
        {
            originalMaterial = ringRenderer.material;
        }
    }

    public bool IsTopRing()
    {
        if (currentTower == null)
        {
            Debug.LogWarning($"Ring {name} �� ����������� � �����.");
            return false;
        }
        return currentTower.GetTopRing() == this;
    }

    public void SetCurrentTower(Tower tower)
    {
        currentTower = tower;
    }

    public Tower GetCurrentTower()
    {
        return currentTower;
    }

    public void SetHighlight(bool isHighlighted)
    {
        if (ringRenderer == null) return;

        if (isHighlighted)
        {
            // �������� ������ ��������
            ringRenderer.material = highlightedMaterial;
        }
        else
        {
            // ���������� �������� ��������
            ringRenderer.material = originalMaterial;
        }
    }
}