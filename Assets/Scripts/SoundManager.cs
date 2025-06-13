using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource bgmSource;
    public AudioClip[] stageBGMs;
    private bool isFadingOut;
    private float fadeOutDuration;
    private float fadeOutTimer;
    private float initialVolume = 1f;
    public AudioClip mainScreenBGM;

    public AudioSource crowdAudioSource;   // SFX 전용 AudioSource
    public AudioClip cheerClip;
    public AudioClip booClip;

    private AudioClip clipToPlayDelayed;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 저장된 볼륨(0~10) 불러와서 0~1로 변환하여 적용
            float master01 = PlayerPrefs.GetInt("MasterLevel", 10) / 10f;
            AudioListener.volume = master01;

            float bgm01 = PlayerPrefs.GetInt("BGMLevel", 10) / 10f;
            if (bgmSource != null)
                bgmSource.volume = bgm01;

            float sfx01 = PlayerPrefs.GetInt("SFXLevel", 10) / 10f;
            if (crowdAudioSource != null)
                crowdAudioSource.volume = sfx01;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayStageBGM(int stageIndex)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MainScreen" ||
            sceneName == "ScoreScreen" ||
            sceneName == "SettingScreen" ||
            sceneName == "ScoreBoardScreen")
        {
            if (bgmSource.isPlaying && bgmSource.clip == mainScreenBGM)
                return;

            bgmSource.clip = mainScreenBGM;
            bgmSource.loop = true;
            bgmSource.Play();  // 저장된 볼륨 유지
            return;
        }

        if (bgmSource.isPlaying && bgmSource.clip == stageBGMs[stageIndex])
            return;

        bgmSource.clip = stageBGMs[stageIndex];
        bgmSource.loop = true;
        bgmSource.Play();  // 저장된 볼륨 유지
    }

    public void StopBGM() => bgmSource.Stop();

    public void FadeOutBGM(float duration) => StartCoroutine(FadeOutCoroutine(duration));

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = bgmSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        bgmSource.volume = 0;
        bgmSource.Stop();
        bgmSource.volume = startVolume;
    }

    public void PlayCrowdReaction(bool isSuccess)
    {
        if (crowdAudioSource == null)
        {
            Debug.LogWarning("crowdAudioSource is not assigned!");
            return;
        }

        AudioClip clipToPlay = isSuccess ? cheerClip : booClip;
        if (clipToPlay == null)
        {
            Debug.LogWarning("Crowd clip is not assigned!");
            return;
        }

        clipToPlayDelayed = clipToPlay;
        Invoke(nameof(PlayCrowdClipNow), 0.2f);
    }

    private void PlayCrowdClipNow() => crowdAudioSource.PlayOneShot(clipToPlayDelayed);

    // BGM 볼륨 API
    public void SetBGMVolume(float volume01) => bgmSource.volume = Mathf.Clamp01(volume01);
    public void ChangeBGMVolume(float delta) => SetBGMVolume(bgmSource.volume + delta);

    // SFX 볼륨 API
    public void SetSFXVolume(float v) => crowdAudioSource.volume = Mathf.Clamp01(v);
    public void ChangeSFXVolume(float d) => SetSFXVolume(crowdAudioSource.volume + d);

    /// <summary>
    /// 범용 SFX 재생 API: 모든 효과음을 crowdAudioSource로 재생합니다.
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (crowdAudioSource != null && clip != null)
            crowdAudioSource.PlayOneShot(clip);
    }
}
