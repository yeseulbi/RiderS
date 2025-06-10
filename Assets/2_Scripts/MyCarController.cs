using UnityEngine;
using UnityEngine.UIElements;

public class MyCarController : MonoBehaviour
{
    private SurfaceEffector2D surfaceEffector2D;
    private Rigidbody2D rb;
    private bool onGround = false;

    public float jumpForce = 7f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<SurfaceEffector2D>(out var effector))
        {
            onGround = true;
            surfaceEffector2D = effector;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<SurfaceEffector2D>(out var effector))
        {
            onGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
            GameManager.Instance.GameStop();
            Debug.Log($"oops! ({onGround})");
        }
    }

    private void Update()
    {
        if (surfaceEffector2D == null) return;

        if (Input.GetKey(KeyCode.RightArrow))
            surfaceEffector2D.speed = surfaceEffector2D.speed < 15f ? surfaceEffector2D.speed + 3f*Time.deltaTime : 15f;

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            surfaceEffector2D.speed = 1f;
        }
        else
        {
            surfaceEffector2D.speed = surfaceEffector2D.speed > 5f ? surfaceEffector2D.speed - 3f * Time.deltaTime : surfaceEffector2D.speed + 3f * Time.deltaTime;
        }
        UIManager.Instance.UpdateSurfaceText($"Surface Speed : {surfaceEffector2D.speed:F1}");

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            Jump();
        }
        UIManager.Instance.UpdateCarSpeedText($"Car Speed : {rb.linearVelocity.magnitude:F1}");
    }

    private void FixedUpdate()
    {
        if (!onGround&&Input.GetKey(KeyCode.RightArrow))
                rb.AddTorque(-1.5f);
    }

    private void Jump()
    {
        if (rb == null) return;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
