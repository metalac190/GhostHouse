using Mechanics.Feedback;
using UnityEngine;
using UnityEngine.Audio;
using Utility.Audio.Controllers;
using Utility.ObjectPooling;

namespace Utility.Audio.Managers
{
    public class SoundManager : MonoBehaviour
    {
        private const string DefaultManagerName = "Audio Manager";
        private const string DefaultMusicManagerName = "Music Manager";
        public const string DefaultMusicPlayerName = "Music Player";
        private const string DefaultSfxPoolName = "SFX Pool";
        private const string DefaultSfxPlayerName = "SFX Player";

        [SerializeField] private bool _scenePersistent = true;

        [Header("Music")]
        [SerializeField] private MusicManager _musicManager;
        [SerializeField] private AudioMixerGroup _musicGroup = null;

        [Header("Sfx")]
        [SerializeField] private SfxMaterialLibrary _materialLibrary = null;
        [SerializeField] private AudioMixerGroup _sfxGroup = null;
        [SerializeField] private Transform _poolParent;
        [SerializeField] private int _initialPoolSize = 5;

        private PoolManager<SfxPoolAudioSource> _poolManager = new PoolManager<SfxPoolAudioSource>();

        public static MusicManager MusicManager => Instance._musicManager;
        public static AudioMixerGroup MusicGroup => Instance._musicGroup;
        public static AudioMixerGroup SfxGroup => Instance._sfxGroup;

        #region Singleton

        private static SoundManager _instance;

        public static SoundManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<SoundManager>();
                    if (_instance == null) {
                        _instance = new GameObject(DefaultManagerName, typeof(SoundManager)).GetComponent<SoundManager>();
                    }
                }
                return _instance;
            }
        }

        private void Awake() {
            transform.SetParent(null);
            if (_instance == null) {
                if (_scenePersistent) {
                    DontDestroyOnLoad(gameObject);
                }
                _instance = this;
            }
            else if (_instance != this) {
                Destroy(gameObject);
            }

            #endregion

            // Create Music Manager
            if (_musicManager == null) {
                _musicManager = new GameObject(DefaultMusicManagerName, typeof(MusicManager)).GetComponent<MusicManager>();
                _musicManager.transform.SetParent(transform);
            }
            _musicManager.Setup();
            // Create SFX Pool
            if (_poolParent == null) {
                Transform pool = new GameObject(DefaultSfxPoolName).transform;
                pool.SetParent(transform);
                _poolParent = pool;
            }
            _poolManager.BuildInitialPool(_poolParent, DefaultSfxPlayerName, _initialPoolSize);
        }

        public void PlaySfx(SfxType type, Vector3 position = default) {
            _materialLibrary.GetSfx(type).Play(position);
        }

        public SfxPoolAudioSource GetController() {
            var source = _poolManager.GetObject();
            source.ResetSource();
            return source;
        }

        public void ReturnController(SfxPoolAudioSource source) {
            source.ResetSource();
            _poolManager.ReturnObject(source);
        }
    }
}