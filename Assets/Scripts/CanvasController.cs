using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasController : MonoBehaviour
{
    CanvasScaler cs;
    void Start() {
        cs = GetComponent<CanvasScaler>();

        ResetToDefault();
    }

    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;
    public Slider uiScale;
    public Toggle fullscreen;
    public Toggle customCuror;
    void PushSettingsToUI() {
        masterVolume.value = Settings.masterVolume;
        musicVolume.value = Settings.musicVolume;
        sfxVolume.value = Settings.sfxVolume;
        uiScale.value = Settings.uiScale;
        fullscreen.isOn = Settings.fullscreen;
        customCuror.isOn = Settings.customCursor;
    }
    public void ResetToDefault() {
        Settings.ResetToDefault();
        PushSettingsToUI();
    }

    
    public Vector2 minRefRes;
    public Vector2 maxRefRes;
    void ApplyUIscale() {
        cs.referenceResolution = Vector2.Lerp(minRefRes, maxRefRes, Settings.uiScale);
    }
    void ApplyFullscreen() {
        Screen.SetResolution(Screen.width, Screen.height, Settings.fullscreen);
    }
    void ApplyCustomCursor() {

    }
    void FindCustomCursor() {

    }



    public TMP_Text dayText;
    public void SetDay(int val) {
        dayText.text = "Day " + val;
    }
    public TMP_Text sizeText;
    public Slider sizeSlider;
    public void SetSize(float val) {

    }
}
