using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text uiText;
    private int point = 0;
    public int life = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PointCheck")) //��ֹ� �浹
        {
            point += 100; // ���� �ܰ� �� �ε�
            Debug.Log("���� : " + point);
            uiText.text = "Point: " + point.ToString(); // ���� ���� UI�� ǥ��
        }

        if (other.CompareTag("Obstacle"))
        {
            life -= 1;
            if (life < 0)
            {
                life = 3;
            }
        }
    }
}
