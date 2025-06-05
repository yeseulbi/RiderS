using UnityEngine;

public class Coin : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private AudioSource audioSource;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    // Ʈ���ſ� ������ �� ȣ��Ǵ� �޼���
    private void OnTriggerEnter2D(Collider2D other)
    {
        particleSystem.Play();
        audioSource.Play();

        Destroy(gameObject, 0.5f);
    }
}
