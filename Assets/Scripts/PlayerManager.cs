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
    public static int life = 3; // static ������ ���� (���� �ٲ� ������)
    private CharacterController controller;
    private bool isInvincible = false; // �浹 �ߺ� ����
    //2025.04.01 ������ - ������, ����, �������� Ŭ���� ������ ���� PlayerManager �߰�

    private void Start()
    {
        controller = Player.GetComponent<CharacterController>();
        UpdateUI();
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
            life = 3; // ��� ���� ���� �� �ʱ�ȭ
        }

        playerDead();

        yield return new WaitForSeconds(1f); 
        isInvincible = false;
    }

    void playerDead()
    {
        controller.enabled = false; // ��Ʈ�ѷ� ��� ��Ȱ��ȭ
        Player.transform.position = StartPoint.transform.position;
        controller.enabled = true; // �ٽ� Ȱ��ȭ
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
