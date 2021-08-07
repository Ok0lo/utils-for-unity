using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance { get; private set; }

    public enum Sound {
        SoundOne,
        SoundTwo,
    } 

    private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> soundAudioClipDictionary;
    private float defaultVolume = .5f;
    private float volume;

    private void Awake() {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
        
        volume = PlayerPrefs.GetFloat("soundVolume", defaultVolume);

        soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();
        foreach(Sound sound in System.Enum.GetValues(typeof(Sound))) {
            soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }

    public void PlaySound(Sound sound, float specialVolumeValue = -1f) {
        if(specialVolumeValue != -1f) {
            audioSource.PlayOneShot(soundAudioClipDictionary[sound], specialVolumeValue);
            return;
        }

        audioSource.PlayOneShot(soundAudioClipDictionary[sound], volume);
    }

    public void IncreaseVolume() {
        volume += .1f;
        volume = Mathf.Clamp01(volume);

        PlayerPrefs.SetFloat("soundVolume", volume);
    }

    public void DeincreaseVolume() {
        volume -= .1f;
        volume = Mathf.Clamp01(volume);

        PlayerPrefs.SetFloat("soundVolume", volume);
    }

    public float GetVolume() {
        return volume;
    }

}