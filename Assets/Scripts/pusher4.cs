using UnityEngine;

public class Pusher4 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ItemKuning"))
        {
            Destroy(other.gameObject);
        }
    }
}
