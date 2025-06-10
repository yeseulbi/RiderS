using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(SpriteShapeController))]
public class LoadController : MonoBehaviour
{
    public float extendDistance = 2f; // 한 번에 확장할 거리
    public float cloneTriggerDistance = 10f; // 복제 트리거 거리

    private SpriteShapeController spriteShape;
    private Spline spline;
    private Rigidbody2D rb;
    private EdgeCollider2D edgeCollider;
    private float lastCloneX;

    void Start()
    {
        spriteShape = GetComponent<SpriteShapeController>();
        spline = spriteShape.spline;
        rb = GetComponent<Rigidbody2D>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        lastCloneX = transform.position.x;
        UpdateEdgeCollider();
    }

    void Update()
    {
        // Spline을 자동으로 확장 (더 많은 포인트와 험난한 굴곡)
        if (rb.position.x > spline.GetPosition(spline.GetPointCount() - 1).x - extendDistance)
        {
            Vector3 lastPos = spline.GetPosition(spline.GetPointCount() - 1);
            int pointsToAdd = 12; // 더 많은 포인트
            float segment = extendDistance / pointsToAdd;
            float waveAmplitude = 2.5f; // 굴곡의 높이 증가
            float waveFrequency = 6.0f; // 굴곡의 빈도 증가
            float perlinScale = 2.0f;   // Perlin 노이즈 스케일

            for (int i = 1; i <= pointsToAdd; i++)
            {
                float x = lastPos.x + segment * i;
                float perlin = Mathf.PerlinNoise(x * perlinScale, Time.time * 0.1f) - 0.5f;
                float y = lastPos.y
                    + Mathf.Sin((x + Time.time) * waveFrequency) * waveAmplitude
                    + perlin * waveAmplitude * 2.0f;
                Vector3 newPos = new Vector3(x, y, lastPos.z);
                spline.InsertPointAt(spline.GetPointCount(), newPos);

                int idx = spline.GetPointCount() - 1;

                // 연속(Continuous)과 브로큰(Broken)을 번갈아 적용
                if (i % 2 == 0)
                {
                    spline.SetTangentMode(idx, ShapeTangentMode.Continuous);
                    // 양쪽 핸들 길이 0.4로 고정 (오른쪽은 앞으로, 왼쪽은 뒤로)
                    spline.SetLeftTangent(idx, Vector3.left * 0.4f);
                    spline.SetRightTangent(idx, Vector3.right * 0.4f);
                }
                else
                {
                    spline.SetTangentMode(idx, ShapeTangentMode.Broken);
                    // 브로큰은 한쪽만 0.4, 한쪽은 0으로 예시
                    spline.SetLeftTangent(idx, Vector3.left * 0.4f);
                    spline.SetRightTangent(idx, Vector3.zero);
                }
            }
            UpdateEdgeCollider();
        }

        // 일정 거리 도달 시 복제
        if (rb.position.x - lastCloneX >= cloneTriggerDistance)
        {
            GameObject clone = Instantiate(gameObject, new Vector3(rb.position.x, rb.position.y, 0), Quaternion.identity);
            var cloneController = clone.GetComponent<LoadController>();
            if (cloneController != null)
                cloneController.lastCloneX = clone.transform.position.x;
            lastCloneX = rb.position.x;
        }
    }

    // Spline을 EdgeCollider2D에 반영
    void UpdateEdgeCollider()
    {
        int count = spline.GetPointCount();
        Vector2[] points = new Vector2[count];
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = spline.GetPosition(i);
            points[i] = new Vector2(pos.x, pos.y);
        }
        edgeCollider.points = points;
    }
}
