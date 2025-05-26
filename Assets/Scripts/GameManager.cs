using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Scene Change header

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<string> Stages;
    public List<string> Screens;
    public int nowStage = 0;
    public int score;
    public int pointGet = 0;
    public int life;
    public Button settingButton;
    public Button scoreButton;
    private bool _isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // �ߺ� ����
        }
    }

    private void Start()
    {
        // Register the GoSetting callback when the button is clicked
        settingButton.onClick.AddListener(GoSetting);
        scoreButton.onClick.AddListener(GoScoreBoard);
    }
    public void SceneChange(int num)  // Change the scene
    {
        SceneManager.LoadScene(Screens[num]);
    }

    public void StageChange(int num)
    {
        nowStage = num;
        if (nowStage == 1)
        {
            GamePause();
        }
        SceneManager.LoadScene(Stages[nowStage]);
    }

    public void StageClear()
    {
        // 스테이지 클리어 후 10000점을 넘은 경우 목숨을 하나 추가한다.
        if ((score + pointGet) / 10000 > score / 10000)
        {
            life += 1;
        }
        nowStage++;
    }

    public void GamePause()
    {
        _isPaused = true;
    }

    public void GameRestart()
    {
        _isPaused = false;
    }

    public bool CheckPaused()
    {
        return _isPaused;
    }

    public void GameStart()
    {
        life = 3;
        score = 0;
        nowStage = 0;
        SceneChange(1);
    }

    public void GoSetting()
    {
        SceneChange(3);
    }

    public void GoScoreBoard()
    {
        SceneChange(4);
    }
    

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // If we click exit button, exit application.
#endif
    }
}
