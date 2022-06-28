using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestFollow : MonoBehaviour
{
    public GameObject player;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
    }
}
