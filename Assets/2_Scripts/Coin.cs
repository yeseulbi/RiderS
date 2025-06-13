using UnityEngine;

public class Coin : MonoBehaviour
{
    private ParticleSystem particlesystem;
    private AudioSource audioSource;

    public static Coin Inst; // �̱��� �ν��Ͻ�

    public int coinCount = 0;  // ȹ�� ���� ����
    public static int myCoin; // ���� ���� ����

    private void Awake()
    {
        Inst = this; // �̱��� �ν��Ͻ� ����
    }
    void Start()
    {
        particlesystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    // Ʈ���ſ� ������ �� ȣ��Ǵ� �޼���
    private void OnTriggerEnter2D(Collider2D other)
    {
        particlesystem.Play();
        audioSource.Play();
        AddCoin(1); // ���� ȹ�� ó��

        Destroy(gameObject, 0.5f);
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UIManager.Instance.UpdateCoinText(coinCount);
    }
}
