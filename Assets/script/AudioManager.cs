using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgMusicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip bgMusic;
    public AudioClip cardFlip;
    public AudioClip match;
    public AudioClip mismatch;
    public AudioClip gameWin;
    public AudioClip gameOver;

    [Header("Mute Button UI")]
    public Image muteButtonImage;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private bool isMuted = false;

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

    void Start()
    {
        if (bgMusicSource && bgMusic)
        {
            bgMusicSource.clip = bgMusic;
            bgMusicSource.loop = true;
            bgMusicSource.volume = 0.2f;
            bgMusicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource && clip)
            sfxSource.PlayOneShot(clip);
    }


    public void ToggleMute()
    {
        isMuted = !isMuted;

        if (bgMusicSource)
            bgMusicSource.mute = isMuted;

        if (sfxSource)
            sfxSource.mute = isMuted;

        UpdateMuteUI();
    }

    void UpdateMuteUI()
    {
        if (muteButtonImage == null)
            return;

        muteButtonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }

    public void Quit()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
}
