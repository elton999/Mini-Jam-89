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
        if (timer < 0 && canAnim && shouldAnim) {
            timer = 0;
        }
        else if (timer >= 0) {
            timer += Time.deltaTime;
            uiData.ApplyTo(targetImages, targetTexts, timer);
        }
        else if (timer >= uiData.animTime()) {
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
    }
    [HideInInspector] public bool finished = false;


}