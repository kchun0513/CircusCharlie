using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public float delayTime = 3f; // 3�� �� ���� ������ ��ȯ
    public TMP_Text StageText;
    public TMP_Text PointText;
    public TMP_Text LifeText;
    private float BlinkingInterval = 0.3f;
    private float HeartBlinkingInterval = 0.05f;
    private float SecondsforAdding = 0.05f;
    public Image[] Hearts;
    private bool _disappearing = true;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
        if (GameManager.Instance.nowStage == -1)
        {
            StageText.text = "GAME OVER!";
            PointText.text = "POINT : " + GameManager.Instance.score.ToString();
            LifeText.text = "More practice!";
        } else
        {
            StageText.text = "STAGE " + (GameManager.Instance.nowStage + 1).ToString();
            PointText.text = "POINT : " + GameManager.Instance.score.ToString();
            if (GameManager.Instance.life == 0)
            {
                LifeText.text = "Last Chance!";
            }
        }
        StartCoroutine(PointAdding());
        StartCoroutine(HeartBlinking());
        StartCoroutine(Blinking());    
    }

    //private void Update()
    //{
    //    for (int i = 0; i < Hearts.Length; i++)
    //    {
    //        if (GameManager.Instance.life < i+1)
    //        {
    //            Hearts[i].color = new Color(Hearts[i].color.r, Hearts[i].color.g, Hearts[i].color.b, 0);
    //        } else if (GameManager.Instance.life == i+1)
    //        {
    //            if (_disappearing)
    //            {
    //                Hearts[i].color = new Color(Hearts[i].color.r, Hearts[i].color.g, Hearts[i].color.b, Hearts[i].color.a - 0.1f);
    //                if (Hearts[i].color.a == 0f)
    //                {
    //                    _disappearing = false;
    //                }
    //            } else
    //            {
    //                Hearts[i].color = new Color(Hearts[i].color.r, Hearts[i].color.g, Hearts[i].color.b, Hearts[i].color.a + 0.1f);
    //                if (Hearts[i].color.a == 1f)
    //                {
    //                    _disappearing = true;
    //                }
    //            }

    //        }
    //    }
    //}

    private IEnumerator PointAdding()
    {
        audioSource.loop = true;
        audioSource.Play();

        while (GameManager.Instance.pointGet > 0)
        {
            if (GameManager.Instance.pointGet >= 111)
            {
                GameManager.Instance.pointGet -= 111;
                GameManager.Instance.score += 111;
            }
            else if (GameManager.Instance.pointGet > 0)
            {
                GameManager.Instance.score += GameManager.Instance.pointGet;
                GameManager.Instance.pointGet = 0;
            }

            PointText.text = "POINT : " + GameManager.Instance.score.ToString();
            yield return new WaitForSeconds(SecondsforAdding);
        }

        audioSource.loop = false;
        audioSource.Stop();

        Invoke("LoadNextScene", delayTime);
    }

    private IEnumerator HeartBlinking()
    {
        while (true)
        {
            for (int i = 0; i < Hearts.Length; i++)
            {
                if (GameManager.Instance.life < i + 1)
                {
                    Hearts[i].color = new Color(Hearts[i].color.r, Hearts[i].color.g, Hearts[i].color.b, 0);
                }
                else if (GameManager.Instance.life == i + 1)
                {
                    if (_disappearing)
                    {
                        Hearts[i].color = new Color(Hearts[i].color.r, Hearts[i].color.g, Hearts[i].color.b, Hearts[i].color.a - 0.1f);
                        if (Hearts[i].color.a <= 0f)
                        {
                            _disappearing = false;
                        }
                    }
                    else
                    {
                        Hearts[i].color = new Color(Hearts[i].color.r, Hearts[i].color.g, Hearts[i].color.b, Hearts[i].color.a + 0.1f);
                        if (Hearts[i].color.a >= 1f)
                        {
                            _disappearing = true;
                        }
                    }

                } else
                {
                    Hearts[i].color = new Color(Hearts[i].color.r, Hearts[i].color.g, Hearts[i].color.b, 1);
                }
            }
            yield return new WaitForSeconds(HeartBlinkingInterval);
        }
    }


    private IEnumerator Blinking()
    {
        while (true)
        {
            if (GameManager.Instance.nowStage == -1)
            {
                if (StageText.text == "GAME OVER!")
                {
                    StageText.text = "";
                }
                else
                {
                    StageText.text = "GAME OVER!";
                }
            }
            else if (GameManager.Instance.nowStage == 3)
            {
                if (StageText.text == "ALL CLEAR!")
                {
                    StageText.text = "";
                }
                else
                {
                    StageText.text = "ALL CLEAR!";
                }
            }
            else
            {
                if (StageText.text == "STAGE " + (GameManager.Instance.nowStage + 1).ToString())
                {
                    StageText.text = "";
                }
                else
                {
                    StageText.text = "STAGE " + (GameManager.Instance.nowStage + 1).ToString();
                }
            }
            yield return new WaitForSeconds(BlinkingInterval);
        }
    }

    void LoadNextScene()
    {
        if (GameManager.Instance.nowStage == -1)
        {
            GameManager.Instance.SceneChange(0);
        }
        else if (GameManager.Instance.nowStage == 3) {
            GameManager.Instance.SceneChange(2);
        }
        else
        {
            GameManager.Instance.StageChange(GameManager.Instance.nowStage);
        }
    }
}
