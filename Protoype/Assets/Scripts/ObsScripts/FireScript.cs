using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    private ParticleSystem.MainModule fire;
    private void Awake() {
        // adjust the lifetime depending on the width of the road
        fire = transform.Find("Fire").gameObject.GetComponent<ParticleSystem>().main;
        fire.startLifetime = PlatformPool.inst.platform.transform.localScale.x / fire.startSpeed.constant;

    }

}
