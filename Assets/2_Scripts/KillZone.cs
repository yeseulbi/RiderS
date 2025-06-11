using Unity.VisualScripting;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public MyCarController myCarController;
    float lastLoadX_Rem;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.GameStop();   
    }
    private void Start()
    {
        lastLoadX_Rem = myCarController.lastLoadX;
    }
    private void Update()
    {

        if (myCarController.lastLoadX > lastLoadX_Rem)
        {
            transform.position = new Vector2(transform.position.x + 24f, transform.position.y);
            lastLoadX_Rem = myCarController.lastLoadX;
            Debug.Log($"lastLoadRem: {lastLoadX_Rem}, lastLoad: {myCarController.lastLoadX}");
        }
    }
}
