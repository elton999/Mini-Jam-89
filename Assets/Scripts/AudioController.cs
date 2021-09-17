using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : AnimData
{

    // Singleton - only one should be added to scene
    // // volume adjustment from UI

    // Guide to audio:
    // // for music, assign a reference to MusicAnimations, then mu.StartMusic() and mu.StopMusic()
    // // for SFX, assign a ref to AudioSource, then call src.PlayOneShot(src.clip)

    public List<AudioSource> musicAudio; // tag sources with Music or add here
    public List<AudioSource> sfxAudio; // tag sources with SFX or add here
    void Start() {
        if (musicAudio.Count == 0)
            musicAudio = FindSourcesByTag("Music");
        if (sfxAudio.Count == 0)
            sfxAudio = FindSourcesByTag("SFX");
    }
    List<AudioSource> FindSourcesByTag(string tag) {
        List<AudioSource> sources = new List<AudioSource>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < objects.Length; i++)
            sources.Add(objects[i].GetComponent<AudioSource>());
        return sources;
    }



    // UI volume sliders call these OnValueChanged
    public void SetMasterVolume(float val) {
        Settings.masterVolume = val;
        ApplyMusicVolume();
        ApplySFXVolume();
    }
    public void SetMusicVolume(float val) {
        Settings.musicVolume = val;
        ApplyMusicVolume();
    }
    public void SetSFXVolume(float val) {
        Settings.sfxVolume = val;
        ApplySFXVolume();
    }
    // Change volume on the actual audio sources
    void ApplyMusicVolume() {
        float combined = Settings.masterVolume * Settings.musicVolume;
        for (int i = 0; i < musicAudio.Count; i++) {
            MusicAnimations ma = musicAudio[i].GetComponent<MusicAnimations>();
            float musicVolumeMultiplier = (ma == null)? 1 : ma.musicVolumeMultiplier;
            musicAudio[i].volume = combined * musicVolumeMultiplier;
        }
    }
    void ApplySFXVolume() {
        float combined = Settings.masterVolume * Settings.sfxVolume;
        for (int i = 0; i < sfxAudio.Count; i++)
            sfxAudio[i].volume = combined;
    }

    
}
