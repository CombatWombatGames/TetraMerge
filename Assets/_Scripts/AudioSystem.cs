using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [SerializeField] AudioSource sfxSource = default;
    [SerializeField] AudioClip turnSfx = default;
    [SerializeField] AudioClip dropSfx = default;
    [SerializeField] AudioClip highlightSfx = default;
    [SerializeField] AudioClip mergeSfx = default;

    public void PlayTurnSfx()
    {
        sfxSource.pitch = 0.7f;
        sfxSource.PlayOneShot(turnSfx, Random.Range(0.2f, 0.3f));
    }

    public void PlayDropSfx()
    {
        sfxSource.pitch = 0.5f;
        sfxSource.PlayOneShot(dropSfx, 0.5f);
    }

    public void PlayHighlightSfx()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(highlightSfx, 0.3f);
    }

    public void PlayMergeSfx()
    {
        sfxSource.pitch = 0.75f;
        sfxSource.PlayOneShot(mergeSfx, 0.07f);
    }
}
