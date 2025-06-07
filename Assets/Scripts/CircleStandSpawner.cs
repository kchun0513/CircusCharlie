using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleStandSpawner : MonoBehaviour
{
    public GameObject sourceObject;
    public float radius = 10f;

    [Header("Multi-Floor Settings")]
    public int floors = 2;
    public Vector3 floorOffset = new Vector3(0, 2.5f, 10f); // (X, Y, Z)
    public List<int> countsPerFloor = new List<int>() { 10, 12 }; // 층마다 개수 직접 지정

    void Start()
    {
        if (sourceObject == null)
        {
            Debug.LogWarning("sourceObject가 할당되지 않았습니다!");
            return;
        }

        Vector3 center = sourceObject.transform.position;

        for (int floor = 0; floor < floors; floor++)
        {
            // 방어 코드: 리스트 길이보다 floors가 클 경우 예외 방지
            int floorCount = (floor < countsPerFloor.Count) ? countsPerFloor[floor] : countsPerFloor[^1];

            float r = radius + floorOffset.z * floor;
            float y = floorOffset.y * floor;
            float xOffset = floorOffset.x * floor;

            // 왼쪽 반원 (180° → 0°)
            for (int i = 0; i < floorCount; i++)
            {
                float t = (float)i / (floorCount - 1);
                float angle = Mathf.Lerp(180f, 0f, t);
                CreateStandAtAngle(angle, center, r, y, xOffset);
            }

            // 오른쪽 반원 (0° → 180°)
            for (int i = 0; i < floorCount; i++)
            {
                float t = (float)i / (floorCount - 1);
                float angle = Mathf.Lerp(0f, 180f, t);
                CreateStandAtAngle(angle, center, r, y, xOffset);
            }
        }

        sourceObject.SetActive(false);
    }

    void CreateStandAtAngle(float angleDeg, Vector3 center, float radius, float yOffset, float xOffset)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad));
        Vector3 offset = direction * radius + new Vector3(xOffset, yOffset, 0);
        Vector3 spawnPos = center + offset;

        Quaternion rot = Quaternion.LookRotation(center - spawnPos) * Quaternion.Euler(0, 90f, 0);
        Instantiate(sourceObject, spawnPos, rot, this.transform);
    }
}