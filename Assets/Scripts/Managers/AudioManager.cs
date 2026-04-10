using UnityEngine;
using UnityEngine.Audio;

namespace ClickerGame.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer audioMixer;
        
        [Header("Audio Sources")]
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSource;
        
        [Header("Audio Clips")]
        [SerializeField] private AudioClip clickSound;
        [SerializeField] private AudioClip bgmClip;
        
        [Header("Default Volumes")]
        [SerializeField] private float defaultBGMVolume = 0.5f;
        [SerializeField] private float defaultSFXVolume = 0.5f;
        
        private const string BGM_VOLUME_KEY = "BGMVolume";
        private const string SFX_VOLUME_KEY = "SFXVolume";
        
        public float BGMVolume { get; private set; }
        public float SFXVolume { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            LoadVolumes();
        }
        
        private void LoadVolumes()
        {
            BGMVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, defaultBGMVolume);
            SFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, defaultSFXVolume);
            
            SetBGMVolume(BGMVolume);
            SetSFXVolume(SFXVolume);
        }
        
        public void SetBGMVolume(float volume)
        {
            BGMVolume = volume;
            
            if (audioMixer != null)
            {
                float db = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
                audioMixer.SetFloat("BGMVolume", db);
            }
            else if (bgmSource != null)
            {
                bgmSource.volume = volume;
            }
            
            PlayerPrefs.SetFloat(BGM_VOLUME_KEY, volume);
            PlayerPrefs.Save();
        }
        
        public void SetSFXVolume(float volume)
        {
            SFXVolume = volume;
            
            if (audioMixer != null)
            {
                float db = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
                audioMixer.SetFloat("SFXVolume", db);
            }
            else if (sfxSource != null)
            {
                sfxSource.volume = volume;
            }
            
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
            PlayerPrefs.Save();
        }
        
        public void PlayBGM(AudioClip clip, bool loop = true)
        {
            if (bgmSource != null && clip != null)
            {
                bgmSource.clip = clip;
                bgmSource.loop = loop;
                bgmSource.Play();
            }
        }
        
        public void StopBGM()
        {
            if (bgmSource != null)
            {
                bgmSource.Stop();
            }
        }
        
        public void PlaySFX(AudioClip clip)
        {
            if (sfxSource != null && clip != null)
            {
                sfxSource.PlayOneShot(clip);
            }
        }
        
        public void PlaySFXAtPoint(AudioClip clip, Vector3 position)
        {
            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, position, SFXVolume);
            }
        }
        
        public void PlayClickSound()
        {
            if (clickSound != null)
            {
                PlaySFX(clickSound);
            }
        }
        
        public void PlayBGM()
        {
            if (bgmClip != null)
            {
                PlayBGM(bgmClip, true);
            }
        }
    }
}