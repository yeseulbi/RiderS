using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCarController : MonoBehaviour
{
    private SurfaceEffector2D surfaceEffector2D;
    private Rigidbody2D rb;
    AudioSource Start_Sound, Running_Sound, Dead_Sound;
    ParticleSystem onGround_Particle, Dead_Particle;

    private bool onGround = false;

    public float jumpForce = 7f;

    private float lastZAngle;
    private float totalRotation; // ���� ȸ�� ���� (�� ����)
    public int rotateCount;     // 360��(�� ����) ȸ�� Ƚ��

    public static MyCarController Instance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Running_Sound = GetComponent<AudioSource>();
        Start_Sound = transform.GetChild(0).GetComponent<AudioSource>();
        onGround_Particle = transform.GetChild(0).GetComponent<ParticleSystem>();
        Dead_Sound = transform.GetChild(1).GetComponent<AudioSource>();
        Dead_Particle = transform.GetChild(1).GetComponent<ParticleSystem>();

        Instance = this;
    }

    private void Start()
    {
        lastZAngle = transform.eulerAngles.z;
        totalRotation = 0f;
        rotateCount = 0;

        var main = onGround_Particle.main;
        if(Shop.myCar!=null)
            main.startColor = Shop.myCar.color;
    }

    float RunningSound_Time = 0f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
            return;
        if (collision.gameObject.TryGetComponent<SurfaceEffector2D>(out var effector))
        {
            Running_Sound.time = RunningSound_Time;
            surfaceEffector2D = effector;
            surfaceEffector2D.speed = surfaceSpeed; // �ʱ� �ӵ� ����
            onGround_Particle.Play();

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
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead)
            return;
        onGround = true;
        if (!Running_Sound.isPlaying)
            Running_Sound.Play();
        if (!onGround_Particle.isPlaying)
            onGround_Particle.Play();

            Running_Sound.volume = Mathf.Clamp(rb.linearVelocity.magnitude / 10f, 0.1f, 1f); // �ӵ��� ���� ���� ����
        var main = onGround_Particle.main;
        main.startSize = Mathf.Clamp(rb.linearVelocity.magnitude / 10f, 0.1f, 1f);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        RunningSound_Time = Running_Sound.time; // ���� Running_Sound �ð� ����
        if (collision.gameObject.TryGetComponent<SurfaceEffector2D>(out var effector))
        {
            onGround = false;
        }
        if (Running_Sound.isPlaying)
            Running_Sound.Stop();
        onGround_Particle.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Start_Sound.isPlaying && !onGround&& collision.CompareTag("Ground"))
            Start_Sound.Play();
        if (collision.CompareTag("Obstacle"))
        {
            Die();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
            Start_Sound.Stop();
    }

    float surfaceSpeed;
    private void Update()
    {
        if (isDead)
            return;
        if (surfaceEffector2D == null) return;

        surfaceSpeed = surfaceEffector2D.speed;

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
        UIManager.Instance.UpdateCarSpeedText($"Car Speed : {rb.linearVelocity.magnitude:F1}");

        if (UIManager.Instance.ESCPanel.activeSelf)
            Running_Sound.Stop();
        /*if( Input.GetKeyDown(KeyCode.Space))
        {
            rotateCount++;
            GameManager.myCoin+=100;
        }*/
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
    bool isDead = false;
    public void Die()
    {
        isDead = true;
        onGround_Particle.Stop();
        Running_Sound.Stop();
        var die = gameObject.GetComponent<SpriteRenderer>();
        die.enabled = false;
        Dead_Sound.Play();
        Dead_Particle.Play();
        Destroy(gameObject, 1.1f);
        Invoke(("Gamestop"), 1f);
    }
    void Gamestop()
    {
        GameManager.Instance.GameStop();
    }
    private void Jump()
    {
        if (rb == null) return;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
