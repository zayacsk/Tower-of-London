using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public delegate void RingClickEvent(Ring ring);
    public event RingClickEvent OnRingClicked;

    public delegate void TowerClickEvent(Tower tower);
    public event TowerClickEvent OnTowerClicked;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("������� ������ �� �������! ���������, ��� � ������ ���������� ��� MainCamera.");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ProcessMouseClick();
        }
    }

    // ��������� Raycast � ����������, �� ������ ������� ��� �������� ����
    private void ProcessMouseClick()
    {
        if (cam == null)
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Ring"))
            {
                Ring ring = hit.collider.GetComponentInParent<Ring>();
                if (ring != null)
                {
                    OnRingClicked?.Invoke(ring);
                }
            }
            else if (hit.collider.CompareTag("Tower"))
            {
                Tower tower = hit.collider.GetComponent<Tower>();
                if (tower != null)
                {
                    OnTowerClicked?.Invoke(tower);
                }
            }
        }
    }
}