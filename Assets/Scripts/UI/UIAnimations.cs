using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAnimations : AnimData
{

    public UIAnimData uiData;
    public List<Image> targetImages;
    public List<TMP_Text> targetTexts;
    public int repetitions = 1; // how many times a single loop is played
    public bool looping = false;
    public bool startOnStart = false;

    void Start() {
        if (startOnStart)
            StartAnimation();
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
        else if (timer < uiData.animTime()) {
            timer += Time.deltaTime;
            uiData.ApplyTo(targetImages, targetTexts, timer);
        }
        else {
            timer = -1;
            if (!looping) {
                remReps--;
                if (remReps <= 0) StopAnimation(true);
            }
        }
        
    }

    bool started = false;
    public void StartAnimation() {
        started = true;
        finished = false;
        remReps = repetitions;
    }
    public void StopAnimation(bool doStop) {
        started = false;
        finished = true;
        remReps = 0;
        if (doStop) onStop.Invoke();
    }
    [HideInInspector] public bool finished = false;
    public Invoker onStop;

}