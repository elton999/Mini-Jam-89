using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSpawner : MonoBehaviour
{

    // This is a collection of similar tasks, not a single task bubble
    public class Task {
        GameObject prefab;
        Transform parent; // parent of spawned tasks
        Vector2 offset; // this lets tasks be shifted slightly to avoid overlap, not a perfect fix
        public Task(GameObject pfab, Transform p, Vector2 o) {
            prefab = pfab;
            parent = p;
            offset = o;
        }

        public void SpawnTaskAt(Vector2 position) {
            GameObject task = Instantiate(prefab, parent);
            if (IsTaskExactlyAt(position)) // this is another messy attempt to prevent task overlap
                task.transform.position = position + offset + 0.2f * Random.insideUnitCircle.normalized;
            else task.transform.position = position + offset;
            //task.transform.Find("Sprite").GetComponent<SpriteAnimations>().StartAnimation();
            numSpawnedTasks++;
        }
        public void DestroyTaskAt(Vector2 position) {
            GameObject closest = GetClosestTask(position);
            DestroyTask(closest);
        }
        public void DestroyTask(GameObject task) {
            Destroy(task.gameObject);
            currentTasks.Remove(task);
        }
        public void DestroyAll() {
            foreach (GameObject task in currentTasks)
                DestroyTask(task);
        }
        public void Reset() {
            DestroyAll();
            currentTasks.Clear();
            numSpawnedTasks = 0;
        }
        public GameObject GetClosestTask(Vector2 position) {
            int closestIndex = -1;
            float minDist = float.MaxValue;
            for (int i = 0; i < currentTasks.Count; i++) {
                float dist = Vector2.Distance(currentTasks[i].transform.position, position + offset);
                if (dist <= minDist) {
                    minDist = dist;
                    closestIndex = i;
                }
            }
            return currentTasks[closestIndex];
        }
        public List<GameObject> GetTasksWithinDistance(Vector2 position, float distance) {
            List<GameObject> inside = new List<GameObject>();
            foreach (GameObject task in currentTasks) {
                if (Vector2.Distance(task.transform.position, position + offset) <= distance)
                    inside.Add(task);
            }
            return inside;
        }
        public bool IsTaskExactlyAt(Vector2 position) {
            foreach (GameObject task in currentTasks) {
                if (Vector2.Distance(task.transform.position, position + offset) < 0.01f)
                    return true;
            }
            return false;
        }
        public int NumRemainingTasks() {
            return currentTasks.Count;
        }

        List<GameObject> currentTasks;
        int numSpawnedTasks = 0;
        public float GetTaskCompletion() { // after currentTasks has been modified, compare remaining to original.
            return 1 - currentTasks.Count / (float) numSpawnedTasks;
        }
    }

    public GameObject waterTaskBubble; // water, on inner vines, most days (when water is low)
    public GameObject pruneTaskBubble; // prune, on outer vines, all days except combat (when plant vines are too big, then size reduces next day)
    public GameObject unholyTaskBubble; // fertilize, on pumpkin, 1 day before combat
    public GameObject repairTaskBubble; // repair the damage dealt by the ghosts, on vines OR pumpkin, 1-2 days after combat. can also be solved by prune tool!

    public Task wateringTasks;
    public Task pruningTasks;
    public Task unholyTasks;
    public Task repairTasks;
    void Start() {
        wateringTasks = new Task(waterTaskBubble, transform, new Vector2(-0.3f, 0.3f));
        pruningTasks = new Task(pruneTaskBubble, transform, new Vector2(0.3f, 0.3f));
        unholyTasks = new Task(unholyTaskBubble, transform, new Vector2(-0.3f, -0.3f));
        repairTasks = new Task(repairTaskBubble, transform, new Vector2(0.3f, -0.3f));
    }
    
    

    // // Pumpkin anims are only updated between days
    // Tools can be used regardless of tasks. But when used near a task, it deletes that task for 
    // // This enables overwatering / overpruning (less growth), and overfertilizing (more combat days)
    // Pumpkin spawns more tasks as it gets bigger
    // Tasks remain across days - never destroyed unless completed.

    // Enemies always move directly toward pumpkin.
    // Enemies damage any vines they collide with - dont destroy
    // // Sometimes they'll stop to suck at a vine or the pumpkin, which reduces pumpkin size.
    // // After a suck is finished (time-based), a repair bubble is immediately spawned.
    // Damaged vines are included in the pumpkin growth limiter, but not included in the pumpkin growth enhancer

    // Note: call spm.AttemptAttack before spawning tasks
    [SerializeField] PumpkinAnimations pumpkin;
    [SerializeField] SpawnEnemiesManager spm;
    public List<float> repairProportions; // 0.5f at index 0 means: spawn 50% of the repair tasks 1 day after combat
    public void SpawnTasks(int daysSinceAttack) {
        bool needWater = pumpkin.mech.NeedsWater();
        bool needPrune = pumpkin.mech.NeedsPrune();
        Vector2 pumpkinPos = pumpkin.transform.position;
        if (needWater) {
            SpawnWateringTasks(pumpkinPos);
        }
        if (needPrune && daysSinceAttack > 0) {
            SpawnPruningTasks();
        }
        if (unholyTasks.NumRemainingTasks() == 0 && daysSinceAttack >= spm.daysBetweenAttacks-1) {
            SpawnUnholyTasks(pumpkinPos); // 1 day before combat
        }
        if (daysSinceAttack == 0) { // delete unholy tasks if the window is missed
            unholyTasks.Reset();
        }
        if (enemyDamagePoints.Count > 0 && daysSinceAttack > 0) {
            float proportion = repairProportions[daysSinceAttack-1];
            SpawnRepairTasks(proportion);
        }
    }
    public List<Vector2> enemyDamagePoints; // filled by Enemy script
    void SpawnRepairTasks(float proportion) {
        proportion = Mathf.Clamp(proportion, 0, 1);
        int numCurrentRepairs = Mathf.RoundToInt(proportion * enemyDamagePoints.Count);
        List<Vector2> currentRepairPoints = new List<Vector2>();
        // Decide which damagePoints to repair
        for (int i = 0; i < numCurrentRepairs; i++) {
            int index = Random.Range(0, enemyDamagePoints.Count);
            currentRepairPoints.Add(enemyDamagePoints[index]);
            enemyDamagePoints.RemoveAt(index);
        }
        // Spawn the tasks
        for (int i = 0; i < currentRepairPoints.Count; i++) {
            repairTasks.SpawnTaskAt(currentRepairPoints[i]);
        }
    }
    public float waterRandomVineChance = 0.15f;
    void SpawnWateringTasks(Vector2 pumpkinPos) {
        wateringTasks.SpawnTaskAt(pumpkinPos);
        if (Random.value < waterRandomVineChance)
            wateringTasks.SpawnTaskAt(pumpkin.GetRandomVine(true, false).transform.position);
    }
    public int pruneCount = 5; // needs to be updated with specific regions or something
    void SpawnPruningTasks() {
        for (int i = 0; i < pruneCount; i++) {
            SpriteRenderer vine = pumpkin.GetRandomVine(false, true); // fix overlap
            pruningTasks.SpawnTaskAt(vine.transform.position);
        }
    }
    void SpawnUnholyTasks(Vector2 pumpkinPos) {
        unholyTasks.SpawnTaskAt(pumpkinPos);
    }


}
