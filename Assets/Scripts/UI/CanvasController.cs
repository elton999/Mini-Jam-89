using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasController : MonoBehaviour
{
    RectTransform rectTransform;
    CanvasScaler cs;
    CameraController cc;
    void Start() {
        rectTransform = GetComponent<RectTransform>();
        cs = GetComponent<CanvasScaler>();
        cc = Camera.main.GetComponent<CameraController>();
        cc.cc = this;

        ResetToDefault();
        escapeButton = exitButton;
        SetTargetSlider(targetSize / maxSize);
    }
    [Range(0, 1)] public float DebugSizeSlider = 0;
    void Update() {
        SetSize(DebugSizeSlider);

        if (cursor != null && Settings.customCursor)
            UpdateCustomCursor();
        if (Input.GetKeyDown(KeyCode.Escape))
            if (escapeButton != null) escapeButton.onClick.Invoke();
    }
    Button escapeButton;
    public Button exitButton;


    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;
    public Slider uiScale;
    public Toggle fullscreen;
    public Toggle customCursor;
    void PushSettingsToUI() {
        masterVolume.value = Settings.masterVolume;
        musicVolume.value = Settings.musicVolume;
        sfxVolume.value = Settings.sfxVolume;
        uiScale.value = Settings.uiScale;
        fullscreen.isOn = Settings.fullscreen;
        customCursor.isOn = Settings.customCursor;
    }
    public void ResetToDefault() {
        Settings.ResetToDefault();
        PushSettingsToUI();
        Save();
    }

    
    public AudioController ac;
    public void Save() {
        ac.SetMasterVolume(masterVolume.value);
        ac.SetMusicVolume(musicVolume.value);
        ac.SetSFXVolume(sfxVolume.value);
        SetUIscale(uiScale.value);
        SetFullscreen(fullscreen.isOn);
        SetCustomCursor(customCursor.isOn);
    }


    // OnValueChanged for instant feedback
    void SetUIscale(float val) {
        Settings.uiScale = val;
        ApplyUIscale();
    }
    public void SetFullscreen(bool val) {
        Settings.fullscreen = val;
        ApplyFullscreen();
    }
    public void SetCustomCursor(bool val) {
        Settings.customCursor = val;
        ApplyCustomCursor();
    }

    public Vector2 minRefRes;
    public Vector2 maxRefRes;
    void ApplyUIscale() {
        cs.referenceResolution = Vector2.Lerp(minRefRes, maxRefRes, Settings.uiScale);
    }
    void ApplyFullscreen() {
        Screen.SetResolution(Screen.width, Screen.height, Settings.fullscreen);
    }
    public Image cursor;
    void ApplyCustomCursor() {
        cursor.gameObject.SetActive(Settings.customCursor);
        Cursor.visible = !Settings.customCursor;
    }
    void UpdateCustomCursor() {
        //Debug.Log(Input.mousePosition + " " + cursor.rectTransform.anchoredPosition);
        Vector2 mousePos = Input.mousePosition;
        mousePos.x *= rectTransform.sizeDelta.x / Screen.width;
        mousePos.y *= rectTransform.sizeDelta.y / Screen.height;
        mousePos.y -= cursor.rectTransform.sizeDelta.y;
        cursor.rectTransform.anchoredPosition = mousePos;
    }



    public TMP_Text dayText;
    public int daysPerCycle = 5;
    public void SetDay(int val) {
        if (val == 0) val = 1;
        dayText.text = "Day " + (val * daysPerCycle);
    }
    public UIAnimations winAnim;
    public UIAnimations loseAnim;
    public AudioAnimations endMusic;
    public void DoEnding() {
        if (sizeSlider.value >= targetSizeSlider.value) {
            winAnim.gameObject.SetActive(true);
            winAnim.StartAnimation();
        }
        else {
            loseAnim.gameObject.SetActive(true);
            loseAnim.StartAnimation();
        }
        endMusic.StopAudio();
        endMusic.StartAudio();
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
    public TMP_Text targetSizeText;
    public string sizeUnits = "kg";
    public int sizeDecimals = 1;
    public int sizePercentDecimals = 0;
    void SetSizeText(float val) {
        float size = RoundDecimals(val * maxSize, sizeDecimals);
        float percentage = RoundDecimals(100 * val / targetSizeSlider.value, sizePercentDecimals);
        sizeText.text = size + " " + sizeUnits + " (" + percentage + "%)";
    }
    public List<Image> sizeSliderFills;
    public List<Color> sizeSliderColors; // evenly spaced + blending
    public List<bool> sizeSliderBlending;
    void SetSizeSliderFillColors(Color c) {
        for (int i = 0; i < sizeSliderFills.Count; i++)
            sizeSliderFills[i].color = c;
    }
    void SetSizeSlider(float val) {
        sizeSlider.value = val;
        if (sizeSliderColors.Count == 0) return;
        if (sizeSliderColors.Count == 1) {
            SetSizeSliderFillColors(sizeSliderColors[0]);
            return;
        }
        float colorIndex = val * (sizeSliderColors.Count-1);
        int colorIndex1 = Mathf.FloorToInt(colorIndex);
        if (colorIndex1 >= sizeSliderColors.Count-1) {
            SetSizeSliderFillColors(sizeSliderColors[sizeSliderColors.Count-1]);
            return;
        }
        Color c = sizeSliderColors[colorIndex1];
        int colorIndex2 = colorIndex1+1;
        if (sizeSliderBlending[colorIndex1] && sizeSliderBlending[colorIndex2]) {
            float prog = (colorIndex - colorIndex1) / (colorIndex2 - colorIndex1);
            c = Color.Lerp(sizeSliderColors[colorIndex1], sizeSliderColors[colorIndex2], prog);
        }
        SetSizeSliderFillColors(c);
    }
    public int targetSizeDecimals = 0;
    void SetTargetSlider(float val) {
        targetSizeSlider.value = val;
        float size = RoundDecimals(val * maxSize, targetSizeDecimals);
        targetSizeText.text = size + " " + sizeUnits;
    }
    float RoundDecimals(float x, int d) {
        float mag = Mathf.Pow(10, d);
        return Mathf.Round(mag * x) / mag;
    }
    // Alert messages
    public void StopAlerts() {
        waterAlert.StopAnimation(false);
        combatAlert.StopAnimation(false);
    }
    public UIAnimations waterAlert;
    public void DoWaterAlert() {
        waterAlert.StartAnimation();
    }
    public UIAnimations combatAlert;
    public void DoCombatAlert() {
        combatAlert.StartAnimation();
    }



    


    public void LoadScene(string name) {
        SceneManager.LoadScene(name);
    }
    [HideInInspector] public bool paused = false;
    public void Pause() {
        Time.timeScale = 0;
        paused = true;
    }
    public void Resume() {
        Time.timeScale = 1;
        paused = false;
    }
    public void SetEscButton(Button b) {
        escapeButton = b;
    }
    public Transform effectParent;
    public Transform textParent;
    public void SkipCutscene() {
        cc.StopCutscene(false);
        cc.StartFollow();
        StopUIanims(effectParent);
        StopUIanims(textParent);
    }
    void StopUIanims(Transform parent) {
        for (int i = 0; i < parent.childCount; i++) {
            UIAnimations uiAnim = parent.GetChild(i).GetComponent<UIAnimations>();
            if (uiAnim != null) uiAnim.StopAnimation(false);
            parent.GetChild(i).gameObject.SetActive(false);
        }
    }

}
