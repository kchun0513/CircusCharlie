using UnityEngine;

public class MonkeySpawner : MonoBehaviour
{
    public GameObject monkeyFastPrefab;
    public GameObject monkeySlowPrefab;

    public float spawnInterval = 3f;
    public float spawnX = 10f; // 오른쪽 바깥쪽에서 생성
    public float spawnY = 0f;
    public float spawnZ = 0f;


    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnMonkey();
            timer = 0f;
        }
    }

    void SpawnMonkey()
    {
        GameObject prefabToSpawn = (Random.value < 0.5f) ? monkeySlowPrefab : monkeyFastPrefab;

        Vector3 spawnPos = new Vector3(spawnX, spawnY, spawnZ);
        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
}