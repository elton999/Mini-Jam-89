using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAnimations : AnimData
{

    // Ideally this is a singleton - only one in scene
    // // Handles volume adjustment from sliders
    // // plus fades in / out between days with StartAnimation()
    // // plus coordinates intros / loop sections of music

    public List<AudioSource> musicAudio;
    public List<AudioSource> sfxAudio;
    public void ApplyMasterVolume(float val) {

    }

    // Audio animations
    public FloatAnimData musicFadeOut;
    public FloatAnimData musicFadeIn;
    [HideInInspector] public float musicVolumeMultiplier = 0;


    public int repetitions = 1;
    public bool looping = false;
    public bool startOnStart = false;

    void Start() {
        CalculateAnimTime();
        if (startOnStart)
            StartAnimation();
    }
    float animTime = 0;
    void CalculateAnimTime() {
        //animTime = audioData.animTime();
    }


    float timer = 0;
    int remReps = 0;
    void Update() {
        bool canAnim = (remReps > 0 || looping);
        bool shouldAnim = (started && !finished);
        if (timer < 0 && canAnim && shouldAnim) {
            timer = 0;
        }
        else if (timer >= 0) {
            timer += Time.deltaTime;
            //audioData.ApplyTo(musicAudio, timer);
        }
        else if (timer >= animTime) {
            timer = -1;
            if (!looping) {
                remReps--;
                if (remReps <= 0) StopAnimation();
            }
        }
        
    }

    bool started = false;
    void StartAnimation() {
        started = true;
        finished = false;
        remReps = repetitions;
    }
    void StopAnimation() {
        started = false;
        finished = true;
        remReps = 0;
    }
    [HideInInspector] public bool finished = false;


}