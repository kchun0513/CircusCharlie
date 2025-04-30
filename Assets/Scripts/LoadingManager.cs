using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public float delayTime = 3f; // 3초 후 다음 씬으로 전환
    public TMP_Text StageText;
    public TMP_Text PointText;
    public TMP_Text LifeText;

    void Start()
    {
        if (GameManager.Instance.nowStage == -1)
        {
            StageText.text = "GAME OVER!";
            PointText.text = "POINT : " + GameManager.Instance.score.ToString();
            LifeText.text = "More practice!";
        } else
        {
            StageText.text = "STAGE " + (GameManager.Instance.nowStage + 1).ToString();
            PointText.text = "POINT : " + GameManager.Instance.score.ToString();
            LifeText.text = "LIFE : " + GameManager.Instance.life.ToString();
        }
        Invoke("LoadNextScene", delayTime);      
    }

    void LoadNextScene()
    {
        if (GameManager.Instance.nowStage == -1)
        {
            GameManager.Instance.SceneChange(0);
        } else
        {
            GameManager.Instance.StageChange(GameManager.Instance.nowStage);
        }
    }
}
