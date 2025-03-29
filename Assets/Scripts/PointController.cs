using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointController : MonoBehaviour
{
    private int point = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PointCheck")) //��ֹ� �浹
        { 
            point += 100; // ���� �ܰ� �� �ε�
            Debug.Log("���� : " + point);
        }
    }
}
