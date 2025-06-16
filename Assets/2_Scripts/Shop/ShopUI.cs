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
        coinText.text = "���� ����: "+ myCoin;
        totalText.text = "�ְ� ���: " + totalTurns;

    }

    void Update()
    {
        if("���� ����: "+myCoin.ToString()!= coinText.text)
        {
            coinText.text = "���� ����: " + myCoin;
        }
        if("�ְ� ���: " + totalTurns.ToString() != totalText.text)
        {
            totalText.text = "�ְ� ���: " + totalTurns;
        }
    }
    public void GoButton()
    {
        SceneManager.LoadScene("PlayScene");
        Time.timeScale = 1f; // ���� �簳
        gameObject.SetActive(false);
    }
}