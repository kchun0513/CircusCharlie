using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineSpawner : MonoBehaviour
{
    public List<GameObject> ObstaclePrefabs;
    public Transform spawnPoint;
    public PlayerManager pm;
    public GameObject Player;

    public float spawnerDistance = 100f;
    public float spawnIntervalMin = 1f;
    public float spawnIntervalMax = 3f;

    public float[] spawnYPositions = new float[] { 0f, 2f }; // 두 개의 y 위치
    private float[] lastSpawnTime;

    private List<GameObject> activeObstacles = new List<GameObject>();
    private int maxObstacles = 8;

    void Start()
    {
        lastSpawnTime = new float[spawnYPositions.Length];
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (!pm.getClear())
        {
            if (!GameManager.Instance.CheckPaused() && activeObstacles.Count < maxObstacles)
            {
                for (int i = 0; i < spawnYPositions.Length; i++)
                {
                    if (Time.time - lastSpawnTime[i] >= spawnIntervalMin)
                    {
                        float interval = Random.Range(spawnIntervalMin, spawnIntervalMax);

                        Vector3 spawnPos = new Vector3(
                            spawnPoint.position.x,
                            spawnYPositions[i],
                            Player.transform.position.z + spawnerDistance
                        );

                        GameObject prefab = ObstaclePrefabs[Random.Range(0, ObstaclePrefabs.Count)];
                        GameObject newObstacle = Instantiate(prefab, spawnPos, Quaternion.identity);
                        activeObstacles.Add(newObstacle);
                        lastSpawnTime[i] = Time.time;

                        yield return new WaitForSeconds(interval);
                    }
                }
            }
            yield return null;
        }
    }

    void Update()
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            if (activeObstacles[i] == null || activeObstacles[i].transform.position.z < 0)
            {
                Destroy(activeObstacles[i]);
                activeObstacles.RemoveAt(i);
            }
        }
    }

    public void RemoveAllObstacles()
    {
        foreach (var obj in activeObstacles)
        {
            if (obj != null) Destroy(obj);
        }
        activeObstacles.Clear();
    }
}
