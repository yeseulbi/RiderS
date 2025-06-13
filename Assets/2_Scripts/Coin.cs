using UnityEngine;

public class Coin : MonoBehaviour
{
    private ParticleSystem particlesystem;
    private AudioSource audioSource;

    public static int myCoin; // 보유 코인 개수

    private void Awake()
    { 

    }
    void Start()
    {
        particlesystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    // 트리거에 들어왔을 때 호출되는 메서드
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return; // 플레이어가 아닌 경우 무시
        particlesystem.Play();
        audioSource.Play();
        GameManager.Instance.AddCoin(1); // 코인 추가

        Destroy(gameObject, 0.5f);
    }
}
