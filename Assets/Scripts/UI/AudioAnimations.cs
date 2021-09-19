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
        if (clips.Count == 0) noAnims = true;
        if (noAnims) return;

        CalculatePauseTime();
        if (fadeInOnStart)
            DoFadeIn();
        if (startOnStart)
            StartAudio();
    }

    float pauseTimer = -1;
    void Update() {
        if (noAnims) return;

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
            CalculatePauseTime();
            AttemptNextClip();
            pauseTimer = -1;
        }
    }


    // easy access OneShot
    public void PlayOneShot() {
        source.PlayOneShot(source.clip);
    }
    public bool noAnims = false;

    // Advanced animations
    public List<AudioClip> clips;
    public bool startOnStart = false;
    public int repetitions = 1; // = -1 for looping
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
        //Debug.Log(currentPauseTime);
    }


    int numCycles = 0;
    int currentClipIndex = -1;
    void CalculateFirstClip() {
        if (loopStartIndex > 0 && randomizeNonLoops)
            currentClipIndex = RandomNonLoop();
        else if (randomizeLoops)
            currentClipIndex = RandomInLoop();
        else currentClipIndex = 0;
    }
    void AttemptNextClip() {
        CycleClipIndex();
        // Reset to beginning after loop
        if (numCycles >= clips.Count) {
            if (remReps > 0 || repetitions == -1) {
                numCycles = 0;
                CalculateFirstClip();
                remReps--;
            }
            else {
                StopAudio();
                return;
            }
        }

        PlayClip();
    }
    
    void CycleClipIndex() {
        //Debug.Log(currentClipIndex + " " + loopStartIndex);
        if (numCycles >= loopStartIndex-1) {
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
        numCycles++;
    }
    int RandomInLoop() {
        //Debug.Log("loop");
        if (loopStartIndex >= clips.Count) {
            Debug.LogError("Cant find random looping audio clip because loop start index is out of bounds.");
            return -1;
        }
        return Random.Range(loopStartIndex, clips.Count);
    }
    int RandomNonLoop() {
        //Debug.Log("non");
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
        remReps = repetitions-1;
        numCycles = 0;
        pauseTimer = -1;
        CalculateFirstClip();
        PlayClip();
    }
    void PlayClip() {
        //Debug.Log(currentClipIndex + " " + numCycles);
        source.clip = clips[currentClipIndex];
        source.Play();
    }
    public void StopAudio() {
        started = false;
        finished = true;
        remReps = 0;
        numCycles = 0;
        pauseTimer = -1;
        source.Stop();
        StopVolumeControl();
        onStop.Invoke();
    }
    [HideInInspector] public bool finished = false;
    public Invoker onStop;


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
        if (timerVC < 0) {
            if (canAnimVC && shouldAnimVC)
                timerVC = 0;
        }
        else if (timerVC < currentVolumeControlData.animTime()) {
            timerVC += Time.deltaTime;
            volumeMultiplier = currentVolumeControlData.GetCurrent(timerVC);
            //Debug.Log(volumeMultiplier);
        }
        else {
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