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
        if (other.CompareTag("PointCheck")) //장애물 충돌
        {
            point += 100; // 현재 단계 재 로드
            Debug.Log("쩜수 : " + point);
            uiText.text = "Point: " + point.ToString(); // 변수 값을 UI에 표시
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
