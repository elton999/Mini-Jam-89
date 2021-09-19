using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public static float masterVolume;
    public static float musicVolume;
    public static float sfxVolume;
    public static float uiScale;
    public static bool fullscreen;
    public static bool customCursor;

    public static void ResetToDefault() {
        masterVolume = 0.5f;
        musicVolume = 1f;
        sfxVolume = 1f;
        uiScale = 0.75f;
        fullscreen = true;
        customCursor = true;
    }

}
