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
        animTime = uiData.animTime();
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
            uiData.ApplyTo(targetImages, targetTexts, timer);
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