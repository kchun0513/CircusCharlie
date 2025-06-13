using System.Collections;
using UnityEngine;

public class WhipEffect : MonoBehaviour
{
    public TrailRenderer whipTrail;
    public AudioClip whipSoundClip;
    // (기존의 public AudioSource audioSource; 필드는 삭제)

    public void PlayWhip()
    {
        StartCoroutine(ShowTrail());
        // 로컬 audioSource는 더 이상 사용하지 않고 SoundManager로만 재생
        if (SoundManager.Instance != null && whipSoundClip != null)
            SoundManager.Instance.PlaySFX(whipSoundClip);
    }

    IEnumerator ShowTrail()
    {
        whipTrail.emitting = true;
        yield return new WaitForSeconds(0.3f);
        whipTrail.emitting = false;
    }
}
