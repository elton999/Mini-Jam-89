using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioAnimations : AnimData
{

    // Fades in / out between days with DoFadeIn() and DoFadeOut()
    // Coordinates intros / loop sections of music
    // // customize with clips list

    AudioSource source;
    void Start() {
        source = GetComponent<AudioSource>();

        CalculatePauseTime();
        if (fadeInOnStart)
            DoFadeIn();
        if (startOnStart)
            StartAudio();
    }

    float pauseTimer = -1;
    void Update() {
        if (startedVolumeControl)
            UpdateVolumeControl();

        if (pauseTimer < 0) {
            if (started && !source.isPlaying)
                pauseTimer = 0;
        }
        else if (pauseTimer < currentPauseTime) {
            pauseTimer += Time.deltaTime;
        }
        else {
            AttemptNextClip();
            pauseTimer = -1;
        }
    }


    public List<AudioClip> clips;
    public bool startOnStart = false;
    public int repetitions = 1; // how many times a single loop is played. =-1 for looping
    int remReps = 0;
    public int loopStartIndex = 0;
    public bool randomizeLoops = false;
    public bool randomizeNonLoops = false;

    public float pauseTime = 0;
    public float pauseTimeVariation = 0;
    float currentPauseTime = 0;
    void CalculatePauseTime() {
        float r = 2*Random.value - 1;
        currentPauseTime = pauseTime + r * pauseTimeVariation;
    }



    int currentClipIndex = -1;
    void AttemptNextClip() {
        CycleClipIndex();
        if (remReps > 0) remReps--;
        bool canRepeat = (remReps > 0 || repetitions == -1);
        if (!canRepeat) {
            StopAudio();
            return;
        }

        if (currentClipIndex >= clips.Count) {
            // Restart from loopStartIndex
            if (loopStartIndex > 0 && loopStartIndex < clips.Count) {
                currentClipIndex = loopStartIndex;
            }
            // Restart from 0
            else currentClipIndex = 0;
        }

        // If still out of bounds
        if (currentClipIndex >= clips.Count) {
            StopAudio();
            return;
        }

        PlayClip();
    }
    int numCycles = 0;
    // bugs: remReps doesnt do full reps
    // // currentClipIndex never reaches the loop since it's always randomized
    void CycleClipIndex() {
        numCycles++;
        Debug.Log(currentClipIndex + " " + loopStartIndex);
        if (currentClipIndex >= loopStartIndex && numCycles < loopStartIndex) {
            // Looping clips
            if (randomizeLoops)
                currentClipIndex = RandomInLoop();
            else currentClipIndex++;
        }
        else {
            // Non-Looping clips
            if (randomizeNonLoops)
                currentClipIndex = RandomNonLoop();
            else currentClipIndex++;
        }
    }
    int RandomInLoop() {
        if (loopStartIndex >= clips.Count) {
            Debug.LogError("Cant find random looping audio clip because loop start index is out of bounds.");
            return -1;
        }
        return Random.Range(loopStartIndex, clips.Count);
    }
    int RandomNonLoop() {
        if (loopStartIndex <= 0) {
            Debug.LogError("Cant find random non-looping audio clip because all clips are loopable.");
            return -1;
        }
        return Random.Range(0, loopStartIndex);
    }



    bool started = false;
    public void StartAudio() {
        started = true;
        finished = false;
        remReps = repetitions;
        CycleClipIndex();
        PlayClip();
    }
    void PlayClip() {
        Debug.Log(currentClipIndex);
        source.clip = clips[currentClipIndex];
        source.Play();
    }
    public void StopAudio() {
        started = false;
        finished = true;
        remReps = 0;
        source.Stop();
        StopVolumeControl();
    }
    [HideInInspector] public bool finished = false;



    // Update volumeMultiplier based on the FloatAnimData
    [HideInInspector] public float volumeMultiplier = 1;
    public FloatAnimData volumeFadeOut;
    public FloatAnimData volumeFadeIn;
    FloatAnimData currentVolumeControlData;
    bool startedVolumeControl = false;
    bool finishedVolumeControl = false;
    public bool fadeInOnStart = false;
    public void DoFadeOut() {
        currentVolumeControlData = volumeFadeOut;
        startedVolumeControl = true;
        finishedVolumeControl = false;
    }
    public void DoFadeIn() {
        currentVolumeControlData = volumeFadeIn;
        startedVolumeControl = true;
        finishedVolumeControl = false;
    }
    float timerVC = -1;
    void UpdateVolumeControl() {
        bool canAnimVC = (currentVolumeControlData != null);
        bool shouldAnimVC = (startedVolumeControl && !finishedVolumeControl);
        if (timerVC < 0 && canAnimVC && shouldAnimVC) {
            timerVC = 0;
        }
        else if (timerVC >= 0) {
            timerVC += Time.deltaTime;
            volumeMultiplier = currentVolumeControlData.GetCurrent(timerVC);
        }
        else if (timerVC >= currentVolumeControlData.animTime()) {
            timerVC = -1;
            StopVolumeControl();
        }
    }
    void StopVolumeControl() {
        currentVolumeControlData = null;
        startedVolumeControl = false;
        finishedVolumeControl = true;
    }



}