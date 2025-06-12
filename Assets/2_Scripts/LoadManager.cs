using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    // 복제된 Load 오브젝트를 추적할 리스트
    private List<GameObject> spawnedLoads = new List<GameObject>();
    public MyCarController myCarController; // MyCarController 스크립트 참조

    // Load_1 프리팹을 인스펙터에서 할당
    public GameObject[] loadPrefab;

    public float lastLoadX = 0f; // 마지막 Load 생성 위치
    public GameObject myCar; // MyCar 오브젝트 참조
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
        // Load_1 프리팹 복제 로직 (랜덤 프리팹)
        if (myCar.transform.position.x - lastLoadX >= 0)
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
            Debug.Log($"lastLoadX: {lastLoadX}, myCarX: {myCar.transform.position.x}");
        }
    }
}
