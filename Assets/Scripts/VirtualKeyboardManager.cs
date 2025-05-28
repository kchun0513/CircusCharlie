using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VirtualKeyboardManager : MonoBehaviour {
    [Header("References")]
    public GameObject keyButtonPrefab;    // KeyButton 프리팹
    public Transform keysContainer;       // Grid Layout Group
    public TMP_InputField nameInput;      // ScoringScene의 NameInput
    public Button backspaceButton;
    public Button submitButton;

    private List<char> current = new List<char>();
    private const int MaxChars = 3;

    void Start() {
        // 1) 키 생성
        for (char c = 'A'; c <= 'Z'; c++) {
            var go = Instantiate(keyButtonPrefab, keysContainer);
            var kb = go.GetComponent<KeyButton>();
            kb.Init(c, OnKeyPressed);
        }
        // 2) 특수 버튼 이벤트 연결
        backspaceButton.onClick.AddListener(OnBackspace);
        submitButton.onClick.AddListener(OnSubmit);
        // 3) 초기 표시 갱신
        RefreshDisplay();
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
        if (current.Count == MaxChars) {
            string initials = new string(current.ToArray());
            Debug.Log("Entered Initials: " + initials);
            // TODO: GameManager.Instance.SetPlayerInitials(initials);
        } else {
            Debug.LogWarning("3글자를 입력해주세요.");
        }
    }

    void RefreshDisplay() {
        nameInput.text = new string(current.ToArray());
    }
}