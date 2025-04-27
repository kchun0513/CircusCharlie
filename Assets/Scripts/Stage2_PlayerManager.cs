using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Stage2_PlayerManager : MonoBehaviour
{
    public TMP_Text PointText;
    public TMP_Text LifeText;
    public TMP_Text BonusText;
    public GameObject StartPoint;
    public GameObject Player;
    public ObjGenController objCon;
    private int point = 0;  // 초기 포인트는 0점
    public int bonusPoint = 5000;  // 초기 보너스 점수는 5000점
    private bool clear = false;
    public static int life = 3; // 초기 목숨은 3개로 고정
    private CharacterController controller;
    private bool isInvincible = false; // 

    private void Start()
    {
        controller = Player.GetComponent<CharacterController>();
        UpdateUI();
        StartCoroutine(DecreaseBonusOverTime());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!clear) {
            if (other.CompareTag("PointCheck") && !isInvincible) // ���� ȹ��
            {
                point += 100;
                Debug.Log("���� : " + point);
                UpdateUI();
            }
            if (other.CompareTag("Obstacle") && !isInvincible) // ��ֹ� �浹
            {
                StartCoroutine(HandleObstacleCollision());
            }
        }
     
        if (other.CompareTag("Finish"))
        {
            if (!clear)
            {
                clear = true;
                point = point + bonusPoint;
                objCon.removeObject();
                GameObject deathZone = GameObject.FindWithTag("Obstacle");
                deathZone.GetComponent<BoxCollider>().isTrigger = false;
                PointText.text = "POINT : " + point.ToString();
                LifeText.text = "Stage 1 Clear!";
            }
        }
    }

    IEnumerator HandleObstacleCollision()
    {
        isInvincible = true; 
        life--; 

        if (life <= 0)
        {
            life = 3; // ��� ���� ���� �� �ʱ�ȭ
        }

        playerDead();

        yield return new WaitForSeconds(0.5f); 
     
        isInvincible = false;
    }

    IEnumerator DecreaseBonusOverTime()
    {
        while (bonusPoint > 0 && !clear)
        {
            yield return new WaitForSeconds(0.25f);
            bonusPoint -= 10;
            if (bonusPoint < 0) bonusPoint = 0;
            UpdateUI();
        }
    }

    void playerDead()
    {
        controller.enabled = false; // ��Ʈ�ѷ� ��� ��Ȱ��ȭ
        Player.transform.position = StartPoint.transform.position;
        controller.enabled = true; // �ٽ� Ȱ��ȭ
        objCon.removeObject();
        point = 0;
        bonusPoint = 5000;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (!clear)
        {
            if (PointText != null) { PointText.text = "POINT : " + point.ToString(); }
            if (LifeText != null) { LifeText.text = "LIFE : " + life.ToString(); }
            if (BonusText != null) { BonusText.text = "BONUS : " + bonusPoint.ToString(); }
        }
        
    }

    public bool getClear()
    {
        return clear;
    }
}
