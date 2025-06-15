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

        coinText.text = "���� ����: "+ myCoin;
        totalText.text = "�ְ� ���: " + GameManager.Instance.TotalTurn;
    }

    void Update()
    {
        if(myCoin!= GameManager.myCoin)
        {
            myCoin = GameManager.myCoin;
            coinText.text = "���� ����: " + myCoin;
        }
    }
    public void GoButton()
    {
        SceneManager.LoadScene("PlayScene");
        Time.timeScale = 1f; // ���� �簳
        gameObject.SetActive(false);
    }
}