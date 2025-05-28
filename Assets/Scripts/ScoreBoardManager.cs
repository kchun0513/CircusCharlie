using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class ScoreBoardManager : MonoBehaviour
{
    public Button exitButton;
    [Header("Top 5 Score UI")]
    public TMPro.TMP_Text[] initialsTexts = new TMPro.TMP_Text[5];
    public TMPro.TMP_Text[] scoreTexts    = new TMPro.TMP_Text[5];

    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(GoMain);
        LoadScores();
    }

    public void GoMain()
    {
        Debug.Log("[Settings] Exit 버튼 클릭 → GoMain() 실행");
        Destroy(GameManager.Instance.gameObject);  // 재시작을 위해 
        GameManager.Instance.SceneChange(0);
    }

    private void LoadScores() {
        // CSV 파일 로드 (persistentDataPath/ScoreRecord.csv)
        string csvPath = Path.Combine(Application.persistentDataPath, "ScoreRecord.csv");
        if (!File.Exists(csvPath)) {
            Debug.LogError($"ScoreRecord.csv not found at {csvPath}");
            return;
        }
        // 줄 단위로 분리하고 빈 줄 제거
        string[] lines = File.ReadAllLines(csvPath)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .ToArray();
        // 파싱하여 ScoreRecord 리스트로 변환
        List<ScoreRecord> all = new List<ScoreRecord>();
        foreach (var line in lines) {
            var parts = line.Split(',');
            if (parts.Length == 2 && int.TryParse(parts[0], out int sc)) {
                all.Add(new ScoreRecord(parts[1], sc));
            }
        }
        // 점수 기준 내림차순 정렬 후 상위 5개 선택
        var top5 = all
            .OrderByDescending(r => r.score)
            .Take(5)
            .ToList();
        // UI에 Top 5 할당
        for (int i = 0; i < initialsTexts.Length; i++) {
            if (i < top5.Count) {
                initialsTexts[i].text = top5[i].initials;
                scoreTexts[i].text    = top5[i].score.ToString();
            } else {
                initialsTexts[i].text = "";
                scoreTexts[i].text    = "";
            }
        }
    }
}

public class ScoreRecord {
    public string initials;
    public int score;

    public ScoreRecord(string initials, int score) {
        this.initials = initials;
        this.score = score;
    }
}
