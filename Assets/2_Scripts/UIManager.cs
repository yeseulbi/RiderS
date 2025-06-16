using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Text")]
    public Text timeText;
    public Text surfaceSpeedText;
    public Text carSpeedText;
    public Text currentTimeText;
    public Text fastTimeText;

    public Text coinText, totalCoin, takeCoin;
    public Text RotateCount, totalTurn;

    [Header("Panel")]
    public GameObject panel;
    public GameObject ESCPanel;
    public GameObject SettingPanel;

    [Header("Animator")]
    public Animator RotateCount_Animator;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void UpdateRotateCount(int count)
    {
        RotateCount.text = count.ToString();
        RotateCount_Animator.SetTrigger("UpdateRotateCount");
    }
    public void UpdateTimeText(string time)
    {
        timeText.text = time;
    }

    public void UpdateSurfaceText(string speed)
    {
        surfaceSpeedText.text = speed;
    }

    public void UpdateCarSpeedText(string speed)
    {
        carSpeedText.text = speed;
    }

    public void UpdateTotal(string crr_time, string fst_time, int coin, int TakeCoin, int turn)
    {
        currentTimeText.text = crr_time;
        fastTimeText.text = fst_time;
        totalCoin.text = coin + " Coin";
        takeCoin.text = "+"+ TakeCoin + " Coin";
        totalTurn.text = turn + " Turn";
    }

    public void UpdateCoinText(int coin)
    {
        coinText.text = coin + " Coin";
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
    }

    public void HidePanel()
    {
        panel.SetActive(false);
    }

    public void GameRestart()
    {
        GameManager.Instance.GameRestart();
    }
    public void GameContinue()
    {
        ESCPanel.SetActive(false);
        Time.timeScale = 1f; // 게임 재개
    }
    public void GameSetting()
    {
        SettingPanel.SetActive(true);
    }
    public void GameShop()
    {
        SceneManager.LoadScene("Shop");
        if (Shop.Instance != null)
        {
            Shop.Instance.ShopObject.SetActive(true);
        }
    }
    public void GameExit()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }
}