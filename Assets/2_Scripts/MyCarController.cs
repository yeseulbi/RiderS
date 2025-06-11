using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCarController : MonoBehaviour
{
    private SurfaceEffector2D surfaceEffector2D;
    private Rigidbody2D rb;
    private bool onGround = false;

    public float jumpForce = 7f;

    // Load_1 프리팹을 인스펙터에서 할당
    public GameObject[] loadPrefab;

    public float lastLoadX = 0f; // 마지막 Load 생성 위치

    private float lastZAngle;
    private float totalRotation; // 누적 회전 각도 (도 단위)
    private int rotateCount;     // 360도(한 바퀴) 회전 횟수

    // 복제된 Load 오브젝트를 추적할 리스트
    private List<GameObject> spawnedLoads = new List<GameObject>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // 시작 시 첫 Load 위치를 현재 위치로 초기화
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

            // 공중에서 누적 회전이 270도 이상이면 한 바퀴로 판정
            if (Mathf.Abs(totalRotation) >= 270f)
            {
                rotateCount++;
                Debug.Log($"(착지) 회전 횟수: {rotateCount}");
                UIManager.Instance.UpdateRotateCount(rotateCount);
                totalRotation = 0f; // 누적 회전 초기화
            }
            else
            {
                totalRotation = 0f; // 착지 시 각도 초기화
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

        // 누적 회전 각도 계산
        float currentZ = transform.eulerAngles.z;
        float delta = Mathf.DeltaAngle(lastZAngle, currentZ); // -180~180도 차이
        totalRotation += delta;
        lastZAngle = currentZ;

        // 한 바퀴(360도) 회전 체크
        if (Mathf.Abs(totalRotation) >= 360f)
        {
            rotateCount += (int)(Mathf.Abs(totalRotation) / 360f);
            totalRotation = totalRotation % 360f; // 남은 각도만 저장
            Debug.Log($"회전 횟수: {rotateCount}");
            UIManager.Instance.UpdateRotateCount(rotateCount);
        }

        // Load_1 프리팹 복제 로직 (랜덤 프리팹)
        if (transform.position.x - lastLoadX >= 0)
        {
            Vector3 spawnPos = new Vector3(lastLoadX + 52f, 6.03f, 0f);
            int randomIndex = Random.Range(0, loadPrefab.Length);
            GameObject newLoad = Instantiate(loadPrefab[randomIndex], spawnPos, Quaternion.identity);
            spawnedLoads.Add(newLoad);

            // 2개 초과 시 가장 먼저 생성된 Load 삭제
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
