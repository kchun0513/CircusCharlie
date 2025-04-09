using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjGenController : MonoBehaviour
{
    public GameObject fireRingPrefab;
    public GameObject fireBasePrefab;
    public Transform spawnPoint;
    public PlayerManager pm;
    public float spawnInterval = 2f;
    private List<GameObject> fireRings = new List<GameObject>();
    public GameObject Player;
    public float spawnerDistance = 100f;
    private Vector3 spawnPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFireRing());
    }

    IEnumerator SpawnFireRing()
    {
        //print(spawnPoint.position);
        while (!pm.getClear())
        {
            spawnPosition = spawnPoint.position;
            spawnPosition.z = Player.transform.position.z + spawnerDistance;
            Debug.Log(spawnPosition);
            int result = Random.Range(0, 2);
            GameObject newObj;
            if (result == 0) {
                newObj = Instantiate(fireRingPrefab, spawnPosition, spawnPoint.rotation);
            } else
            {
                newObj = Instantiate(fireBasePrefab, spawnPosition, spawnPoint.rotation);
            }
            
            fireRings.Add(newObj); 
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = fireRings.Count - 1; i >= 0; i--)
        {
            if (fireRings[i].transform.position.z < 0) // z 좌표가 0보다 작으면 삭제
            {
                Destroy(fireRings[i]);
                fireRings.RemoveAt(i); // 리스트에서도 제거
            }
        }
    }

    public void removeObstacles()
    {
        for (int i = fireRings.Count - 1; i >= 0; i--)
        {
            Destroy(fireRings[i]);
            fireRings.RemoveAt(i);
        }
    }
}
