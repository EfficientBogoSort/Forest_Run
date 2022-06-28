using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    private float speed = 2f;
    private const float Wait = 2f;
    private float waitTime = 0;
    private bool waiting = false;
    private bool goingSide = true;
    private Transform osci = null;
    private int phase = Constants.PHASE[0];
    private float initPos;
    private int dir = 1;
    private float standTime = 0;
    private const float AvgStandTime = 2f;
    private const float Deviation = .5f;
    private float maxStandtime;
    private float travelDist;
    private const float SmallSize = 3;
    private const float BigSize = 6;


    public Material easy;
    public Material medium;
    public Material hard;

    private void Awake() {
        osci = transform.GetChild(0).transform;

        osci.localScale = new Vector3(SmallSize, SmallSize, SmallSize);

        osci.localPosition = new Vector3(osci.localPosition.x, transform.localScale.y / 2, osci.localPosition.z);
        name = Constants.OSCI_NAME;
        travelDist = Mathf.Abs(PlatformPool.inst.platform.transform.localScale.x / Constants.LANES);
        setMaterial(easy);
        spawn();
    }

    private void OnEnable() {

        // transition to phase 2 of the game
        if (Time.timeSinceLevelLoad > Constants.PHASE_LIMIT[0] && phase == Constants.PHASE[0]) {
            osci.localScale = new Vector3(BigSize, BigSize, BigSize);
            setMaterial(medium);
            speed *= 6;
            phase = Constants.PHASE[1];
        } else if (Time.timeSinceLevelLoad > Constants.PHASE_LIMIT[1] && phase == Constants.PHASE[1]) {
            phase = Constants.PHASE[2];
            setMaterial(hard);
        }

        spawn();
        
        
    }
    void Update()
    {
        // decide what type of movement to do
        if (phase < Constants.PHASE[2]) {
            move();
        } else {
            teleport();
        } 
    }


    private void move() {
        // not in cool down
        if (!waiting) {
            // going out of the initial position
            if (goingSide) {
                osci.localPosition += dir * speed * Time.deltaTime * Vector3.right;
                // change direction
                if (Mathf.Abs(osci.localPosition.x - initPos) >= travelDist) {
                    goingSide = false;
                    waiting = true;
                }
              // returning to initial position  
            } else {
                osci.localPosition -= dir * speed * Time.deltaTime * Vector3.right;
                // change direction
                if ((osci.localPosition.x - initPos) * dir < 0) {
                    goingSide = true;
                    waiting = true;
                }
            }
        } else {
            waitTime++;
            if (waitTime * Time.deltaTime >= Wait) {
                waiting = false;
                waitTime = 0;
            }
        }
    }
    // oscillator will teleport to the other lane after a specific amount
    // of time 
    private void teleport() {
        if (standTime * Time.deltaTime > maxStandtime) {
            if (goingSide) {
                osci.localPosition = new Vector3(initPos + dir * PlatformPool.inst.platform.transform.localScale.x / Constants.LANES,
                    osci.localPosition.y, osci.localPosition.z);
                goingSide = false;
            } else {
                osci.localPosition = new Vector3(initPos, osci.localPosition.y, osci.localPosition.z);
                goingSide = true;
            }
            standTime = 0;
        } else {
            standTime++;
        }
    }

    private void spawn() {
        // setting initial state of oscillator
        waiting = false;
        waitTime = 0;
        int lane = Random.Range(-1, 2);
        initPos = lane * PlatformPool.inst.platform.transform.localScale.x / Constants.LANES;
        osci.localPosition = new Vector3(initPos, osci.localPosition.y, osci.localPosition.z);

        standTime = 0;
        if (phase == Constants.PHASE[2]) {
            maxStandtime = Random.Range(AvgStandTime - Deviation, AvgStandTime + Deviation);
        }


        if (lane == 0) { // middle lane, pick either move to right or left
            dir = Random.Range(0, 2);
            if (dir == 0) {
                dir--;
            }

        } else if (lane == 1) { // right lane, move to the left
            dir = -1;
        } else if (lane == -1) { // left lane, move to the right
            dir = 1;
        }
    }

    private void setMaterial(Material material) {
        transform.GetChild(0).transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = material;
    }

}
