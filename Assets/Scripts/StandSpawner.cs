using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandSpawner : MonoBehaviour
{
    public GameObject sourceObject;
    public int count = 10;
    public float spacing = 5f;
    public Vector3 direction = Vector3.forward;

    [Header("Multi-Floor Settings")]
    public int floors = 2;
    public Vector3 floorOffset = new Vector3(1.0f, 2.5f, 0); // 층마다 이동할 거리 (x, y, z)

    void Start()
    {
        if (sourceObject == null)
        {
            Debug.LogWarning("sourceObject가 할당되지 않았습니다!");
            return;
        }

        for (int floor = 0; floor < floors; floor++)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 horizontalOffset = direction.normalized * spacing * i;
                Vector3 floorOffsetTotal = floorOffset * floor;

                Vector3 newPos = sourceObject.transform.position + horizontalOffset + floorOffsetTotal;

                Instantiate(sourceObject, newPos, sourceObject.transform.rotation, this.transform);
            }
        }
    }
}