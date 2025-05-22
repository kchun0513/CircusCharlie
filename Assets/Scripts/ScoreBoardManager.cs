using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour
{
    public Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(GoMain);
    }

    public void GoMain()
    {
        Debug.Log("[Settings] Exit 버튼 클릭 → GoMain() 실행");
        Destroy(GameManager.Instance.gameObject);  // 재시작을 위해 
        GameManager.Instance.SceneChange(0);
    }
}
