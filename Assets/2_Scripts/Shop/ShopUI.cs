using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Text coinText, totalText;
    int myCoin;

    void Start()
    {
        myCoin = GameManager.myCoin;

        coinText.text = "보유 코인: "+ myCoin;
        totalText.text = "최고 기록: " + GameManager.Instance.TotalTurn;
    }

    void Update()
    {
        if(myCoin!= GameManager.myCoin)
        {
            myCoin = GameManager.myCoin;
            coinText.text = "보유 코인: " + myCoin;
        }
    }
    public void GoButton()
    {
        SceneManager.LoadScene("PlayScene");
        Time.timeScale = 1f; // 게임 재개
        gameObject.SetActive(false);
    }
}