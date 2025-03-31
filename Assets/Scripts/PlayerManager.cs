using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public TMP_Text PointText;
    public TMP_Text LifeText;
    public GameObject StartPoint;
    public GameObject Player;
    public ObjGenController objCon;
    private int point = 0;
    private bool clear = false;
    public static int life = 3; // static 변수로 변경 (씬이 바뀌어도 유지됨)
    private CharacterController controller;
    private bool isInvincible = false; // 충돌 중복 방지
    //2025.04.01 김충훈 - 라이프, 점수, 스테이지 클리어 판정을 위한 PlayerManager 추가

    private void Start()
    {
        controller = Player.GetComponent<CharacterController>();
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!clear) {
            if (other.CompareTag("PointCheck") && !isInvincible) // 점수 획득
            {
                point += 100;
                Debug.Log("점수 : " + point);
                UpdateUI();
            }
            if (other.CompareTag("Obstacle") && !isInvincible) // 장애물 충돌
            {
                StartCoroutine(HandleObstacleCollision());
            }
        }
     
        if (other.CompareTag("Finish"))
        {
            clear = true;
            LifeText.text = "Stage 1 Clear!";
            objCon.removeObstacles();
        }
    }

    IEnumerator HandleObstacleCollision()
    {
        isInvincible = true; 
        life--; 

        if (life <= 0)
        {
            life = 3; // 모든 생명 소진 시 초기화
        }

        playerDead();

        yield return new WaitForSeconds(1f); 
        isInvincible = false;
    }

    void playerDead()
    {
        controller.enabled = false; // 컨트롤러 잠시 비활성화
        Player.transform.position = StartPoint.transform.position;
        controller.enabled = true; // 다시 활성화
        objCon.removeObstacles();
        point = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (PointText != null) { PointText.text = "POINT : " + point.ToString(); }
        if (LifeText != null) { LifeText.text = "LIFE : " + life.ToString(); }
    }

    public bool getClear()
    {
        return clear;
    }
}
