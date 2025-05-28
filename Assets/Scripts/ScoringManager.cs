using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public GameObject keyButtonPrefab;    // KeyButton 프리팹
    public Transform keysContainer;       // Grid Layout Group
    public Button backspaceButton;
    private List<char> current = new List<char>();
    private const int MaxChars = 3;

    void Start()
    {
        // 1) 키 생성
        for (char c = 'A'; c <= 'Z'; c++) {
            var go = Instantiate(keyButtonPrefab, keysContainer);
            var kb = go.GetComponent<KeyButton>();
            kb.Init(c, OnKeyPressed);
        }
        // 2) 특수 버튼 이벤트 연결
        backspaceButton.onClick.AddListener(OnBackspace);
        SubmitButton.onClick.AddListener(OnSubmit);
        // 3) 초기 표시 갱신
        RefreshDisplay();

        // PointText 세팅
        PointText.text = "POINT : " + GameManager.Instance.score.ToString();
        NameInput.ActivateInputField();  // auto focus on NameInput field
    }

    void OnKeyPressed(char c) {
        if (current.Count >= MaxChars) return;
        current.Add(c);
        RefreshDisplay();
    }

    void OnBackspace() {
        if (current.Count > 0) {
            current.RemoveAt(current.Count - 1);
            RefreshDisplay();
        }
    }

    void OnSubmit() {
        if (current.Count == MaxChars)
        {
            string initials = new string(current.ToArray());
            Debug.Log("Entered Initials: " + initials);
            // Append to CSV in persistent data path
            string csvPath = Path.Combine(Application.persistentDataPath, "ScoreRecord.csv");
            string line = GameManager.Instance.score + "," + initials;
            File.AppendAllText(csvPath, line + System.Environment.NewLine);
            GameManager.Instance.SceneChange(0);
            // TODO: GameManager.Instance.SetPlayerInitials(initials);
        }
        else
        {
            Debug.LogWarning("3글자를 입력해주세요.");
        }
    }

    void RefreshDisplay() {
        NameInput.text = new string(current.ToArray());
    }
}
