using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource bgmSource;
    public AudioClip[] stageBGMs;
    private bool isFadingOut = false;
    private float fadeOutDuration = 0f;
    private float fadeOutTimer = 0f;
    private float initialVolume = 1f;
    public AudioClip mainScreenBGM;
    public AudioSource crowdAudioSource;
    public AudioClip cheerClip;
    public AudioClip booClip;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayStageBGM(int stageIndex)
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Main BGM 화면 목록
        if (sceneName == "MainScreen" ||
            sceneName == "ScoreScreen" ||
            sceneName == "SettingScreen" ||
            sceneName == "ScoreBoardScreen")
        {
            if (bgmSource.isPlaying && bgmSource.clip == mainScreenBGM)
                return;

            bgmSource.clip = mainScreenBGM;
            bgmSource.loop = true;
            bgmSource.volume = 0.5f;
            bgmSource.Play();
            return;
        }

        if (bgmSource.isPlaying && bgmSource.clip == stageBGMs[stageIndex])
            return; // 이미 같은 곡이 재생 중이면 아무것도 안 함

        bgmSource.clip = stageBGMs[stageIndex];
        bgmSource.loop = true;
        bgmSource.volume = 1f;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void FadeOutBGM(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

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

        this.clipToPlayDelayed = clipToPlay;
        Invoke(nameof(PlayCrowdClipNow), 0.2f);
    }

    private AudioClip clipToPlayDelayed;

    private void PlayCrowdClipNow()
    {
        crowdAudioSource.PlayOneShot(clipToPlayDelayed);
    }
}
