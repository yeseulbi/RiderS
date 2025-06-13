using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Text timeText;
    public Text surfaceSpeedText;
    public Text carSpeedText;
    public Text currentTimeText;
    public Text fastTimeText;
    public Text coinText, totalCoin;

    public Text RotateCount, totalTurn;
    public GameObject panel;

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

    public void UpdateTotal(string crr_time, string fst_time, int coin, int turn)
    {
        currentTimeText.text = crr_time;
        fastTimeText.text = fst_time;
        totalCoin.text = coin + " Coin";
        totalTurn.text = turn + " Turn";
    }

    public void UpdateCoinText(int coinCount)
    {
        coinText.text = coinCount + " Coin";
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
}
