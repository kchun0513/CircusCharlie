using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public TMP_Text PointText;
    public TMP_Text LifeText;
    public TMP_Text BonusText;
    public GameObject StartPoint;
    public GameObject Player;
    public ObjGenController objCon;
    private int point = 0;
    public int bonusPoint = 5000;
    private bool clear = false;
    public static int life = 3; 
    private CharacterController controller;
    private bool isInvincible = false; 

    private void Start()
    {
        controller = Player.GetComponent<CharacterController>();
        UpdateUI();
        StartCoroutine(DecreaseBonusOverTime());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!clear) {
            if (other.CompareTag("PointCheck") && !isInvincible)
            {
                point += 100;
                Debug.Log("���� : " + point);
                UpdateUI();
            }
            if (other.CompareTag("Obstacle") && !isInvincible)
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
                // in stage2, this part error. So we check if deathZone is Null.
                if (deathZone != null)
                {
                    Collider col = deathZone.GetComponent<Collider>();
                    if (col != null)
                    {
                        col.isTrigger = false;
                    }
                    else
                    {
                        Debug.LogError("DeathZone has no Collider component.");
                    }
                }
                else
                {
                    Debug.LogError("No GameObject found with tag 'Obstacle' for deathZone.");
                }
                PointText.text = "POINT : " + point.ToString();
                LifeText.text = "Stage Clear!";
                
             
                Invoke("StageClear", 3f);
                
            }
        }
    }

    private void StageClear()
    {
        GameManager.Instance.score += point;
        GameManager.Instance.StageClear();
        GameManager.Instance.SceneChange(1);
    }

    private void GameOver()
    {
        GameManager.Instance.nowStage = -1;
        GameManager.Instance.SceneChange(1);
    }

    IEnumerator HandleObstacleCollision()
    {
        isInvincible = true;
        GameManager.Instance.life--; 

        if (GameManager.Instance.life <= 0)
        {
            GameOver();
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
        controller.enabled = false; 
        Player.transform.position = StartPoint.transform.position;
        controller.enabled = true;
        objCon.removeObject();
        point = 0;
        bonusPoint = 5000;
        GameManager.Instance.SceneChange(1);
        //UpdateUI();
    }

    void UpdateUI()
    {
        if (!clear)
        {
            if (PointText != null) { PointText.text = "POINT : " + point.ToString(); }
            if (LifeText != null) { LifeText.text = "LIFE : " + GameManager.Instance.life.ToString(); }
            if (BonusText != null) { BonusText.text = "BONUS : " + bonusPoint.ToString(); }
        }
        
    }

    public bool getClear()
    {
        return clear;
    }
}
