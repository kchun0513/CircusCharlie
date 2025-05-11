using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using UnityEngine;

public class WhipEffect : MonoBehaviour
{
    public TrailRenderer whipTrail;
    public AudioSource audioSource;
    public AudioClip whipSoundClip;

    public void PlayWhip()
    {
        StartCoroutine(ShowTrail());
        
        if (audioSource != null && whipSoundClip != null)
        {
            audioSource.PlayOneShot(whipSoundClip);
        }
    }

    IEnumerator ShowTrail()
    {
        whipTrail.emitting = true;
        yield return new WaitForSeconds(0.3f);
        whipTrail.emitting = false;
    }
}