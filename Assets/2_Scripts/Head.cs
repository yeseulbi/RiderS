using UnityEngine;
using UnityEngine.Rendering;

public class Head : MonoBehaviour
{
    GameObject myCar => GameObject.Find("MyCar");
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Coin"))
        {
            MyCarController.Instance.Die();
            Destroy(gameObject, 1.1f);
        }
    }
    private void FixedUpdate()
    {
        var Pos = myCar.transform.position;
        var Rot = myCar.transform.rotation;
        gameObject.transform.position = Pos;
        gameObject.transform.rotation = Rot;
    }
}
