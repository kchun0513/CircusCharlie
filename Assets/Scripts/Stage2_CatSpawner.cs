using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_CatSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject catPrefab;         // 고양이 프리팹 (Inspector에서 연결)
    public Transform catSpawnPoint;      // 고양이가 생성될 위치 (예: 줄 위의 특정 위치)
    public float spawnInterval = 3f;     // 고양이가 생성될 간격 (초)

    void Start()
    {
        StartCoroutine(SpawnCats());
    }

    IEnumerator SpawnCats()
    {
        while (true)
        {
            // catSpawnPoint의 위치와 회전값으로 고양이를 생성함
            Instantiate(catPrefab, catSpawnPoint.position, catSpawnPoint.rotation);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}