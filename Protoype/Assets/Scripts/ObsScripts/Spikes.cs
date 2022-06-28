using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private bool goUp = true;
    private float speed = 5;
    private const float SpeedInc = 5;
    private bool waiting = false;
    private float waitTime = 0;
    private float wait = 2f;
    private const float waitReduct = 4;
    private const float Offset = 0.1f;
    private int phase = Constants.PHASE[0];
    private const float InstantTrigger = 0.8f;
    private bool triggered = false;
    private Transform spikes;
    private Transform gates;
    private const float LowHeight = 0.9f;
    private const float HighHeight = 2f;
    private Vector3 platScale;
    private void Awake() {
        transform.name = Constants.SPIKE_NAME;
        spikes = transform.Find("Spikes");
        gates = transform.Find("Gates");
        platScale = PlatformPool.inst.platform.transform.localScale;

        // set the scale and position according to the platform 
        adjustDim(gates, platScale.x / 3, gates.localScale.y);
        gates.localPosition = new Vector3(gates.localPosition.x,
            platScale.y / 2 + Offset, gates.localPosition.z);
        adjustDim(spikes, platScale.x / 3, Movement.jumpHeight * LowHeight);
        resetObs();

    }
    
    private void OnEnable() {
        // transition to phase 2
        if (Time.timeSinceLevelLoad > Constants.PHASE_LIMIT[0] && phase == Constants.PHASE[0]) {
            spikes.localScale = new Vector3(spikes.localScale.x,
            Movement.jumpHeight * HighHeight, spikes.localScale.z);
            speed *= SpeedInc;
            gates.localPosition = new Vector3(0, gates.localPosition.y,
                gates.localPosition.z);
            wait /= waitReduct;
            // position the gates in all lanes
            for (int i = 0; i < 3; i++) {
                Transform gate = gates.GetChild(i);
                gate.localPosition = new Vector3((i - 1),
                    gate.localPosition.y, gate.localPosition.z);
            }
            phase = Constants.PHASE[1];
          // transition to phase 3
        } else if (Time.timeSinceLevelLoad > Constants.PHASE_LIMIT[1] && phase == Constants.PHASE[1]) {
            phase = Constants.PHASE[2];
        }
        resetObs();
        
    }
    
    void Update(){
        if (!waiting) {
            move();
            // start going down if it has reached its maximum height
            if (spikes.localPosition.y >= spikes.localScale.y) {
                goUp = false;
              // reached the bottom of the floor
            } else if (spikes.localPosition.y <= 0) {                        
                if (phase == Constants.PHASE[2] && !triggered) {
                    // chance to instantly go to the top during phase 3
                    instantTrigger();

                } else {
                    sleep();
                }

            }
          // in cool down period
        } else { 
            waitTime++;
            if (waitTime * Time.deltaTime >= wait) {
                waiting = false;
            }
        }
    }
    // performs the movement of the spikes
    private void move() {
        if (!goUp) {
            spikes.localPosition += -speed * Time.deltaTime * Vector3.up;
        } else {
            spikes.localPosition += speed * Time.deltaTime * Vector3.up;
        }
    }

    private void adjustDim(Transform child, float width, float height) {
        child.localScale = new Vector3(width, height, width / 2);
    }
    // moves the gates to the new spike position
    private void resetObs() {
        moveSpike();
        if (phase == Constants.PHASE[0]) {
            gates.localPosition = new Vector3(spikes.localPosition.x,
                gates.localPosition.y, gates.localPosition.z);
        }

    }

    // instantly moves the spikes to their maximum height to 
    // catch the player by surprise
    private void instantTrigger() {
        float chance = Random.Range(0, 1.0f);
        if (chance > InstantTrigger) {
            spikes.localPosition = new Vector3(spikes.localPosition.x,
            spikes.localScale.y, spikes.localPosition.z);
            triggered = true;
        }
    }
    // spikes do not operate during their cool down period
    private void sleep() {
        // enter into cool down
        waiting = true;
        waitTime = 0;
        goUp = true;
        triggered = false;
    }

    private void moveSpike() {
        // choose a lane to spawn
        int lane = Random.Range(-1, 2);
        spikes.localPosition = new Vector3(lane * PlatformPool.inst.platform.transform.localScale.x / Constants.LANES,
            spikes.localPosition.y, spikes.localPosition.z);
    }
}
