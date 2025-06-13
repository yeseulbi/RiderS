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
    Transform Loads;
    private void Awake()
    {
        lastLoadX = myCar.transform.position.x + 20f;
    }
    void Start()
    {
        Loads = GetComponent<Transform>();
    }

    void Update()
    {
        if (myCar == null)
            return;
        // ù��° Load ������ ����
        if (spawnedLoads.Count == 0)
            LoadSpawn();

        // Load_1 ������ ���� ���� (���� ������)
        if (myCar.transform.position.x - lastLoadX >= 0)
        {
            LoadSpawn(52f);

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

    void LoadSpawn(float pos)
    {
        Vector3 spawnPos = new Vector3(lastLoadX + pos, 6.03f, 0f);
        int randomIndex = Random.Range(0, loadPrefab.Length);
        GameObject newLoad = Instantiate(loadPrefab[randomIndex], spawnPos, Quaternion.identity, Loads);
        spawnedLoads.Add(newLoad);
    }
    void LoadSpawn()
    {
        Vector3 spawnPos = new Vector3(lastLoadX, 6.03f, 0f);
        int randomIndex = Random.Range(0, loadPrefab.Length);
        GameObject newLoad = Instantiate(loadPrefab[randomIndex], spawnPos, Quaternion.identity, Loads);
        spawnedLoads.Add(newLoad);
    }
}
