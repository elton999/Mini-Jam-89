using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinAnimations : MonoBehaviour
{

    public List<float> pumpkinStages; // 0-1
    public List<Sprite> pumpkinSprites;
    public List<float> pumpkinSizes; // localScale
    public void SetPumpkinStage(float progress) {

    }

    public List<float> plantStages; // 0-1
    public List<int> plantSizes; // radius of vine circle
    public void SetPlantStage(float progress) {
        int size;
        int stageIndex1 = GetLowerIndex(progress, plantStages);
        if (stageIndex1 == plantStages.Count-1) {
            size = plantSizes[stageIndex1];
        }
        else {
            int stageIndex2 = stageIndex1+1;
            float lerp = GetStageProgress(progress, stageIndex1, stageIndex2, plantStages);
            size = Mathf.RoundToInt(Mathf.Lerp(plantSizes[stageIndex1], plantSizes[stageIndex2], lerp));
        }
        CreateVineCircle(size);
    }
    [Range(0.5f, 3)] public float vineCirclePower = 2;
    List<SpriteRenderer> vines = new List<SpriteRenderer>();
    void CreateVineCircle(int radius) {
        DestroyVines();
        for (int i = 0; i < radius; i++) {
            float circle = Mathf.Sqrt(Mathf.Pow(radius, vineCirclePower) - Mathf.Pow(0.5f + i, vineCirclePower));
            int rowRad = Mathf.RoundToInt(circle);

            CreateVineRow(rowRad, 0.5f + i);
            CreateVineRow(rowRad, -0.5f - i);
        }
    }
    void DestroyVines() {
        for (int i = 0; i < vines.Count; i++) {
            if (vines[i] != null)
                Destroy(vines[i].gameObject);
        }
        vines.Clear();
    }
    public GameObject vinesLeft;
    public List<GameObject> vinesMiddle;
    public GameObject vinesRight;
    void CreateVineRow(int radius, float yPos) {
        // Right side
        for (int i = 0; i < radius-1; i++) {
            int middleIndex = Random.Range(0, vinesMiddle.Count);
            SpriteRenderer mid = Instantiate(vinesMiddle[middleIndex], transform).GetComponent<SpriteRenderer>();
            mid.transform.localPosition = new Vector2(0.5f + i, yPos);
            vines.Add(mid);
        }
        SpriteRenderer right = Instantiate(vinesLeft, transform).GetComponent<SpriteRenderer>();
        right.transform.localPosition = new Vector2(0.5f + radius-1, yPos);
        vines.Add(right);
        // Left side
        for (int i = 0; i < radius-1; i++) {
            int middleIndex = Random.Range(0, vinesMiddle.Count);
            SpriteRenderer mid = Instantiate(vinesMiddle[middleIndex], transform).GetComponent<SpriteRenderer>();
            mid.transform.localPosition = new Vector2(-0.5f - i, yPos);
            vines.Add(mid);
        }
        SpriteRenderer left = Instantiate(vinesLeft, transform).GetComponent<SpriteRenderer>();
        left.transform.localPosition = new Vector2(-0.5f - (radius-1), yPos);
        vines.Add(left);
    }



    int GetLowerIndex(float val, List<float> stages) {
        for (int i = 1; i < stages.Count; i++) {
            if (val < stages[i])
                return i-1;
        }
        return stages.Count-1;
    }
    float GetStageProgress(float val, int i1, int i2, List<float> stages) {
        return (val - stages[i1]) / (stages[i2] - stages[i1]);
    }

    [Range(0, 1)] public float DebugSize = 0;
    void Update() {
        SetPlantStage(DebugSize);
    }
}
