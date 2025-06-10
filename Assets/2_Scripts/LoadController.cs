using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(SpriteShapeController))]
public class LoadController : MonoBehaviour
{
    public float extendDistance = 2f; // �� ���� Ȯ���� �Ÿ�
    public float cloneTriggerDistance = 10f; // ���� Ʈ���� �Ÿ�

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
        // Spline�� �ڵ����� Ȯ�� (�� ���� ����Ʈ�� �賭�� ����)
        if (rb.position.x > spline.GetPosition(spline.GetPointCount() - 1).x - extendDistance)
        {
            Vector3 lastPos = spline.GetPosition(spline.GetPointCount() - 1);
            int pointsToAdd = 12; // �� ���� ����Ʈ
            float segment = extendDistance / pointsToAdd;
            float waveAmplitude = 2.5f; // ������ ���� ����
            float waveFrequency = 6.0f; // ������ �� ����
            float perlinScale = 2.0f;   // Perlin ������ ������

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

                // ����(Continuous)�� ���ū(Broken)�� ������ ����
                if (i % 2 == 0)
                {
                    spline.SetTangentMode(idx, ShapeTangentMode.Continuous);
                    // ���� �ڵ� ���� 0.4�� ���� (�������� ������, ������ �ڷ�)
                    spline.SetLeftTangent(idx, Vector3.left * 0.4f);
                    spline.SetRightTangent(idx, Vector3.right * 0.4f);
                }
                else
                {
                    spline.SetTangentMode(idx, ShapeTangentMode.Broken);
                    // ���ū�� ���ʸ� 0.4, ������ 0���� ����
                    spline.SetLeftTangent(idx, Vector3.left * 0.4f);
                    spline.SetRightTangent(idx, Vector3.zero);
                }
            }
            UpdateEdgeCollider();
        }

        // ���� �Ÿ� ���� �� ����
        if (rb.position.x - lastCloneX >= cloneTriggerDistance)
        {
            GameObject clone = Instantiate(gameObject, new Vector3(rb.position.x, rb.position.y, 0), Quaternion.identity);
            var cloneController = clone.GetComponent<LoadController>();
            if (cloneController != null)
                cloneController.lastCloneX = clone.transform.position.x;
            lastCloneX = rb.position.x;
        }
    }

    // Spline�� EdgeCollider2D�� �ݿ�
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
