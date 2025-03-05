using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource shootingSound;
    [SerializeField] private AudioSource enemyShootingSound;
    [SerializeField] private AudioSource uiBackgroundMusic;
    [SerializeField] private AudioSource backgroundMusic;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip shootingClip;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayuiBackgroundMusic();
    }

    public void PlayShootingSound()
    {
        if (shootingSound != null && shootingClip != null)
        {
            shootingSound.volume = sfxVolume * masterVolume;
            shootingSound.pitch = Random.Range(0.8f, 1.2f);
            shootingSound.PlayOneShot(shootingClip);
        }
        else
        {
            Debug.LogError("Shooting sound or clip not assigned.");
        }
    }

    public void PlayenemyShootingSound()
    {
        if (enemyShootingSound != null)
        {
            enemyShootingSound.volume = sfxVolume * masterVolume;
            enemyShootingSound.pitch = Random.Range(0.8f, 1.2f);
            enemyShootingSound.Play();
        }
        else
        {
            Debug.LogError("Reload sound not assigned.");
        }
    }

    public void PlayuiBackgroundMusic()
    {
        if (uiBackgroundMusic != null)
        {
            uiBackgroundMusic.volume = musicVolume * masterVolume;
            uiBackgroundMusic.loop = true;
            uiBackgroundMusic.Play();
        }
        else
        {
            Debug.LogError("Empty magazine sound not assigned.");
        }
    }

    public void StopuiBackgroundMusic()
    {
        if (uiBackgroundMusic != null && uiBackgroundMusic.isPlaying)
        {
            uiBackgroundMusic.Stop();
        }
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.volume = musicVolume * masterVolume;
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusic != null && backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop();
        }
    }

    public void UpdateVolumes()
    {
        if (shootingSound != null)
            shootingSound.volume = sfxVolume * masterVolume;

        if (backgroundMusic != null)
            backgroundMusic.volume = musicVolume * masterVolume;

        if (enemyShootingSound != null)
            enemyShootingSound.volume = sfxVolume * masterVolume;

        if (uiBackgroundMusic != null)
            uiBackgroundMusic.volume = musicVolume * masterVolume;
    }
}