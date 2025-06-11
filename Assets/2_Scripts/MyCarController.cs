using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCarController : MonoBehaviour
{
    private SurfaceEffector2D surfaceEffector2D;
    private Rigidbody2D rb;
    private bool onGround = false;

    public float jumpForce = 7f;

    // Load_1 �������� �ν����Ϳ��� �Ҵ�
    public GameObject[] loadPrefab;

    public float lastLoadX = 0f; // ������ Load ���� ��ġ

    private float lastZAngle;
    private float totalRotation; // ���� ȸ�� ���� (�� ����)
    private int rotateCount;     // 360��(�� ����) ȸ�� Ƚ��

    // ������ Load ������Ʈ�� ������ ����Ʈ
    private List<GameObject> spawnedLoads = new List<GameObject>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // ���� �� ù Load ��ġ�� ���� ��ġ�� �ʱ�ȭ
        lastLoadX = transform.position.x + 20f;
    }

    private void Start()
    {
        lastZAngle = transform.eulerAngles.z;
        totalRotation = 0f;
        rotateCount = 0;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<SurfaceEffector2D>(out var effector))
        {
            onGround = true;
            surfaceEffector2D = effector;

            // ���߿��� ���� ȸ���� 270�� �̻��̸� �� ������ ����
            if (Mathf.Abs(totalRotation) >= 270f)
            {
                rotateCount++;
                Debug.Log($"(����) ȸ�� Ƚ��: {rotateCount}");
                UIManager.Instance.UpdateRotateCount(rotateCount);
                totalRotation = 0f; // ���� ȸ�� �ʱ�ȭ
            }
            else
            {
                totalRotation = 0f; // ���� �� ���� �ʱ�ȭ
            }
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

        // ���� ȸ�� ���� ���
        float currentZ = transform.eulerAngles.z;
        float delta = Mathf.DeltaAngle(lastZAngle, currentZ); // -180~180�� ����
        totalRotation += delta;
        lastZAngle = currentZ;

        // �� ����(360��) ȸ�� üũ
        if (Mathf.Abs(totalRotation) >= 360f)
        {
            rotateCount += (int)(Mathf.Abs(totalRotation) / 360f);
            totalRotation = totalRotation % 360f; // ���� ������ ����
            Debug.Log($"ȸ�� Ƚ��: {rotateCount}");
            UIManager.Instance.UpdateRotateCount(rotateCount);
        }

        // Load_1 ������ ���� ���� (���� ������)
        if (transform.position.x - lastLoadX >= 0)
        {
            Vector3 spawnPos = new Vector3(lastLoadX + 52f, 6.03f, 0f);
            int randomIndex = Random.Range(0, loadPrefab.Length);
            GameObject newLoad = Instantiate(loadPrefab[randomIndex], spawnPos, Quaternion.identity);
            spawnedLoads.Add(newLoad);

            // 2�� �ʰ� �� ���� ���� ������ Load ����
            if (spawnedLoads.Count > 2)
            {
                Destroy(spawnedLoads[0]);
                spawnedLoads.RemoveAt(0);
            }

            lastLoadX += 52f;
        }

        if (Input.GetKey(KeyCode.RightArrow))
            surfaceEffector2D.speed = surfaceEffector2D.speed < 15f ? surfaceEffector2D.speed + 3f * Time.deltaTime : 15f;

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
        if (!onGround && rb.angularVelocity > -500f)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddTorque(-2f);
            }
            else
            {
                rb.angularVelocity = 0f; // Reset angular velocity when not accelerating
            }
        }
    }

    private void Jump()
    {
        if (rb == null) return;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
