using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderScript : MonoBehaviour
{
    private Transform cauldron;
    private Transform fire;
    private Vector3 roadScale;
    private const float Height = 10;
    private void Awake() {
        // adjust the dimensions according to
        // the platform dimensions
        cauldron = transform.Find("Cauldron");
        fire = transform.Find("Fire");
        roadScale = PlatformPool.inst.platform.transform.localScale;
        cauldron.localScale = new Vector3(1, Height, 1);
    }
    private void OnEnable() {
        // choose a side to spawn
        int side = Random.Range(0, 2);
        if (side == 0) {
            side--;
        }
        cauldron.localPosition = new Vector3(side * (roadScale.x / 2 + 2),
            roadScale.y / 2, 0);
        cauldron.eulerAngles = new Vector3(0, -side * 90, 0);
        fire.transform.localPosition = new Vector3(
            cauldron.localPosition.x, cauldron.localScale.y / 2 + 1,
            cauldron.localPosition.z);
        float fireRot = (side == 1) ? 180 : 0;
        // adjust the fire direction accrodingly
        fire.transform.eulerAngles = new Vector3(0, fireRot, 0);
        // plat the fire effect
        fire.GetComponent<ParticleSystem>().Play();

    }
}
