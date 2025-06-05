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
        //1. ���� ����
        Time.timeScale = 0f;

        //2. current, fast �ð� ����
        if (elapsedTime < fatestTime)
        {
            fatestTime = elapsedTime;
            Debug.Log("New fastest time: " + FormatElapsedTime(fatestTime));
        }
        UIManager.Instance.UpdateCurrentTimeText("Current Time : " + FormatElapsedTime(elapsedTime));
        UIManager.Instance.UpdateFastTimeText("Fastest Time : " + FormatElapsedTime(fatestTime));

        //3. �г� Ȱ��ȭ 
        UIManager.Instance.ShowPanel();
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
