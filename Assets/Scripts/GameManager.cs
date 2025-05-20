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
    public int life;
    [SerializeField] private Button settingButton;

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
    }
    public void SceneChange(int num)  // Change the scene
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

    public void GoSetting()
    {
        SceneChange(3);
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
