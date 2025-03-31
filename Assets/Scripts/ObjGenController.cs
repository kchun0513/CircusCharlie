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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFireRing());
    }

    IEnumerator SpawnFireRing()
    {
        print(spawnPoint.position);
        while (!pm.getClear())
        {
            int result = Random.Range(0, 2);
            GameObject newObj;
            if (result == 0) {
                newObj = Instantiate(fireRingPrefab, spawnPoint.position, spawnPoint.rotation);
            } else
            {
                newObj = Instantiate(fireBasePrefab, spawnPoint.position, spawnPoint.rotation);
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
