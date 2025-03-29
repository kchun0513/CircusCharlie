using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointController : MonoBehaviour
{
    private int point = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PointCheck")) //장애물 충돌
        { 
            point += 100; // 현재 단계 재 로드
            Debug.Log("쩜수 : " + point);
        }
    }
}
