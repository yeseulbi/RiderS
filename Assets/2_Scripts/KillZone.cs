using Unity.VisualScripting;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public GameObject MyCar;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.GameStop();   
    }
    private void Start()
    {

    }
    private void Update()
    {
        if (MyCar == null)
            return;

        transform.position = new Vector2(MyCar.transform.position.x+24f, transform.position.y);
    }
}
