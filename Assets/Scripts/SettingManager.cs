using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// 세팅 화면에서 Master / BGM / SFX 볼륨을 조절합니다.
/// AudioMixer 없이 AudioListener와 SoundManager 인스턴스를 직접 사용합니다.
/// </summary>
public class SettingManager : MonoBehaviour
{
    [Header("Master Volume")]
    public Button masterMinusButton;
    public Button masterPlusButton;
    public TMP_Text masterValueText;
    private int masterLevel;

    [Header("BGM Volume")]
    public Button bgmMinusButton;
    public Button bgmPlusButton;
    public TMP_Text bgmValueText;
    private int bgmLevel;

    [Header("SFX Volume")]
    public Button sfxMinusButton;
    public Button sfxPlusButton;
    public TMP_Text sfxValueText;
    private int sfxLevel;

    public Button exitButton;

    private const int MinLevel = 0;
    private const int MaxLevel = 10;

    void Start()
    {
        // 1) 이전 저장값 로드
        masterLevel = PlayerPrefs.GetInt("MasterLevel", MaxLevel);
        bgmLevel    = PlayerPrefs.GetInt("BGMLevel",    MaxLevel);
        sfxLevel    = PlayerPrefs.GetInt("SFXLevel",    MaxLevel);

        // 2) UI 및 볼륨 적용
        UpdateMasterUI(); ApplyMasterVolume(masterLevel);
        UpdateBGMUI();    ApplyBGMVolume(bgmLevel);
        UpdateSFXUI();    ApplySFXVolume(sfxLevel);

        // 3) 버튼 콜백 등록
        masterMinusButton.onClick.AddListener(() => ChangeMasterLevel(-1));
        masterPlusButton .onClick.AddListener(() => ChangeMasterLevel(+1));
        bgmMinusButton   .onClick.AddListener(() => ChangeBGMLevel(-1));
        bgmPlusButton    .onClick.AddListener(() => ChangeBGMLevel(+1));
        sfxMinusButton   .onClick.AddListener(() => ChangeSFXLevel(-1));
        sfxPlusButton    .onClick.AddListener(() => ChangeSFXLevel(+1));

        // 4) 종료 버튼
        exitButton.onClick.AddListener(GoMain);
    }

    private void ChangeMasterLevel(int delta)
    {
        masterLevel = Mathf.Clamp(masterLevel + delta, MinLevel, MaxLevel);
        PlayerPrefs.SetInt("MasterLevel", masterLevel);
        UpdateMasterUI(); ApplyMasterVolume(masterLevel);
    }

    private void ChangeBGMLevel(int delta)
    {
        bgmLevel = Mathf.Clamp(bgmLevel + delta, MinLevel, MaxLevel);
        PlayerPrefs.SetInt("BGMLevel", bgmLevel);
        UpdateBGMUI(); ApplyBGMVolume(bgmLevel);
    }

    private void ChangeSFXLevel(int delta)
    {
        sfxLevel = Mathf.Clamp(sfxLevel + delta, MinLevel, MaxLevel);
        PlayerPrefs.SetInt("SFXLevel", sfxLevel);
        UpdateSFXUI(); ApplySFXVolume(sfxLevel);
    }

    private void UpdateMasterUI() => masterValueText.text = masterLevel.ToString();
    private void UpdateBGMUI()    => bgmValueText   .text = bgmLevel   .ToString();
    private void UpdateSFXUI()    => sfxValueText   .text = sfxLevel   .ToString();

    // Master: 0~10 -> 0~1 범위로 매핑하여 AudioListener.volume에 적용
    private void ApplyMasterVolume(int level)
    {
        float vol01 = level / (float)MaxLevel;
        AudioListener.volume = vol01;
    }

    // BGM: 0~10 -> 0~1 범위로 매핑하여 SoundManager에 적용
    private void ApplyBGMVolume(int level)
    {
        float vol01 = level / (float)MaxLevel;
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetBGMVolume(vol01);
    }

    // SFX: 0~10 -> 0~1 범위로 매핑하여 SoundManager에 적용
    private void ApplySFXVolume(int level)
    {
        float vol01 = level / (float)MaxLevel;
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetSFXVolume(vol01);
    }

    public void GoMain()
    {
        Destroy(GameManager.Instance.gameObject);
        GameManager.Instance.SceneChange(0);
    }
}
