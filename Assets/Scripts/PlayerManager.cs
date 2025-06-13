using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Oculus.Interaction;

public class PlayerManager : MonoBehaviour
{
    public TMP_Text PointText;
    public TMP_Text LifeText;
    public TMP_Text BonusText;
    public GameObject StartPoint;
    public GameObject Player;
    public Image[] Hearts;
    public Sprite Heart;
    public Sprite EmptyHeart;
    public ObjGenController[] objCon;
    private int point = 0;
    public int bonusPoint = 5000;
    private bool clear = false;
    public static int life = 3; 
    private CharacterController controller;
    private bool isInvincible = false; 
    private float jumpHeight = 5f;
    private float gravity = -9.81f;

    private void Start()
    {
        controller = Player.GetComponent<CharacterController>();
        HeartInitialize();
        UpdateUI();
        StartCoroutine(DecreaseBonusOverTime());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!clear) {
            //Debug.Log(other.gameObject);
            //if (other == null || other.gameObject == null) return;
            if (other.CompareTag("PointCheck") && !isInvincible)
            {
                SoundManager.Instance.PlayCrowdReaction(true);
                Debug.Log("100 Points!");
                point += 100;
                Debug.Log("���� : " + point);
                UpdateUI();
            }
            if (other.CompareTag("PointCheck5") && !isInvincible)
            {
                SoundManager.Instance.PlayCrowdReaction(true);
                Debug.Log("500 Points!");
                point += 500;
                Debug.Log("���� : " + point);
                UpdateUI();
            }
            //if (other.CompareTag("PlayerJump") && !isInvincible)
            //{
            //    Debug.Log("Jump!");
            //    // 자연스러운 점프: 짧은 기간 동안 위로 이동
            //    StartCoroutine(JumpCoroutine());
            //}
            if (other.CompareTag("Obstacle") && !isInvincible)
            {
                SoundManager.Instance.PlayCrowdReaction(false);
                Debug.Log(other);
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
                for (int i = 0; i < objCon.Length; i++) {
                    objCon[i].removeObject();
                }
                
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

                SoundManager.Instance.PlaySFX(SoundManager.Instance.clearClip); // 효과음 재생
             
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
        //GameManager.Instance.score += point;
        GameManager.Instance.pointGet = point;
        GameManager.Instance.StageClear();

        // Stage 4 == GameClear
        GameManager.Instance.SceneChange(1);
    }

    private void GameClear()
    {
        Destroy(GameManager.Instance.gameObject);  // 재시작을 위해 
        GameManager.Instance.SceneChange(2);  // if Game cleared, go to the ScoreScreen
    }

    public void GameOver()
    {
        GameManager.Instance.nowStage = -1;
        Destroy(GameManager.Instance.gameObject);
        GameManager.Instance.SceneChange(1);
    }

    IEnumerator HandleObstacleCollision()
    {
        isInvincible = true;


        if (GameManager.Instance.life <= 0)
        {
            GameOver();
        } else
        {
            playerDead();
        }


        yield return new WaitForSeconds(0.5f); 
     
        isInvincible = false;
    }

    IEnumerator DecreaseBonusOverTime()
    {
        while (bonusPoint > 0 && !clear)
        {
            if (!GameManager.Instance.CheckPaused())
            {
                bonusPoint -= 10;
                if (bonusPoint < 0) bonusPoint = 0;
                UpdateUI();
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void playerDead()
    {
        controller.enabled = false; 
        Player.transform.position = StartPoint.transform.position;
        controller.enabled = true;
        for (int i = 0; i < objCon.Length; i++)
        {
            objCon[i].removeObject();
        }
        GameManager.Instance.life--;
        point = 0;
        bonusPoint = 5000;
        GameManager.Instance.SceneChange(1);
        //UpdateUI();
    }

    public void HeartInitialize()
    {
        if (Hearts.Length > 0)
        {
            for (int i = 0; i < Hearts.Length; i++)
            {
                if (GameManager.Instance.life > i)
                {
                    Hearts[i].sprite = Heart;
                }
                else
                {
                    Hearts[i].sprite = EmptyHeart;
                }
            }
        }
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
