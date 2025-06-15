using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static int myCoin; // 보유 코인 개수

    int coinCount;  // 획득 코인 개수
    public int TotalTurn;

    public void AddCoin(int amount)
    {
        Debug.Log($"코인 획득: {amount}개, 현재 코인: {coinCount}개");
        coinCount += amount;
        UIManager.Instance.UpdateCoinText(coinCount);
    }

    private float elapsedTime = 0f;
    private float fatestTime = float.MaxValue;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        //TotalTurn = 50;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        UIManager.Instance.UpdateTimeText(FormatElapsedTime(elapsedTime));

        if(Input.GetKeyDown(KeyCode.Escape))
        { 
            if (UIManager.Instance.ESCPanel.activeSelf)
            {
                UIManager.Instance.ESCPanel.SetActive(false);
                Time.timeScale = 1f; // 게임 재개
            }
            else
            {
                UIManager.Instance.ESCPanel.SetActive(true);
                Time.timeScale = 0f; // 게임 일시 정지
            }
        }
    }

    public void GameStop()
    {
        //1. 게임 중지
        Time.timeScale = 0f;

        //2. current, fast 시간 저장
        if (elapsedTime < fatestTime)
        {
            fatestTime = elapsedTime;
            Debug.Log("New fastest time: " + FormatElapsedTime(fatestTime));
        }
        UIManager.Instance.UpdateTotal
            ("Current Time : " + FormatElapsedTime(elapsedTime),
            "Fastest Time : " + FormatElapsedTime(fatestTime),
            myCoin,
            coinCount,
            MyCarController.Instance.rotateCount);

        //3. 패널 활성화 
        UIManager.Instance.ShowPanel();

        if(MyCarController.Instance.rotateCount>TotalTurn)
            TotalTurn = MyCarController.Instance.rotateCount;   // 최고 기록 저장
        UIManager.Instance.RotateCount.gameObject.SetActive(false);
        myCoin += coinCount; // 현재 코인 개수 저장
        coinCount = 0; // 코인 획득 개수 초기화
    }

    public void GameRestart()
    {
        //3. 게임 재시작
        Time.timeScale = 1f;

        //4. 씬 로드
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

        //1. UI 초기화
        elapsedTime = 0f;

        //2. 패널 비활성화
        UIManager.Instance.HidePanel();
    }

    private string FormatElapsedTime(float time)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);
        int milliseconds = (int)((time * 1000) % 1000);
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

}
