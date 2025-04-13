using UnityEngine;

public class Pusher2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ItemBiru"))
        {
            Destroy(other.gameObject);
        }
    }
}
