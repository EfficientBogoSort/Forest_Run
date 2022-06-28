using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPool : MonoBehaviour
{

    public int numPlats;
    public static PlatformPool inst;
    public GameObject platform;
    public int platCount = 0;

    private void Awake() {
        if (inst == null) {
            inst = this;
        }
        numPlats = 20;

    }

    
    // pre-instantiate all platforms
    private void Start() {
        GameObject tmp;
        for (int i = 0; i < numPlats; i++) {
            tmp = Instantiate(platform);
            tmp.transform.parent = this.transform;
            // place them consecutively
            tmp.transform.position = new Vector3(tmp.transform.position.x, tmp.transform.position.y,
                tmp.transform.position.z + tmp.transform.lossyScale.z * i);
        }
    }
}
