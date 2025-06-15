using System;
using UnityEngine;

public class MyCarSetting : MonoBehaviour
{
    public Sprite[] carSprite;
    SpriteRenderer mySR;

    private void Awake()
    {
        mySR = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        if (Shop.myCar != null)
        {
            var index = (int)Shop.myCar.type;
            mySR.sprite = carSprite[index];

            mySR.color = Shop.myCar.color;
        }
        else
            Shop.myCar = new Car();
    }

    void Update()
    {
        
    }
}