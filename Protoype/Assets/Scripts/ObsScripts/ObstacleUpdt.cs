using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleUpdt : MonoBehaviour
{
    private GameObject player;
    private float despawnDist;
    private void Start() {
        player = GameObject.Find(Constants.PLAYER_NAME);
        despawnDist = PlatformPool.inst.platform.transform.localScale.z;
    }
    // disables an object after the player is a cerrtain distance from this object
    void Update() {
        if (transform.InverseTransformPoint(player.transform.position).z > despawnDist) {
            gameObject.SetActive(false);
        }
    }
}
