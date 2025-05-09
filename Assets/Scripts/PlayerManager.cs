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
    private float jumpHeight = 30f;
    private float gravity = -9.81f;

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
                Debug.Log("100 Points!");
                point += 100;
                Debug.Log("���� : " + point);
                UpdateUI();
            }
            if (other.CompareTag("PointCheck5") && !isInvincible)
            {
                Debug.Log("500 Points!");
                point += 500;
                Debug.Log("���� : " + point);
                UpdateUI();
            }
            if (other.CompareTag("PlayerJump") && !isInvincible)
            {
                Debug.Log("Jump!");
                // 자연스러운 점프: 짧은 기간 동안 위로 이동
                StartCoroutine(JumpCoroutine());
            }
            if (other.CompareTag("Obstacle") && !isInvincible)
            {
                Debug.Log("You are collide!");
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

    // 점프 코루틴: 중력 가속도 적용
    private IEnumerator JumpCoroutine()
    {
        // 초기 속도: v0 = sqrt(2 * g * h)
        float velocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        while (!controller.isGrounded)
        {
            controller.Move(Vector3.up * velocity * Time.deltaTime);
            velocity += gravity * Time.deltaTime;
            yield return null;
        }
    }


    private void StageClear()
    {
        GameManager.Instance.score += point;
        GameManager.Instance.StageClear();
        
        // 스테지이 클리어 후 10000점을 넘은 경우 목숨을 하나 추가한다.
        if (GameManager.Instance.score >= 10000){
            GameManager.Instance.life += 1;
        }

        // Stage 4 == GameClear
        if (GameManager.Instance.nowStage == 3){ 
            GameClear();
        } else{
            GameManager.Instance.SceneChange(1);
        }
    }

    private void GameClear()
    {
        Destroy(GameManager.Instance.gameObject);  // 재시작을 위해 
        GameManager.Instance.SceneChange(2);  // if Game cleared, go to the ScoreScreen
    }

    private void GameOver()
    {
        GameManager.Instance.nowStage = -1;
        Destroy(GameManager.Instance.gameObject);
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
