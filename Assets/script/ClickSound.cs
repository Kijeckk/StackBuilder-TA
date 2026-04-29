using UnityEngine;

public class ClickSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // klik kiri / tap layar
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}