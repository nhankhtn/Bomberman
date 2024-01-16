
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")]
    [SerializeField] public AudioClip background;
    [SerializeField] public AudioClip deathEffect;
    [SerializeField] public AudioClip placeBombEffect;
    [SerializeField] public AudioClip bombExlpodesEffect;
    [SerializeField] public AudioClip collectItemEffect;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
