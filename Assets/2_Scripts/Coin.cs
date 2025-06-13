using UnityEngine;

public class Coin : MonoBehaviour
{
    private ParticleSystem particlesystem;
    private AudioSource audioSource;

    public static int myCoin; // ���� ���� ����

    private void Awake()
    { 

    }
    void Start()
    {
        particlesystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    // Ʈ���ſ� ������ �� ȣ��Ǵ� �޼���
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return; // �÷��̾ �ƴ� ��� ����
        particlesystem.Play();
        audioSource.Play();
        GameManager.Instance.AddCoin(1); // ���� �߰�

        Destroy(gameObject, 0.5f);
    }
}
