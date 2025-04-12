using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_CatSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject catPrefab;         // 고양이 프리팹 (Inspector에서 연결)
    public Transform catSpawnPoint;      // 고양이 리스폰 위치 (예: 줄 위의 특정 위치)
    public float spawnInterval = 3f;     // 고양이가 생성될 간격(초)
    public Transform player;             // 플레이어 Transform (Inspector에서 연결)

    void Start()
    {
        // 플레이어가 Inspector에 연결되지 않았다면, "Player" 태그를 가진 객체에서 찾아보자.
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
        StartCoroutine(SpawnCats());
    }

    IEnumerator SpawnCats()
    {
        while (true)
        {
            // 플레이어를 향하는 방향을 계산
            Vector3 directionToPlayer = (player.position - catSpawnPoint.position).normalized;
            // 해당 방향을 바라보도록 회전값 계산
            Quaternion spawnRotation = Quaternion.LookRotation(directionToPlayer);
            
            // 새로운 고양이 인스턴스를, 계산한 회전값과 함께 생성
            GameObject newCat = Instantiate(catPrefab, catSpawnPoint.position, spawnRotation);
            
            // 생성된 고양이의 CatMove 스크립트에 플레이어 Transform 연결 (있다면)
            Stage2_CatMove catMove = newCat.GetComponent<Stage2_CatMove>();
            if (catMove != null)
            {
                catMove.player = player;
            }
            
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}