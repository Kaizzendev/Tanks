using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioManager: MonoBehaviour
    {
        public static AudioManager Instance {get; private set;}
        
        [SerializeField] private AudioMixer _audioMixer;
        private AudioSource _playOneShotAudioSource;
        
        private float _masterVol = 0.5f;
        private float _musicVol = 0.5f;
        private float _sfxVol = 0.5f;
        
        private void Awake()
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
        
        private void Start()
        {
            _audioMixer.SetFloat("Master", ConvertToLogValue(_masterVol));
            _audioMixer.SetFloat("Music", ConvertToLogValue(_musicVol));
            _audioMixer.SetFloat("SFX", ConvertToLogValue(_sfxVol));
            
            _playOneShotAudioSource = gameObject.AddComponent<AudioSource>();
            
            _playOneShotAudioSource.loop = false;
            _playOneShotAudioSource.playOnAwake = false;
            _playOneShotAudioSource.spatialBlend = 0.0f;
            _playOneShotAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("SFX")[0];
        }
        
        public void SetVolume(AudioChannel channel, float value)
        {
            switch (channel)
            {
                case AudioChannel.Master:
                    _audioMixer.SetFloat("Master", ConvertToLogValue(value));
                    _masterVol = value;
                    break;
                case AudioChannel.Music:
                    _audioMixer.SetFloat("Music", ConvertToLogValue(value));
                    _musicVol = value;
                    break;
                case AudioChannel.SFX:
                    _audioMixer.SetFloat("SFX", ConvertToLogValue(value));
                    _sfxVol = value;
                    break;
                default:
                    Debug.LogError("AudioManager::SetVolume: ERROR IN SET VOLUME UNKOWN AUDIO CHANNEL: " + channel);
                    return;
            }
        }
        
        public float GetVolumeByChannel(AudioChannel channel)
        {
            switch (channel)
            {
                case AudioChannel.Master:
                    return _masterVol;
                case AudioChannel.Music:
                    return _musicVol;
                case AudioChannel.SFX:
                    return _sfxVol;
                default:
                    Debug.LogError($"ERROR_UNKNOWN_CHANNEL: {channel}");
                    return 0;
            }
        }
        
        private float ConvertToLogValue(float value)
        {
            return Mathf.Log10(value) * 20;
        }
        

        /// <summary>
        /// Play the sound on the SFX Channel
        /// </summary>
        /// <param name="clipToPlay">The audioClip to play</param>
        /// <param name="scale">The Scale of the volume default 1 that is equal to the SFX volume channel</param>
        public void PlayOneShot2D(AudioClip clipToPlay, float scale = 1.0f)
        {
            if (clipToPlay == null)
            {
                Debug.LogError("AudioManager::PlayOneShot: The audioClip is null");
                return;
            }
            _playOneShotAudioSource.PlayOneShot(clipToPlay, scale);
        }
        
    }
}