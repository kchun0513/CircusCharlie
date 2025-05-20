using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingManager : MonoBehaviour
{
    public AudioMixer audioMixer;            // “MasterVolume” Exposed Param
    public Button masterMinusButton;
    public Button masterPlusButton;
    public TMP_Text masterValueText;
    private int masterLevel;
    private const int MinLevel = 0, MaxLevel = 10;

    [Header("Vignette Toggle (나중에 적용)")]
    public Toggle vignetteToggle;
    // (Vignette 적용은 다음 단계에서 여기에 로직 추가)

    public Button exitButton;

    private void Awake()
    {
        Debug.Log("[SettingManager] Awake() called");
    }

    private void Start()
    {
        Debug.Log("[SettingManager] Start() called");
        // 1) 이전 저장값 불러오기 (기본 10)
        masterLevel = PlayerPrefs.GetInt("MasterLevel", 10);

        // 2) UI 초기화
        UpdateMasterUI();

        // 3) 버튼 콜백 연결
        masterMinusButton.onClick.AddListener(() =>
        {
            ChangeMasterLevel(-1);
        });
        masterPlusButton.onClick.AddListener(() =>
        {
            ChangeMasterLevel(+1);
        });

        // 이걸 앞에 안넣어서 계속 실행이 안됐네. 해결
        if (exitButton == null)
        {
            Debug.LogError("[SettingManager] exitButton is null!");
        }
        else
        {
            Debug.Log("[SettingManager] exitButton assigned: " + exitButton.name);
            exitButton.onClick.AddListener(GoMain);
            Debug.Log("[SettingManager] exitButton listener registered");
        }

        // 4) Toggle 콜백 (추후 로직 연결)
        vignetteToggle.onValueChanged.AddListener(isOn =>
        {
            // TODO: Vignette 효과 On/Off 로직
            Debug.Log("Vignette toggled: " + isOn);
            PlayerPrefs.SetInt("UseVignette", isOn ? 1 : 0);
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

    public void GoMain()
    {
        Debug.Log("[Settings] Exit 버튼 클릭 → GoMain() 실행");
        Destroy(GameManager.Instance.gameObject);  // 재시작을 위해 
        GameManager.Instance.SceneChange(0);
    }

}
