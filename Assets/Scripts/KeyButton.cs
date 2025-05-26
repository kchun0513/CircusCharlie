using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyButton : MonoBehaviour {
    public char keyChar;            
    public Button button;           
    public TMP_Text labelText;      

    // 버튼 초기화: 문자와 클릭 콜백 연결
    public void Init(char c, System.Action<char> onClick) {
        keyChar = c;
        labelText.text = c.ToString();
        button.onClick.AddListener(() => onClick(c));
    }
}