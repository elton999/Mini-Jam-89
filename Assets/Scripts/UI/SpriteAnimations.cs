using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimations : AnimData
{

    public SpriteAnimData frameData;
    public RenderAnimData renderData;
    public int repetitions = 1; // how many times a single loop is played
    public bool looping = false;
    public bool startOnStart = false;

    public SpriteRenderer sr;
    void Start() {
        CalculateAnimTime();
        if (startOnStart)
            StartAnimation();
    }
    float animTime = 0;
    void CalculateAnimTime() {
        if (!frameData.isEmpty()) {
            animTime = frameData.animTime();
            if (!renderData.isEmpty() && renderData.animTime() != animTime)
                Debug.LogError("Frame and render times must match.");
        }
        if (!renderData.isEmpty())
            animTime = renderData.animTime();
    }


    float timer = -1;
    int remReps = 0;
    void Update() {
        bool canAnim = (remReps > 0 || looping);
        bool shouldAnim = (started && !finished);
        if (timer < 0) {
            if (canAnim && shouldAnim)
                timer = 0;
        }
        else if (timer < animTime) {
            timer += Time.deltaTime;
            frameData.ApplyTo(sr, timer);
            renderData.ApplyTo(sr, timer);
        }
        else {
            timer = -1;
            if (!looping) {
                remReps--;
                if (remReps <= 0) StopAnimation();
            }
        }
        
    }

    bool started = false;
    public void StartAnimation() {
        started = true;
        finished = false;
        remReps = repetitions;
    }
    public void StopAnimation() {
        started = false;
        finished = true;
        remReps = 0;
        onStop.Invoke();
    }
    [HideInInspector] public bool finished = false;
    public Invoker onStop;

}
