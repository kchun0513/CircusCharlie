using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsMenuController : MonoBehaviour
{
    [Header("Audio Mixer & Levels")]
    public AudioMixer audioMixer;            // “MasterVolume” Exposed Param
    public Button masterMinusButton;
    public Button masterPlusButton;
    public TMP_Text masterValueText;
    private int masterLevel;
    private const int MinLevel = 0, MaxLevel = 10;

    [Header("Vignette Toggle (나중에 적용)")]
    public Toggle vignetteToggle;
    // (Vignette 적용은 다음 단계에서 여기에 로직 추가)

    [Header("Exit Button")]
    public Button exitButton;
    public string mainSceneName = "MainScreen";  // 돌아갈 씬 이름

    private void Start()
    {
        // 1) 이전 저장값 불러오기 (기본 5)
        masterLevel = PlayerPrefs.GetInt("MasterLevel", 10);

        // 2) UI 초기화
        UpdateMasterUI();

        // 3) 버튼 콜백 연결
        masterMinusButton.onClick.AddListener(() => {
            ChangeMasterLevel(-1);
        });
        masterPlusButton.onClick.AddListener(() => {
            ChangeMasterLevel(+1);
        });

        // 4) Toggle 콜백 (추후 로직 연결)
        vignetteToggle.onValueChanged.AddListener(isOn => {
            // TODO: Vignette 효과 On/Off 로직
            Debug.Log("Vignette toggled: " + isOn);
            PlayerPrefs.SetInt("UseVignette", isOn ? 1 : 0);
        });

        // 5) Exit 버튼 콜백
        exitButton.onClick.AddListener(() => {
            SceneManager.LoadScene(mainSceneName);
        });
    }

    private void ChangeMasterLevel(int delta)
    {
        masterLevel = Mathf.Clamp(masterLevel + delta, MinLevel, MaxLevel);
        PlayerPrefs.SetInt("MasterLevel", masterLevel);
        UpdateMasterUI();

        // 0~10 을 -80dB~0dB 로 매핑
        float db = Mathf.Lerp(-80f, 0f, masterLevel / (float)MaxLevel);
        audioMixer.SetFloat("MasterVolume", db);
    }

    private void UpdateMasterUI()
    {
        masterValueText.text = masterLevel.ToString();
    }
}
