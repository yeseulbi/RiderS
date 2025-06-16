using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Text coinText, totalText;
    int myCoin => GameManager.myCoin;
    int totalTurns => GameManager.TotalTurn;

    void Start()
    {
        coinText.text = "보유 코인: "+ myCoin;
        totalText.text = "최고 기록: " + totalTurns;

    }

    void Update()
    {
        if("보유 코인: "+myCoin.ToString()!= coinText.text)
        {
            coinText.text = "보유 코인: " + myCoin;
        }
        if("최고 기록: " + totalTurns.ToString() != totalText.text)
        {
            totalText.text = "최고 기록: " + totalTurns;
        }
    }
    public void GoButton()
    {
        SceneManager.LoadScene("PlayScene");
        Time.timeScale = 1f; // 게임 재개
        gameObject.SetActive(false);
    }
}