using UnityEngine;

public class Pusher3 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ItemHijau"))
        {
            Destroy(other.gameObject);
        }
    }
}
