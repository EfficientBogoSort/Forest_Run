using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideWall : MonoBehaviour
{
    public int phase;
    private const float DisableDist = 40.0f;
    private bool wallDisabled = false;

    // materials for the different phases
    public Material easy;
    public Material notEasy;
    
    private void Awake() {
        this.name = Constants.WWALL_NAME;
        phase = Constants.PHASE[0];
        // initialize the wall dimensions according to the platform
        // dimensions
        for (int i = 0; i < 3; i++) {
            Transform child = transform.GetChild(i);
            child.transform.localScale = new Vector3(PlatformPool.inst.platform.transform.localScale.x / Constants.LANES,
                Movement.jumpHeight - 1, PlatformPool.inst.platform.transform.localScale.x / (Constants.LANES * 2));
            child.localPosition = new Vector3(PlatformPool.inst.platform.transform.localScale.x / Constants.LANES * (i-1),
               (PlatformPool.inst.platform.transform.localScale.y + child.localScale.y) / 2, child.localPosition.z);
            child.transform.GetChild(0).GetComponent<MeshRenderer>().material = easy;

        }
        chooseLane();
        
    }
    
    private void OnEnable() {
        // switch to phase 2
        if (Time.timeSinceLevelLoad > Constants.PHASE_LIMIT[0] && phase == Constants.PHASE[0]) {
            foreach (Transform child in transform) {
                child.localScale = new Vector3(child.localScale.x,
                    Movement.jumpHeight + 2, child.localScale.z);
                adjustHeight(child);

            }
            phase = Constants.PHASE[1];
            foreach (Transform child in transform) {
                child.transform.GetChild(0).GetComponent<MeshRenderer>().material = notEasy;
            }
            
            // switch to phase 3
        } 
        if (Time.timeSinceLevelLoad > Constants.PHASE_LIMIT[1] && phase == Constants.PHASE[1]) {
            phase = Constants.PHASE[2];
        }
        wallDisabled = false;

        // reactivate all walls
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
        if (phase < Constants.PHASE[2]) {
            chooseLane();
        }
    }
    private void Update() {
        // disable one of the walls when the player gets close to this obstacle instead of at spawn
        if (!wallDisabled && phase == Constants.PHASE[2]  &&
            (Movement.Instance.transform.position - transform.position).magnitude < DisableDist) {
            chooseLane();
            wallDisabled = true;
        }
    }
    private void chooseLane() {
        // select one lane to let the player through
        int delLane = Random.Range(0, 3);
        transform.GetChild(delLane).gameObject.SetActive(false);
    }

    // adjusts the position of the walls according to their height
    public void adjustHeight(Transform obs) {
        obs.localPosition = new Vector3(obs.localPosition.x,
            (PlatformPool.inst.platform.transform.localScale.y + obs.localScale.y) / 2,
            obs.localPosition.z);
    }

}
