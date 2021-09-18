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
    
    public Slider sizeSlider;
    public Slider targetSizeSlider;
    public float maxSize = 2000;
    public float targetSize = 500;
    
    public void SetSize(float val) {
        SetSizeText(val);
        SetSizeSlider(val);
    }
    public TMP_Text sizeText;
    public string sizeUnits = "kg";
    public int sizeDecimals = 1;
    public int sizePercentDecimals = 0;
    void SetSizeText(float val) {
        float targetProgress = val / targetSize;
        float size = RoundDecimals(val, sizeDecimals);
        float percentage = RoundDecimals(100 * targetProgress, sizePercentDecimals);
        sizeText.text = val + " " + sizeUnits + " (" + percentage + "%)";
    }
    public List<Color> sizeSliderColors; // evenly spaced + blending
    public List<Image> sizeSliderFills;
    public List<bool> sizeSliderBlending;
    void SetSizeSliderFillColors(Color c) {
        for (int i = 0; i < sizeSliderFills.Count; i++)
            sizeSliderFills[i].color = c;
    }
    void SetSizeSlider(float val) {
        float sliderProgress = Mathf.Clamp(val / maxSize, 0, 1);
        sizeSlider.value = sliderProgress;
        if (sizeSliderColors.Count == 0) return;
        if (sizeSliderColors.Count == 1) {
            SetSizeSliderFillColors(sizeSliderColors[0]);
            return;
        }
        float colorIndex = sliderProgress * (sizeSliderColors.Count-1);
        int colorIndex1 = Mathf.FloorToInt(colorIndex);
        Color c = sizeSliderColors[colorIndex1];
        int colorIndex2 = colorIndex1+1;
        if (sizeSliderBlending[colorIndex1]) {
            float prog = (colorIndex - colorIndex1) / (colorIndex2 - colorIndex1);
            c = Color.Lerp(sizeSliderColors[colorIndex1], sizeSliderColors[colorIndex2], prog);
        }
        SetSizeSliderFillColors(c);
    }
    float RoundDecimals(float x, int d) {
        float mag = Mathf.Pow(10, d);
        return Mathf.Round(mag * x) / mag;
    }



    // Debug
    public float DebugSizeSlider = 0;
    void Update() {
        SetSize(DebugSizeSlider);
    }

}
