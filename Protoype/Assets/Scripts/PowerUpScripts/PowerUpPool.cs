using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPool : MonoBehaviour
{
    public static PowerUpPool inst;
    private GameObject[] allPowerUps;
    private List<GameObject> pool;
    private int maxPowerUps = 4;
 
    private void Awake() {
        inst = this;
        if (pool != null) {
            pool.Clear();
        } else {
            pool = new List<GameObject>();
        }
    }
    // generate a power up pool to increase performance
    private void Start() {
        maxPowerUps *= PlatformPool.inst.numPlats;
        allPowerUps = Resources.LoadAll<GameObject>(Constants.POWER_UP_PATH);
        
        GameObject tmp;
        for (int i = 0; i < allPowerUps.Length; i++) {
            for (int j = 0; j < maxPowerUps; j++) {
                tmp = Instantiate(allPowerUps[i]);
                tmp.transform.parent = this.transform;
                tmp.name = "Bread";
                tmp.SetActive(false);
                pool.Add(tmp);
            }
        }
    }

    public GameObject getPowerUp() {
        for (int i = 0; i < pool.Count; i++) {
            // randomly choose a power up
            int powerUp = Random.Range(0, pool.Count);
            if (!pool[powerUp].activeInHierarchy) {
                return pool[powerUp];
            }
        }
        return null;
    }
}
