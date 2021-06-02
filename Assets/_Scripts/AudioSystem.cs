using UnityEngine;
using DG.Tweening;

//Plays sounds
public class AudioSystem : MonoBehaviour
{
    [SerializeField] AudioSource sfxSource = default;
    [SerializeField] AudioClip[] Music = default;
    [SerializeField] AudioClip[] mergeStoneSfx = default;
    [SerializeField] AudioClip turnSfx = default;
    [SerializeField] AudioClip dropSfx = default;
    [SerializeField] AudioClip highlightSfx = default;
    [SerializeField] AudioClip mergeMagicSfx = default;
    [SerializeField] AudioClip buttonSfx = default;
    [SerializeField] AudioClip woodSfx = default;
    [SerializeField] AudioClip stoneSfx = default;
    [SerializeField] AudioClip boosterSfx = default;
    [SerializeField] AudioClip thunderSfx = default;
    [SerializeField] AudioClip raiseSfx = default;
    [SerializeField] AudioClip paperFastSfx = default;
    [SerializeField] AudioClip paperSlowSfx = default;
    [SerializeField] AudioClip chainSfx = default;

    public static AudioSystem Player;
    AudioSource musicSource;

    void Awake()
    {
        Player = this;
        musicSource = GameObject.Find("MusicSource").GetComponent<AudioSource>();
        musicSource.mute = PlayerPrefs.GetInt("Mute") == 1;
        if (musicSource.clip == null)
        {
            StartMusic();
        }
    }

    public void PlayTurnSfx()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(turnSfx, Random.Range(0.4f, 0.6f));
    }

    public void PlayDropSfx()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(dropSfx, 0.7f);
    }

    public void PlayHighlightSfx()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(highlightSfx, 0.6f);
    }

    public void PlayMergeSfx(int scale)
    {
        int force = 0;
        if (scale > 16)
        {
            force = 2;
        }
        else if (scale > 9)
        {
            force = 1;
        }
        sfxSource.pitch = Random.Range(0.7f, 0.9f);
        sfxSource.PlayOneShot(mergeStoneSfx[force], 0.2f);
        sfxSource.PlayOneShot(mergeMagicSfx, 0.1f);
    }

    public void PlayButtonSfx()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(buttonSfx, 0.2f);
    }

    public void PlayWoodSfx()
    {
        sfxSource.pitch = 0.75f;
        sfxSource.PlayOneShot(woodSfx, 0.2f);
    }

    public void PlayStoneSfx()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(stoneSfx, 0.2f);
    }

    public void PlayBoosterSfx()
    {
        sfxSource.pitch = 1.2f;
        sfxSource.PlayOneShot(boosterSfx, 0.4f);
    }

    public void PlayThunderSfx()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(thunderSfx, 0.4f);
    }

    public void PlayRaiseSfx()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(raiseSfx, 0.15f);
    }

    public void PlayPaperFastSfx()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(paperFastSfx, 0.15f);
    }

    public void PlayPaperSlowSfx()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(paperSlowSfx, 0.6f);
    }

    public void PlayChainSfx()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(chainSfx, 0.4f);
    }

    public void StartMusic()
    {
        musicSource.clip = Music[0]; //Music[Random.Range(0, Music.Length)];
        musicSource.Play();
    }

    public void RestartMusic()
    {
        if (musicSource.clip == Music[0])
        {
            musicSource.clip = Music[1];
        }
        else
        {
            musicSource.clip = Music[0];
        }
        musicSource.Play();
    }

    public void RestartMusicWithFading()
    {
        DOTween.Sequence()
            .Append(musicSource.DOFade(0f, 1f))
            .AppendCallback(() =>
            {
                if (musicSource.clip == Music[0])
                {
                    musicSource.clip = Music[1];
                }
                else
                {
                    musicSource.clip = Music[0];
                }
                musicSource.volume = 0.6f;
                musicSource.Play();
            });
    }

    public void MuteMusic()
    {
        musicSource.mute = !musicSource.mute;
        PlayerPrefs.SetInt("Mute", musicSource.mute ? 1 : 0);
    }
}
