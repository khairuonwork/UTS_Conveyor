using UnityEngine;

public class Scaler3 : MonoBehaviour
{
    [SerializeField] private float yScaleMultiplier = 5f;
    private Vector3 originalScale;
    private BoxCollider2D boxCollider;

    void Start()
    {
        originalScale = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Vector3 newScale = originalScale;
            newScale.y *= yScaleMultiplier;
            transform.localScale = newScale;

            // Refresh collider
            boxCollider.enabled = false;
            boxCollider.enabled = true;
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            transform.localScale = originalScale;

            // Refresh collider lagi setelah kembali
            boxCollider.enabled = false;
            boxCollider.enabled = true;
        }
    }
}
