using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static int myCoin; // ���� ���� ����

    int coinCount;  // ȹ�� ���� ����
    public int TotalTurn;

    public void AddCoin(int amount)
    {
        Debug.Log($"���� ȹ��: {amount}��, ���� ����: {coinCount}��");
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
                Time.timeScale = 1f; // ���� �簳
            }
            else
            {
                UIManager.Instance.ESCPanel.SetActive(true);
                Time.timeScale = 0f; // ���� �Ͻ� ����
            }
        }
    }

    public void GameStop()
    {
        //1. ���� ����
        Time.timeScale = 0f;

        //2. current, fast �ð� ����
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

        //3. �г� Ȱ��ȭ 
        UIManager.Instance.ShowPanel();

        if(MyCarController.Instance.rotateCount>TotalTurn)
            TotalTurn = MyCarController.Instance.rotateCount;   // �ְ� ��� ����
        UIManager.Instance.RotateCount.gameObject.SetActive(false);
        myCoin += coinCount; // ���� ���� ���� ����
        coinCount = 0; // ���� ȹ�� ���� �ʱ�ȭ
    }

    public void GameRestart()
    {
        //3. ���� �����
        Time.timeScale = 1f;

        //4. �� �ε�
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

        //1. UI �ʱ�ȭ
        elapsedTime = 0f;

        //2. �г� ��Ȱ��ȭ
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
