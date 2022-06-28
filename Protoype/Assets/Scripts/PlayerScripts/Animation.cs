using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    private Animator anim;
    private Transform platform;
    private Movement movement;
    private int phase;
    private int numFrames = 0;
    private const int MaxNumFrames = 30;
    void Start()
    {
        anim = GetComponent<Animator>();
        platform = PlatformPool.inst.platform.transform;
        movement = GetComponent<Movement>();
        phase = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // increase animation speed
        if (phase == 1 && ObstaclePool.phase == 2) {
            phase = 2;
            anim.SetFloat("speedSpeed", 1.5f);
        }
        anim.SetBool("isCrouching", movement.isCrouching);

        if (Input.GetKeyDown(KeyCode.Space)) {
            anim.SetBool("isJumping", true);
        }
        
        // only check if player is on the ground after a certain 
        // number of frames
        else if (numFrames > MaxNumFrames && movement.isGrounded) {
            anim.SetBool("isJumping", false);
            numFrames = 0;
        }
        if (anim.GetBool("isJumping")) {
            numFrames++;
        }
    }
}
