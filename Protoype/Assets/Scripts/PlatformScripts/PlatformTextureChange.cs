using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTextureChange : MonoBehaviour
{
    public Material phase0Material;
    public Material phase1Material;
    public Material phase2Material;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad < Constants.PHASE_LIMIT[0]) {
            GetComponent<MeshRenderer>().material = phase0Material;
        } else if (Time.timeSinceLevelLoad < Constants.PHASE_LIMIT[1]) {
            GetComponent<MeshRenderer>().material = phase1Material;
        } else if (Time.timeSinceLevelLoad < Constants.PHASE_LIMIT[2]) {
            GetComponent<MeshRenderer>().material = phase2Material;
        }
    }
}
