using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinAnimations : MonoBehaviour
{

    [HideInInspector] public PumpkinMechanics mech;
    SpriteRenderer pumpkin;
    Transform vinesParent;
    void Start() {
        mech = transform.Find("Pumpkin").GetComponent<PumpkinMechanics>();
        pumpkin = transform.Find("Pumpkin").GetComponent<SpriteRenderer>();
        vinesParent = transform.Find("Vines");
        SetPumpkinStage(0);
        SetPlantStage(0);
    }

    public List<Sprite> pumpkinSprites;
    public List<float> pumpkinStages; // 0-1
    public List<float> pumpkinScales; // localScale of pumpkin
    public void SetPumpkinStage(float progress) {
        float scale;
        int stageIndex1 = GetLowerIndex(progress, pumpkinStages);
        if (stageIndex1 == pumpkinStages.Count-1) {
            scale = pumpkinScales[stageIndex1];
        }
        else {
            int stageIndex2 = stageIndex1+1;
            float lerp = GetStageProgress(progress, stageIndex1, stageIndex2, pumpkinStages);
            scale = Mathf.Lerp(pumpkinScales[stageIndex1], pumpkinScales[stageIndex2], lerp);
        }
        pumpkin.transform.localScale = scale * Vector3.one;
        pumpkin.transform.localPosition = pumpkinStartPosY * scale / 2 * Vector3.up;
        int pumpIndex = Mathf.RoundToInt(progress * (pumpkinSprites.Count-1));
        pumpkin.sprite = pumpkinSprites[pumpIndex];
    }
    public float pumpkinStartPosY;

    public List<float> plantStages; // 0-1
    public List<int> plantSizes; // radius of vine circle
    public List<float> plantScales; // localScale of vines
    public void SetPlantStage(float progress) {
        int size;
        float scale;
        int stageIndex1 = GetLowerIndex(progress, plantStages);
        if (stageIndex1 == plantStages.Count-1) {
            size = plantSizes[stageIndex1];
            scale = plantScales[stageIndex1];
        }
        else {
            int stageIndex2 = stageIndex1+1;
            float lerp = GetStageProgress(progress, stageIndex1, stageIndex2, plantStages);
            size = Mathf.RoundToInt(Mathf.Lerp(plantSizes[stageIndex1], plantSizes[stageIndex2], lerp));
            scale = Mathf.Lerp(plantScales[stageIndex1], plantScales[stageIndex2], lerp);
        }
        DestroyCircle(vines);
        int radiusY = Mathf.RoundToInt(size * vineCircleSquash);
        vines = CreateSpriteCircle(vinesLeft, vinesMiddle, vinesRight, size, radiusY, vineCirclePower, scale, vinesParent);
    }
    [Range(0.25f, 4)] public float vineCirclePower = 2;
    List<List<SpriteRenderer>> vines;
    public GameObject vinesLeft;
    public List<GameObject> vinesMiddle;
    public GameObject vinesRight;
    [Range(0.1f, 1)] public float vineCircleSquash = 1;



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

    



    // Todo:
    // // add watering animation - expand circle outwards, but decrease scale for absorb effect
    // // add prune animation - cut a vine row


    SpriteRenderer CreateSprite(GameObject prefab, float x, float y, float localScale, Transform parent) {
        SpriteRenderer v = Instantiate(prefab, parent).GetComponent<SpriteRenderer>();
        v.transform.localPosition = new Vector2(x, y);
        v.transform.localScale = localScale * Vector3.one;
        return v;
    }
    List<SpriteRenderer> CreateSpriteRow(GameObject prefab, int width, float y, float localScale, Transform parent) {
        List<GameObject> mids = new List<GameObject>() { prefab };
        return CreateSpriteRow(prefab, mids, prefab, width, y, localScale, parent);
    }
    List<SpriteRenderer> CreateSpriteRow(GameObject prefabL, List<GameObject> prefabM, GameObject prefabR,
                        int width, float y, float localScale, Transform parent) {
        List<SpriteRenderer> row = new List<SpriteRenderer>();
        if (width == 0) return row;
        // Middle
        float x;
        for (int i = 0; i < width-1; i++) {
            int middleIndex1 = Random.Range(0, prefabM.Count);
            int middleIndex2 = Random.Range(0, prefabM.Count);
            x = localScale * (-0.5f - i);
            row.Add(CreateSprite(prefabM[middleIndex1], x, y, localScale, parent));
            row.Add(CreateSprite(prefabM[middleIndex2], -x, y, localScale, parent));
        }
        // End caps
        x = localScale * (0.5f + width-1);
        row.Add(CreateSprite(prefabL, x, y, localScale, parent));
        x = localScale * (-0.5f - (width-1));
        row.Add(CreateSprite(prefabR, x, y, localScale, parent));
        return row;
    }
    List<List<SpriteRenderer>> CreateSpriteCircle(GameObject prefabL, List<GameObject> prefabM, GameObject prefabR,
                        int radiusX, int radiusY, float pow, float localScale, Transform parent) {
        List<List<SpriteRenderer>> rows = new List<List<SpriteRenderer>>();
        if (radiusX == 0 || radiusY == 0) return rows;
        pow = Mathf.Clamp(pow, 0.25f, 4);
        for (int i = 0; i < radiusY; i++) {
            float y =  localScale * (0.5f + i);
            float circle = 1;
            if (radiusY > 1) circle = Mathf.Sqrt(1 - Mathf.Pow((float)i / (radiusY-1), pow));
            int rowWidth = Mathf.RoundToInt(radiusX * circle);

            rows.Add(CreateSpriteRow(prefabL, prefabM, prefabR, rowWidth, y, localScale, parent));
            rows.Add(CreateSpriteRow(prefabL, prefabM, prefabR, rowWidth, -y, localScale, parent));
        }
        return rows;
    }
    List<List<SpriteRenderer>> CreateSpriteCircle(GameObject prefab,
                        int radiusX, int radiusY, float pow, float localScale, Transform parent) {
        List<GameObject> mids = new List<GameObject>() { prefab };
        return CreateSpriteCircle(prefab, mids, prefab, radiusX, radiusY, pow, localScale, parent);
    }
    void DestroyCircle(List<List<SpriteRenderer>> rows) {
        if (rows == null) return;
        for (int i = 0; i < rows.Count; i++) {
            for (int j = 0; j < rows[i].Count; j++) {
                if (rows[i][j] != null)
                    Destroy(rows[i][j].gameObject);
            }
            rows[i].Clear();
        }
        rows.Clear();
    }




    [Range(0, 1)] public float DebugSize = 0;
    void Update() {
        //SetPlantStage(DebugSize);
        //SetPumpkinStage(DebugSize);
        //DestroyCircle(deb);
        //deb = CreateSpriteCircle(vinesLeft, 1, 1, 2, 0.5f, vinesParent);
    }
    List<List<SpriteRenderer>> deb;


}
