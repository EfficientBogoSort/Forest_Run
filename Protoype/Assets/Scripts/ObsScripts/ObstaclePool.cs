using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    public static ObstaclePool inst;
    private GameObject[] allObstacles;
    private List<GameObject> pool;
    private GameObject lastObs;
    public float BaseSpawnChance;
    public float minSpawnChance = 0.7f;
    public static int phase;
    public GameObject PlatPool;

    private void Start() {
        InitializeAttr();
         int numPerObs = PlatPool.GetComponent<PlatformPool>().numPlats;
        GameObject tmp;

        // initialize all obstacles and add them to the pool
        for (int i = 0; i < allObstacles.Length; i++) {
            for (int j = 0; j < numPerObs; j++) {
                tmp = Instantiate(allObstacles[i]);
                tmp.transform.parent = this.transform;
                tmp.SetActive(false);
                pool.Add(tmp);
            }

        }

    }


    private void Update() {
        // spawn increases as phases increases
        if (phase == Constants.PHASE[0] && Time.timeSinceLevelLoad > Constants.PHASE_LIMIT[0]) {
            minSpawnChance -= 0.1f;
            phase = Constants.PHASE[1];
        } else if (phase == Constants.PHASE[1] && Time.timeSinceLevelLoad > Constants.PHASE_LIMIT[1]) {
            minSpawnChance -= 0.1f;
            phase = Constants.PHASE[2];
        }
    }

    // find inactive instances to set active and use
    public GameObject getObstacle() {
        for (int i = 0; i < pool.Count; i++) {
            int obs = Random.Range(0, pool.Count);
            if (!pool[obs].activeInHierarchy && (lastObs == null || lastObs.name != pool[obs].name)) {
                lastObs = pool[obs];
                return pool[obs];
            }
        }
        return null;
    }

    // reset all parameters
    private void InitializeAttr() {
        if (!inst) {
            inst = this;
        }
        if (pool != null) {
            pool.Clear();
        }
        lastObs = null;
        phase = Constants.PHASE[0];
        minSpawnChance = BaseSpawnChance;
        if (allObstacles == null) {
            allObstacles = Resources.LoadAll<GameObject>(Constants.OBS_PATH);
        }
        if (pool == null) {
            pool = new List<GameObject>();
        } else {
            pool.Clear();
        }
    }
}
