using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private readonly List<Ring> rings = new List<Ring>();

    [Header("Tower Settings")]
    [SerializeField] private float fixedXPosition = 0f;
    public float FixedXPosition => fixedXPosition; // Публичное свойство для доступа

    [SerializeField] private float ringHeight = 0.5f;

    public void AddRing(Ring ring)
    {
        if (ring == null)
            return;

        rings.Add(ring);
        ring.SetCurrentTower(this);
        UpdateRingPositions();
    }

    public Ring RemoveTopRing()
    {
        if (rings.Count == 0)
            return null;

        Ring topRing = rings[rings.Count - 1];
        rings.RemoveAt(rings.Count - 1);
        topRing.SetCurrentTower(null);
        UpdateRingPositions();
        return topRing;
    }

    public Ring GetTopRing()
    {
        return rings.Count > 0 ? rings[rings.Count - 1] : null;
    }

    public bool CanPlaceRing(Ring ring)
    {
        if (ring == null)
            return false;
        Ring topRing = GetTopRing();
        return topRing == null || ring.Id < topRing.Id;
    }

    private void UpdateRingPositions()
    {
        for (int i = 0; i < rings.Count; i++)
        {
            Ring ring = rings[i];
            float ringYPosition = i * ringHeight + 0.3f;
            ring.transform.position = new Vector3(
                fixedXPosition,
                ringYPosition,
                transform.position.z
            );
        }
    }

    public int RingsCount()
    {
        return rings.Count;
    }
}