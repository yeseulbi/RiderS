using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    // ������ Load ������Ʈ�� ������ ����Ʈ
    private List<GameObject> spawnedLoads = new List<GameObject>();
    public MyCarController myCarController; // MyCarController ��ũ��Ʈ ����

    // Load_1 �������� �ν����Ϳ��� �Ҵ�
    public GameObject[] loadPrefab;

    public float lastLoadX = 0f; // ������ Load ���� ��ġ
    public GameObject myCar; // MyCar ������Ʈ ����
    private void Awake()
    {
        lastLoadX = myCar.transform.position.x + 20f;
    }
    void Start()
    {
    }

    void Update()
    {
        if (myCar == null)
            return;
        // Load_1 ������ ���� ���� (���� ������)
        if (myCar.transform.position.x - lastLoadX >= 0)
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
            Debug.Log($"lastLoadX: {lastLoadX}, myCarX: {myCar.transform.position.x}");
        }
    }
}
