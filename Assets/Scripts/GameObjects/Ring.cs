using UnityEngine;

public class Ring : MonoBehaviour
{
    [Header("Ring Settings")]
    [SerializeField] private int id;
    public int Id { get => id; set => id = value; }

    private Tower currentTower;

    public bool IsTopRing()
    {
        if (currentTower == null)
        {
            Debug.LogWarning($"Ring {name} не прикреплено к башне.");
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
}