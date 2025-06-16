using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*Color 버튼 함수, Color메소드를 입력할 수 있게 만든다.
 Type 버튼 함수, enum Type을 입력할 수 있게 만든다.*/
public enum carType
{
    SkateBoard,
    Car,
    Scooter,
    Bicycle,
    Ship
}
public class Car
{
    public Color color;  // 0.White 1.Yellow 2.Purple 3.SkyBlue 4.Pink
    public carType type; // 0.SkateBoard 1.Car 2.Scooter 3.Bicycle 4.Ship
    public int price;
    public Car(carType type, int price) // 판매용 차
    {
        this.type = type;
        this.price = price;
    }
    public Car()    // null 일시 기본 차
    {
        type = carType.SkateBoard;
        color = Color.white;
    }
}
public class Shop : MonoBehaviour
{
    public static Shop Instance { get; private set; }
    public static Car myCar;
    public GameObject ShopObject;
    List<Car> cars = new List<Car>();
    private void Awake()
    {
        ShopObject = gameObject;

        cars.Add(new Car(carType.Car, 100));
        cars.Add(new Car(carType.Scooter, 200));
        cars.Add(new Car(carType.Bicycle, 350));
        cars.Add(new Car(carType.Ship, 500));
    }
    SpriteRenderer[] carSprite = new SpriteRenderer[5];
    int TotalTurn = GameManager.TotalTurn;
    Button[] ColorButton;
    void Start()
    {
        if (myCar == null)
            myCar = new Car();

        ColorButton = new Button[5];
        for (int i = 0; i < carSprite.Length; i++)
        {
            carSprite[i] = transform.GetChild(i + 2).GetComponent<SpriteRenderer>();
        }

        var ColorCanvas = transform.GetChild(0);
        for (int i = 0; i < ColorButton.Length; i++)
        {
            ColorButton[i] = ColorCanvas.GetChild(i).GetComponent<Button>();
        }

        // 구매 상태 불러오기 및 UI 반영
        for (int i = 0; i < cars.Count; i++)
        {
            if (PlayerPrefs.GetInt($"CarBought{i}", 0) == 1)
            {
                BuyParents.GetChild(i).gameObject.SetActive(false);
                ButtonParents.GetChild(i + 1).GetComponent<Button>().interactable = true;
            }
            else
            {
                BuyParents.GetChild(i).gameObject.SetActive(true);
                ButtonParents.GetChild(i + 1).GetComponent<Button>().interactable = false;
            }

            if (PlayerPrefs.GetInt($"CarCheck{i}", 0) == 1)
            {
                ButtonParents.GetChild(i).Find("Check").gameObject.SetActive(true);
            }
            else
                ButtonParents.GetChild(i).Find("Check").gameObject.SetActive(false);
        }
        for(int j=1; j<cars.Count;j++)
        {
            if(PlayerPrefs.GetInt($"CarCheck{j}", 0) == 0)
                ButtonParents.GetChild(0).Find("Check").gameObject.SetActive(true);
        }
        if (TotalTurn >= 15)
            ColorCheck(1);
        if (TotalTurn >= 30)
            ColorCheck(2);
        if (TotalTurn >= 50)
            ColorCheck(3);
        if (TotalTurn >= 100)
            ColorCheck(4);
    }
    private void Update()
    {
        if (carSprite[0].color!=myCar.color)
            for(int i=0;i<carSprite.Length;i++)
            {
                carSprite[i].color = myCar.color;
            }
    }
    void ColorCheck(int index)
    {
        ColorButton[index].interactable = true;
        ColorButton[index].GetComponentInChildren<Image>().enabled = false;
    }

    public void ColorSet(int Index)  // button. 총 5개
    {
        switch(Index)
        {
            case 0:
                myCar.color = Color.white;
                break;
            case 1:
                myCar.color = Color.yellow;
                break;
            case 2:
                myCar.color = new Color(0.5019608f, 0,1);
                break;
            case 3:
                myCar.color = Color.cyan;
                break;
            case 4:
                myCar.color = new Color(1, 0.682353f, 1);
                break;
        }
        Debug.Log($"Set Color: {myCar.color}");
    }

    [SerializeField]Transform ButtonParents, BuyParents;
    public void carBuy(int Number)
    {
        if (GameManager.myCoin >= cars[Number].price)
        {
            GameManager.myCoin -= cars[Number].price;
            BuyParents.GetChild(Number).gameObject.SetActive(false);
            ButtonParents.GetChild(Number + 1).GetComponent<Button>().interactable = true;

            // 구매 상태 저장
            PlayerPrefs.SetInt($"CarBought{Number}", 1);
            PlayerPrefs.Save();
        }
        else
            Debug.Log($"{cars[Number].price}는 가진 돈보다 많다");
    }
    public void TypeSet(int Index)
    {
        myCar.type = (carType)Index;
        for(int i=0; i< ButtonParents.childCount; i++)
        {
            ButtonParents.GetChild(i).Find("Check").gameObject.SetActive(false);
            PlayerPrefs.SetInt($"CarCheck{i}", 0);
        }
        ButtonParents.GetChild(Index).Find("Check").gameObject.SetActive(true);

        PlayerPrefs.SetInt($"CarCheck{Index}", 1);
        PlayerPrefs.Save();
    }
}