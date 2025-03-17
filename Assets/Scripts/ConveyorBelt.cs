using UnityEngine;
using System.Collections.Generic;

// Pindahkan class ini ke luar class ConveyorBelt
[System.Serializable]
public class ConveyorBeltItem
{
    public Transform item;
    [HideInInspector] public float currentLerp;
    [HideInInspector] public int StartPoint;
}

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private float itemSpacing;
    [SerializeField] private float speed;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private List<ConveyorBeltItem> _items;

    private void FixedUpdate()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            ConveyorBeltItem beltItem = _items[i];
            Transform item = beltItem.item;

            if (i > 0)
            {
                if (Vector3.Distance(item.position, _items[i - 1].item.position) <= itemSpacing)
                {
                    continue;
                }
            }

            item.position = Vector3.Lerp(_lineRenderer.GetPosition(beltItem.StartPoint),
                                         _lineRenderer.GetPosition(beltItem.StartPoint + 1),
                                         beltItem.currentLerp);

            float distance = Vector3.Distance(_lineRenderer.GetPosition(beltItem.StartPoint),
                                                _lineRenderer.GetPosition(beltItem.StartPoint + 1));
            beltItem.currentLerp += (speed * Time.deltaTime) / distance;

            if (beltItem.currentLerp >= 1) // end of line segment
            {
                if (beltItem.StartPoint + 2 < _lineRenderer.positionCount)
                {
                    beltItem.currentLerp = 0;
                    beltItem.StartPoint++;
                }
            }
        }
    }
}
