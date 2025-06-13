using UnityEngine;

public class Coin : MonoBehaviour
{
    private ParticleSystem particlesystem;
    private AudioSource audioSource;

    public static Coin Inst; // 싱글톤 인스턴스

    public int coinCount = 0;  // 획득 코인 개수
    public static int myCoin; // 보유 코인 개수

    private void Awake()
    {
        Inst = this; // 싱글톤 인스턴스 설정
    }
    void Start()
    {
        particlesystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    // 트리거에 들어왔을 때 호출되는 메서드
    private void OnTriggerEnter2D(Collider2D other)
    {
        particlesystem.Play();
        audioSource.Play();
        AddCoin(1); // 코인 획득 처리

        Destroy(gameObject, 0.5f);
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UIManager.Instance.UpdateCoinText(coinCount);
    }
}
