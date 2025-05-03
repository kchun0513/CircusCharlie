using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<string> Stages;
    public List<string> Screens;
    public int nowStage = 0;
    public int score;
    public int life;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }
    public void SceneChange(int num)
    {
        SceneManager.LoadScene(Screens[num]);
    }

    public void StageChange(int num)
    {
        nowStage = num;
        SceneManager.LoadScene(Stages[nowStage]);
    }

    public void StageClear()
    {
        nowStage++;
    }

    public void GameStart()
    {
        life = 3;
        score = 0;
        nowStage = 0;
        SceneChange(1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
