using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimations : AnimData
{

    public SpriteAnimData frameData;
    public RenderAnimData renderData;
    public int repetitions = 1;
    public bool looping = false;
    public bool startOnStart = false;

    SpriteRenderer sr;
    void Start() {
        sr = GetComponent<SpriteRenderer>();
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
            frameData.ApplyTo(sr, timer);
            renderData.ApplyTo(sr, timer);
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
