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

    private List<ConveyorBeltItem> _items = new List<ConveyorBeltItem>();
    private float spawnTimer = 0f;

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
        if (itemPrefabs.Count == 0) return;

        GameObject randomPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
        GameObject newItemObj = Instantiate(randomPrefab, _lineRenderer.GetPosition(0), Quaternion.identity, itemParent);

        ConveyorBeltItem newItem = new ConveyorBeltItem
        {
            item = newItemObj.transform,
            currentLerp = 0f,
            StartPoint = 0
        };

        _items.Add(newItem);
    }


    private void MoveItems()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            ConveyorBeltItem beltItem = _items[i];
            Transform item = beltItem.item;

            // Jaga jarak antar item
            if (i > 0)
            {
                float distToPrev = Vector3.Distance(item.position, _items[i - 1].item.position);
                if (distToPrev <= itemSpacing)
                    continue;
            }

            // Gerakkan item di sepanjang segment LineRenderer
            Vector3 start = _lineRenderer.GetPosition(beltItem.StartPoint);
            Vector3 end = _lineRenderer.GetPosition(beltItem.StartPoint + 1);

            item.position = Vector3.Lerp(start, end, beltItem.currentLerp);

            float segmentDistance = Vector3.Distance(start, end);
            beltItem.currentLerp += (speed * Time.deltaTime) / segmentDistance;

            // Jika item sudah mencapai ujung segment
            if (beltItem.currentLerp >= 1f)
            {
                if (beltItem.StartPoint + 2 < _lineRenderer.positionCount)
                {
                    beltItem.currentLerp = 0f;
                    beltItem.StartPoint++;
                }
                else
                {
                    // Hapus item di ujung conveyor
                    Destroy(item.gameObject);
                    _items.RemoveAt(i);
                    i--; // supaya loop tidak skip item selanjutnya
                }
            }
        }
    }
}
