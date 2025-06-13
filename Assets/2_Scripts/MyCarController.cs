using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCarController : MonoBehaviour
{
    private SurfaceEffector2D surfaceEffector2D;
    private Rigidbody2D rb;
    AudioSource Start_Sound, Running_Sound;

    private bool onGround = false;

    public float jumpForce = 7f;

    private float lastZAngle;
    private float totalRotation; // 누적 회전 각도 (도 단위)
    public int rotateCount;     // 360도(한 바퀴) 회전 횟수

    public static MyCarController Instance;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Running_Sound = GetComponent<AudioSource>();
        Start_Sound = GameObject.Find("Start_Sound").GetComponent<AudioSource>();

        Instance = this;

    }

    private void Start()
    {
        lastZAngle = transform.eulerAngles.z;
        totalRotation = 0f;
        rotateCount = 0;

    }

    float RunningSound_Time = 0f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<SurfaceEffector2D>(out var effector))
        {
            Running_Sound.time = RunningSound_Time;
            surfaceEffector2D = effector;
            surfaceEffector2D.speed = surfaceSpeed; // 초기 속도 설정

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
    private void OnCollisionStay2D(Collision2D collision)
    {
        onGround = true;
        if (!Running_Sound.isPlaying)
            Running_Sound.Play();

            Running_Sound.volume = Mathf.Clamp(rb.linearVelocity.magnitude / 10f, 0.1f, 1f); // 속도에 따라 볼륨 조절
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        RunningSound_Time = Running_Sound.time; // 현재 Running_Sound 시간 저장
        if (collision.gameObject.TryGetComponent<SurfaceEffector2D>(out var effector))
        {
            onGround = false;
        }
        if (Running_Sound.isPlaying)
            Running_Sound.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Start_Sound.isPlaying && !onGround&& collision.CompareTag("Ground"))
            Start_Sound.Play();
        if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
            GameManager.Instance.GameStop();
            Debug.Log($"oops! ({onGround})");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
            Start_Sound.Stop();
    }

    float surfaceSpeed;
    private void Update()
    {
        if (surfaceEffector2D == null) return;

        surfaceSpeed = surfaceEffector2D.speed;

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

        if(onGround)
        {
            if (Input.GetKey(KeyCode.RightArrow))
                surfaceEffector2D.speed = surfaceEffector2D.speed < 12f ? surfaceEffector2D.speed + 3f * Time.deltaTime : 12f;

            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                surfaceEffector2D.speed = 1f;
            }
            else
            {
                surfaceEffector2D.speed = surfaceEffector2D.speed > 5f ? surfaceEffector2D.speed - 3f * Time.deltaTime : surfaceEffector2D.speed + 3f * Time.deltaTime;
            }
        }
        UIManager.Instance.UpdateSurfaceText($"Surface Speed : {surfaceEffector2D.speed:F1}");

        /*if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            Jump();
        }*/
        UIManager.Instance.UpdateCarSpeedText($"Car Speed : {rb.linearVelocity.magnitude:F1}");
        
        if (UIManager.Instance.ESCPanel.activeSelf)
            Running_Sound.Stop();
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
