using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjGenController : MonoBehaviour
{
   /* public GameObject fireRingPrefab;*/
    /*public GameObject fireBasePrefab;*/
    public List<GameObject> ObstaclePrefabs;
    public Transform spawnPoint;
    public PlayerManager pm;
    public float spawnInterval_max = 3f;
    public float spawnInterval_min = 1f;
    private List<GameObject> Objects = new List<GameObject>();
    public GameObject Player;
    public float spawnerDistance = 100f;
    private Vector3 spawnPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObject());
    }

    IEnumerator SpawnObject()
    {
        //print(spawnPoint.position);
        while (!pm.getClear())
        {
            float interval = Random.Range(spawnInterval_min, spawnInterval_max);
            if (!GameManager.Instance.CheckPaused())
            {
                spawnPosition = spawnPoint.position;
                spawnPosition.z = Player.transform.position.z + spawnerDistance;
                //Debug.Log(spawnPosition);
                int result = Random.Range(0, 2);
                GameObject newObj;
                newObj = Instantiate(ObstaclePrefabs[result], spawnPosition, spawnPoint.rotation);

                Objects.Add(newObj);
            }
            yield return new WaitForSeconds(interval);
        }
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = Objects.Count - 1; i >= 0; i--)
        {
            if (Objects[i].transform.position.z < 0)
            {
                Destroy(Objects[i]);
                //Objects[i].SetActive(false);
                Objects.RemoveAt(i);
            }
        }
    }

    public void removeObject()
    {
        for (int i = Objects.Count - 1; i >= 0; i--)
        {
            Destroy(Objects[i]);
            Objects.RemoveAt(i);
        }
    }
}
