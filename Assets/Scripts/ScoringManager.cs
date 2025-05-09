using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoringManager : MonoBehaviour
{
    public TMP_Text ClearText;      // show clear text
    public TMP_Text PointText;      // total score
    public TMP_InputField NameInput;  // Initial 3 words
    public Button SubmitButton;


    void Start()
    {
        // PointText 세팅
        PointText.text = "POINT : " + GameManager.Instance.score.ToString();
        NameInput.ActivateInputField();  // auto focus on NameInput field
        SubmitButton.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        GameManager.Instance.SceneChange(0);  // if Button Clicked, go to the MainScreen
    }

}
