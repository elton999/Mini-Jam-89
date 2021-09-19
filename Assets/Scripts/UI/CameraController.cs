using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [System.Serializable] public class Cutscene {
        public List<Vector2> path;
        public UIAnimations stopAnimation;
        public float moveTime;

        void CalculateSpeed() {
            float pathDistance = 0;
            for (int i = 0; i < path.Count-1; i++)
                pathDistance += Vector2.Distance(path[i], path[i+1]);
            moveSpeed = pathDistance / moveTime;
        }
        void ApplyPos(Vector2 p) {
            cc.transform.position = (Vector3)p + cc.offset;
        }
        public void Setup(CameraController _cc) {
            if (path.Count < 2)
                Debug.LogError("Cutscene path is unassigned.");
            cc = _cc;
            ApplyPos(path[0]);
            passedIndex = 0;
            CalculateSpeed();
        }
        public void Stop() { // not called by me
            if (stopAnimation == null) return;
            stopAnimation.gameObject.SetActive(true);
            stopAnimation.StartAnimation();
        }
        CameraController cc;
        int passedIndex = 0;
        float moveSpeed;
        public void MoveAlongPath(float deltaTime) {
            if (passedIndex >= path.Count-1) {
                cc.StopCutscene();
                return;
            }
            Vector2 p1 = path[passedIndex];
            Vector2 p2 = path[passedIndex+1];
            Vector2 direction = (p2 - p1).normalized;
            ApplyPos((Vector2)cc.transform.position + moveSpeed * deltaTime * direction);
            // If camera has passed p2
            if (Vector2.Distance(p1, cc.transform.position) >= Vector2.Distance(p1, p2)) {
                passedIndex++;
            }
        }
    }

    public Cutscene cutscene1;
    public Cutscene cutscene2;

    public void StartCutscene1() {
        currentCutscene = cutscene1;
        StartCutscene();
    }
    public void StartCutscene2() {
        currentCutscene = cutscene2;
        StartCutscene();
    }
    void StartCutscene() {
        finishedCutscene = false;
        currentCutscene.Setup(this);
    }
    public void StopCutscene() {
        finishedCutscene = true;
        if (currentCutscene != null)
            currentCutscene.Stop();
        currentCutscene = null;
    }
    [HideInInspector] public bool finishedCutscene = false;
    Cutscene currentCutscene = null;



    public Vector3 offset;
    public Transform target;
    Vector3 oldTargetPos;
    public float targetLerp = 0.9f;
    public float predictiveDistance = 2f;
    bool following = false;
    public void StartFollow() {
        following = true;
    }
    public void StopFollow() {
        following = false;
    }

    void Start() {
        if (target == null)
            Debug.LogError("Camera target is unassigned.");
        else transform.position = target.position + offset;
    }
    void Update() {
        if (currentCutscene != null) {
            currentCutscene.MoveAlongPath(Time.deltaTime);
            return;
        }

        if (following) {
            Vector3 targetPos = target.transform.position;
            targetPos += offset;
            targetPos += predictiveDistance * (Vector3)(target.transform.position - oldTargetPos).normalized;
            targetPos = ClampWithinBounds(targetPos);
            targetPos.z = offset.z;
            transform.position = Vector3.Lerp(transform.position, targetPos, targetLerp);

            oldTargetPos = target.transform.position;
        }
        
    }

    public List<Vector2> bounds;
    Vector2 ClampWithinBounds(Vector2 p) {
        if (bounds.Count < 2) {
            Debug.LogError("Insufficient camera bounding points.");
            return Vector3.zero;
        }
        int minIndex = -1;
        float minDistance = -1;
        for (int i = 0; i < bounds.Count-1; i++) {
            Vector2 p1 = bounds[i];
            Vector2 p2 = bounds[i+1];
            float area = (p2.x - p1.x) * (p2.y * p1.y);
            if (area == 0) continue;
            Vector2 clamped = ClampSingleRect(p, p1, p2);
            float distance = Vector2.Distance(p, clamped);
            if (distance < 0.001f) return p;
            if (distance < minDistance || minDistance == -1) {
                minDistance = distance;
                minIndex = i;
            }
        }
        return ClampSingleRect(p, bounds[minIndex], bounds[minIndex+1]);
    }
    Vector2 ClampSingleRect(Vector2 p, Vector2 min, Vector2 max) {
        float x = Mathf.Clamp(p.x, min.x, max.x);
        float y = Mathf.Clamp(p.y, min.y, max.y);
        return new Vector2(x, y);
    }


}
