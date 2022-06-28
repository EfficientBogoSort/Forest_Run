using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFollow : MonoBehaviour
{
    private Transform player;
    private const float Distance = 12f;
    private Vector3 rotation = new Vector3(20, 0, 0);
    private FogEffect fog;

    private const float FogStartInit = 200;
    private const float FogStartFinal = 30;

    private const float DensityInit = 0f;
    private const float DensityFinal = 0.03f;

    private const float SkyDensInitUp = 0;
    private const float SkyDensInitDown = .1f;
    private const float SkyDensFinal = .4f;
    private void Start() {
        fog = gameObject.GetComponent<FogEffect>();
        player = GameObject.Find(Constants.PLAYER_NAME).transform;
        transform.eulerAngles = rotation;
        fog._startDistance = FogStartInit;
        fog._density = DensityInit;
        fog._fogDensityWithSkyUp = SkyDensInitUp;
        fog._fogDensityWithSkyDown = SkyDensInitDown;
        fog._needForward = true;



    }
    void Update()
    {
        //Debug.Log(Time.deltaTime);
        if (fog._startDistance > FogStartFinal) {
            fog._startDistance += Delta(FogStartInit, FogStartFinal);
            fog._needForward = true;
        } else {
            fog._needForward = false;
        }
        if (fog._density < DensityFinal) {
            fog._density += Delta(DensityInit, DensityFinal);
        }
        if (fog._fogDensityWithSkyUp < SkyDensFinal) {
            fog._fogDensityWithSkyUp += Delta(SkyDensInitUp, SkyDensFinal);
            fog._fogDensityWithSkyDown += Delta(SkyDensInitDown, SkyDensFinal + SkyDensInitDown);
        }
        
        // change the mixing factor
        if (fog._mixingFactor < 1) {
            fog._mixingFactor += Delta(0, 1);
        }
        // follow the player
        transform.position = new Vector3(transform.position.x, transform.position.y,
            player.position.z - Distance);
    }

    private float Delta(float start, float end) {
        return (end - start) * Time.deltaTime / Constants.PHASE_LIMIT[1];
    }
}
