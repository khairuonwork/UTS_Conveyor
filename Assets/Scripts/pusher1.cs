using UnityEngine;

public class Pusher1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ItemMerah"))
        {
            Destroy(other.gameObject);
        }
    }
}
