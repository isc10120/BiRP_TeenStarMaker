using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    AudioSource sfxSource;

    void Awake()
    {
        if (!sfxSource) sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip) sfxSource.PlayOneShot(clip);
    }
}