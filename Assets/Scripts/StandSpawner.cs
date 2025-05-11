using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StandSpawner : MonoBehaviour
{
    public GameObject sourceObject;  // 씬에 미리 배치된 관중석
    public int count = 10;           // 생성 개수
    public float spacing = 5f;       // 간격
    public Vector3 direction = Vector3.forward; // 복사 방향

    void Start()
    {
        if (sourceObject == null)
        {
            Debug.LogWarning("sourceObject가 할당되지 않았습니다!");
            return;
        }

        for (int i = 1; i < count; i++)
        {
            Vector3 newPos = sourceObject.transform.position + direction.normalized * spacing * i;
            Quaternion rot = sourceObject.transform.rotation;
            GameObject copy = Instantiate(sourceObject, newPos, rot, this.transform);
        }
    }
}