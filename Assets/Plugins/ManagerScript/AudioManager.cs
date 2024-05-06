using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

namespace DuyTran
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        AudioSource music;
        AudioSource sound;
        public Sound[] musics;
        public Sound[] sounds;
        float musicVolume = 1;
        float soundVolume = 1;
        //int nowPlayingIndex;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            sound = gameObject.AddComponent<AudioSource>();
            //foreach (Sound s in sounds)
            //{
            //    s.source = sound;
            //    s.source.clip = s.clip;
            //    s.source.pitch = s.pitch;
            //    s.source.volume = s.volume;
            //    s.source.loop = s.loop;
            //}
            music = gameObject.AddComponent<AudioSource>();
        }
        private void Start()
        {
            SetVolumeSoundFX();
            SetVolumeMusicFX();
        }

        public void SetVolumeSoundFX()
        {
            var gameSetting = GameManager._instance.gameData;
            sound.volume = gameSetting.sound == true ? 1 : 0;
        }
        public void SetVolumeMusicFX()
        {
            var gameSetting = GameManager._instance.gameData;
            music.volume = gameSetting.music == true ? 1 : 0;
        }
        public void PlayMusic(string clipName, bool loop = true)
        {
            if (!GameManager._instance.gameData.music)
                return;
            string name = clipName.ToString();
            Sound s = Array.Find(musics, music => music.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            music.clip = s.clip;
            music.pitch = s.pitch;
            music.volume = s.volume * musicVolume;
            music.loop = s.loop;
            music.Play();
        }
        public void PlaySound(string clipName)
        {
            if (!GameManager._instance.gameData.sound)
                return;
            string name = clipName.ToString();
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            sound.clip = s.clip;
            sound.pitch = s.pitch;
            sound.volume = s.volume * soundVolume;
            sound.loop = s.loop;
            sound.Play();
        }
        public void StopMusic()
        {
            music.Stop();
        }
        public void StopSound()
        {
            sound.Stop();
        }
    }
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1;
        [Range(.1f, 3f)]
        public float pitch = 1;
        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }

}