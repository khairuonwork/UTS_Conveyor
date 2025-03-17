using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField] private float yScaleMultiplier = 5f; // Nilai pengali untuk meningkatkan skala sumbu Y
    private Vector3 originalScale; // Menyimpan skala asli objek

    void Start()
    {
        // Simpan skala asli objek saat game dimulai
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Jika tombol Space ditekan, tingkatkan skala sumbu Y
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 newScale = originalScale;
            newScale.y *= yScaleMultiplier;
            transform.localScale = newScale;
        }

        // Jika tombol Space dilepas, kembalikan skala ke ukuran normal
        if (Input.GetKeyUp(KeyCode.Space))
        {
            transform.localScale = originalScale;
        }
    }
}
