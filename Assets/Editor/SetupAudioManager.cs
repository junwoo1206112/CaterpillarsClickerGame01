using UnityEngine;
using UnityEditor;
using ClickerGame.Managers;

namespace ClickerGame.EditorTools
{
    public class SetupAudioManager
    {
        [MenuItem("Tools/Game/Setup AudioManager")]
        public static void Setup()
        {
            // Check if AudioManager exists
            AudioManager manager = Object.FindFirstObjectByType<AudioManager>();
            
            if (manager != null)
            {
                Debug.Log("[SetupAudioManager] AudioManager already exists in scene!");
                return;
            }
            
            // Create AudioManager GameObject
            GameObject audioObj = new GameObject("AudioManager");
            manager = audioObj.AddComponent<AudioManager>();
            
            // Add AudioSources
            AudioSource bgmSource = audioObj.AddComponent<AudioSource>();
            bgmSource.playOnAwake = false;
            bgmSource.loop = true;
            
            AudioSource sfxSource = audioObj.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            sfxSource.loop = false;
            
            // Assign via SerializedObject
            SerializedObject so = new SerializedObject(manager);
            so.FindProperty("bgmSource").objectReferenceValue = bgmSource;
            so.FindProperty("sfxSource").objectReferenceValue = sfxSource;
            so.ApplyModifiedProperties();
            
            Debug.Log("[SetupAudioManager] AudioManager created with AudioSources!");
            Debug.Log("[SetupAudioManager] AudioManager will call DontDestroyOnLoad in Play mode");
        }
        
        [MenuItem("Tools/Game/Create Audio Folder")]
        public static void CreateAudioFolder()
        {
            string sfxPath = "Assets/Audio/SFX";
            string bgmPath = "Assets/Audio/BGM";
            
            if (!AssetDatabase.IsValidFolder("Assets/Audio"))
            {
                AssetDatabase.CreateFolder("Assets", "Audio");
            }
            
            if (!AssetDatabase.IsValidFolder("Assets/Audio/SFX"))
            {
                AssetDatabase.CreateFolder("Assets/Audio", "SFX");
            }
            
            if (!AssetDatabase.IsValidFolder("Assets/Audio/BGM"))
            {
                AssetDatabase.CreateFolder("Assets/Audio", "BGM");
            }
            
            AssetDatabase.Refresh();
            
            Debug.Log("[SetupAudioManager] Audio folders created:");
            Debug.Log($"  - {bgmPath}");
            Debug.Log($"  - {sfxPath}");
        }
    }
}
