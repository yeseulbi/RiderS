using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        UIManager.Instance.UpdateTimeText(FormatElapsedTime(elapsedTime));
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
            Coin.Inst.coinCount,
            MyCarController.Instance.rotateCount);

        //3. 패널 활성화 
        UIManager.Instance.ShowPanel();

        UIManager.Instance.RotateCount.gameObject.SetActive(false);
        Coin.myCoin =+ Coin.Inst.coinCount; // 현재 코인 개수 저장
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
