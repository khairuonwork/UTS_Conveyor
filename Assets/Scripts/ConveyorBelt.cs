using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ConveyorBeltItem
{
    public Transform item;
    [HideInInspector] public float currentLerp;
    [HideInInspector] public int StartPoint;
}

public class ConveyorBelt : MonoBehaviour
{
    [Header("Conveyor Settings")]
    [SerializeField] private float itemSpacing = 1f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private LineRenderer _lineRenderer;

    [Header("Spawn Settings")]
    [SerializeField] private List<GameObject> itemPrefabs;
    [SerializeField] private Transform itemParent;
    [SerializeField] private float spawnInterval = 2f;

    private readonly List<ConveyorBeltItem> _items = new();
    private float spawnTimer = 0f;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            SpawnNewItem();
        }
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnNewItem();
        }

        MoveItems();
    }

    private void SpawnNewItem()
    {
        if (itemPrefabs.Count == 0 || _lineRenderer.positionCount < 2)
            return;

        GameObject prefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
        GameObject newItem = Instantiate(prefab, _lineRenderer.GetPosition(0), Quaternion.identity, itemParent);

        _items.Add(new ConveyorBeltItem
        {
            item = newItem.transform,
            currentLerp = 0f,
            StartPoint = 0
        });
    }

    private void MoveItems()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            ConveyorBeltItem current = _items[i];

            // Skip null/destroyed item
            if (current.item == null)
            {
                _items.RemoveAt(i);
                i--;
                continue;
            }

            // Prevent moving if spacing to previous is too small
            if (i > 0)
            {
                ConveyorBeltItem previous = _items[i - 1];
                if (previous?.item != null)
                {
                    float dist = (current.item.position - previous.item.position).sqrMagnitude;
                    if (dist < itemSpacing * itemSpacing)
                        continue;
                }
            }

            // Get start and end points
            if (current.StartPoint + 1 >= _lineRenderer.positionCount) continue;

            Vector3 start = _lineRenderer.GetPosition(current.StartPoint);
            Vector3 end = _lineRenderer.GetPosition(current.StartPoint + 1);

            float segmentLength = Vector3.Distance(start, end);
            if (segmentLength <= 0.001f) continue;

            // Update position
            current.currentLerp += (speed * Time.deltaTime) / segmentLength;
            current.currentLerp = Mathf.Clamp01(current.currentLerp); // avoid overshoot

            current.item.position = Vector3.Lerp(start, end, current.currentLerp);

            // Transition to next segment
            if (current.currentLerp >= 1f)
            {
                if (current.StartPoint + 2 < _lineRenderer.positionCount)
                {
                    current.StartPoint++;
                    current.currentLerp = 0f;
                }
                else
                {
                    // Reached end
                    Destroy(current.item.gameObject);
                    _items.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
