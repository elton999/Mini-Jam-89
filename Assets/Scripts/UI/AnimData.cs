using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class AnimData : MonoBehaviour
{

    // Contains time-based functions to coordinate smooth animations
    [System.Serializable]
    public class GeneralAnimData {
        public List<float> times; // times are cumulative - 1,2,3 would last 3s
        public bool isEmpty() {
            return times.Count == 0;
        }
        public float animTime() {
            return times[times.Count-1];
        }
        // return i where times[i] is the greatest element less than t
        public int GetIndex(float t) {
            for (int i = 1; i < times.Count; i++) {
                if (times[i] >= t)
                    return i-1;
            }
            return times.Count-1;
        }
        // progression [0-1] of t from times[index] to times[index+1]
        public float GetProgress(int index, float t) {
            if (index < 0) return 0;
            if (index >= times.Count-1) return 1;
            float prog = (t - times[index]) / (times[index+1] - times[index]);
            return Mathf.Clamp(prog, 0, 1);
        }
        public float GetProgress(float t) {
            int index = GetIndex(t);
            return GetProgress(index, t);
        }
    }



    // For SpriteRenderers
    [System.Serializable] 
    public class RenderAnimData : GeneralAnimData {
        public List<Color> colors;
        public List<Vector2> localScales;
        public List<Vector2> localPositions;
        public List<float> localAngles;

        Color currentColor;
        Vector2 currentLocalScale;
        Vector2 currentLocalPos;
        float currentAngle;
        void UpdateCurrent(float t) {
            int index = GetIndex(t);
            int nextIndex = (index == times.Count-1)? index : index+1;
            float progress = GetProgress(index, t);

            if (colors.Count > 0) currentColor = Color.Lerp(colors[index], colors[nextIndex], progress);
            if (localScales.Count > 0) currentLocalScale = Vector2.Lerp(localScales[index], localScales[nextIndex], progress);
            if (localPositions.Count > 0) currentLocalPos = Vector2.Lerp(localPositions[index], localPositions[nextIndex], progress);
            if (localAngles.Count > 0) currentAngle = Mathf.Lerp(localAngles[index], localAngles[nextIndex], progress);
        }
        void ApplyCurrent(SpriteRenderer sr) {
            if (colors.Count > 0) sr.color = currentColor;
            if (localScales.Count > 0) sr.transform.localScale = new Vector3(currentLocalScale.x, currentLocalScale.y, sr.transform.localScale.z);
            if (localPositions.Count > 0) sr.transform.localPosition = new Vector3(currentLocalPos.x, currentLocalPos.y, sr.transform.localPosition.z);
            if (localAngles.Count > 0) sr.transform.localEulerAngles = new Vector3(sr.transform.localEulerAngles.x, sr.transform.localEulerAngles.y, currentAngle);
        }
        public void ApplyTo(SpriteRenderer sr, float t) {
            UpdateCurrent(t);
            ApplyCurrent(sr);
        }
        public void ApplyTo(List<SpriteRenderer> sr, float t) {
            UpdateCurrent(t);
            for (int i = 0; i < sr.Count; i++)
                ApplyCurrent(sr[i]);
        }
    }
    // For SpriteRenderers
    [System.Serializable] 
    public class SpriteAnimData : GeneralAnimData {
        public List<Sprite> sprites;

        Sprite currentSprite;
        void UpdateCurrent(float t) {
            int index = GetIndex(t);
            currentSprite = sprites[index];
        }
        void ApplyCurrent(SpriteRenderer sr) {
            sr.sprite = currentSprite;
        }
        public void ApplyTo(SpriteRenderer sr, float t) {
            if (isEmpty()) return;
            UpdateCurrent(t);
            ApplyCurrent(sr);
        }
        public void ApplyTo(List<SpriteRenderer> sr, float t) {
            if (isEmpty()) return;
            UpdateCurrent(t);
            for (int i = 0; i < sr.Count; i++)
                ApplyCurrent(sr[i]);
        }
    }

    
    // For UI (Images + TMP_Text)
    [System.Serializable] 
    public class UIAnimData : GeneralAnimData {
        public List<Color> colors;
        public List<Vector2> localScales;
        public List<Vector2> anchoredPositions;
        public List<float> localAngles;

        Color currentColor;
        Vector2 currentLocalScale;
        Vector2 currentAnchPos;
        float currentAngle;
        void UpdateCurrent(float t) {
            int index = GetIndex(t);
            int nextIndex = (index == times.Count-1)? index : index+1;
            float progress = GetProgress(index, t);

            if (colors.Count > 0) currentColor = Color.Lerp(colors[index], colors[nextIndex], progress);
            if (localScales.Count > 0) currentLocalScale = Vector2.Lerp(localScales[index], localScales[nextIndex], progress);
            if (anchoredPositions.Count > 0) currentAnchPos = Vector2.Lerp(anchoredPositions[index], anchoredPositions[nextIndex], progress);
            if (localAngles.Count > 0) currentAngle = Mathf.Lerp(localAngles[index], localAngles[nextIndex], progress);
        }
        void ApplyCurrent(Image img) {
            if (colors.Count > 0) img.color = currentColor;
            if (localScales.Count > 0) img.rectTransform.localScale = new Vector3(currentLocalScale.x, currentLocalScale.y, img.transform.localScale.z);
            if (anchoredPositions.Count > 0) img.rectTransform.anchoredPosition = currentAnchPos;
            if (localAngles.Count > 0) img.rectTransform.localEulerAngles = new Vector3(img.rectTransform.localEulerAngles.x, img.rectTransform.localEulerAngles.y, currentAngle);
        }
        void ApplyCurrent(TMP_Text txt) {
            if (colors.Count > 0) txt.color = currentColor;
            if (localScales.Count > 0) txt.rectTransform.localScale = currentLocalScale;
            if (anchoredPositions.Count > 0) txt.rectTransform.anchoredPosition = currentAnchPos;
            if (localAngles.Count > 0) txt.rectTransform.localEulerAngles = new Vector3(txt.rectTransform.localEulerAngles.x, txt.rectTransform.localEulerAngles.y, currentAngle);
        }
        public void ApplyTo(Image img, float t) {
            if (isEmpty()) return;
            UpdateCurrent(t);
            ApplyCurrent(img);
        }
        public void ApplyTo(TMP_Text txt, float t) {
            if (isEmpty()) return;
            UpdateCurrent(t);
            ApplyCurrent(txt);
        }
        public void ApplyTo(List<Image> img, List<TMP_Text> txt, float t) {
            UpdateCurrent(t);
            for (int i = 0; i < img.Count; i++)
                ApplyCurrent(img[i]);
            for (int i = 0; i < txt.Count; i++)
                ApplyCurrent(txt[i]);
        }
    }



    // For simple floats (applied by other scripts)
    // // used for Audio volume control
    [System.Serializable] 
    public class FloatAnimData : GeneralAnimData {
        public List<float> values;
        public float GetCurrent(float t) {
            int index = GetIndex(t);
            int nextIndex = (index == times.Count-1)? index : index+1;
            float progress = GetProgress(index, t);

            return Mathf.Lerp(values[index], values[nextIndex], progress);
        }
    }


    [System.Serializable] public class Invoker : UnityEvent { }


}
