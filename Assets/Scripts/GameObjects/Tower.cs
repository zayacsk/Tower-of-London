using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private readonly List<Ring> rings = new List<Ring>();

    [Header("Tower Settings")]
    [SerializeField] private float fixedXPosition = 0f;
    public float FixedXPosition => fixedXPosition;

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
        float currentHeight = 0f; // Текущая «вершина» башни, от которой будем ставить следующее кольцо

        for (int i = 0; i < rings.Count; i++)
        {
            Ring ring = rings[i];
            Renderer rend = ring.GetComponent<Renderer>();
            if (rend == null)
            {
                Debug.LogWarning($"У кольца {ring.name} нет Renderer!");
                continue;
            }

            // Полная высота кольца по оси Y
            float ringFullHeight = rend.bounds.size.y;
            // Половина высоты кольца — чтобы учесть смещение pivot (если он в центре)
            float halfHeight = ringFullHeight / 2f;

            // Ставим кольцо так, чтобы его нижняя грань оказалась на currentHeight
            // => центр кольца будет на (currentHeight + halfHeight)
            float ringYPosition = currentHeight + halfHeight;

            ring.transform.position = new Vector3(
                fixedXPosition,
                ringYPosition,
                transform.position.z
            );

            // Сдвигаем «вершину» на полную высоту кольца, чтобы следующее кольцо встало выше
            currentHeight += ringFullHeight;
        }
    }

    public int RingsCount()
    {
        return rings.Count;
    }

    public void ClearRings()
    {
        // Создаем временный список для хранения колец, которые нужно удалить
        List<Ring> ringsToRemove = new List<Ring>(rings);

        // Очищаем основной список колец
        rings.Clear();

        // Удаляем каждый объект кольца
        foreach (Ring ring in ringsToRemove)
        {
            if (ring != null)
            {
                Destroy(ring.gameObject);
            }
        }
    }
}