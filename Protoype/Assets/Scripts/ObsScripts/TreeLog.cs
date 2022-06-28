using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLog : MonoBehaviour
{
    private Transform treeLog;
    private Transform stump;
    //private readonly float high = 5.5f;
    private float height;
    private const float Radius = 3;
    private Vector3 smallScale;
    private float TO_HORIZONTAL = 90;


    private void Awake() {
        name = Constants.TREE_LOG_NAME;
        treeLog = transform.Find("Dead Tree");
        stump = transform.Find("Stump");
        // change the length of the tree according
        // to the platform
        treeLog.localScale = new Vector3(Radius,
            PlatformPool.inst.platform.transform.localScale.x / 2,
            Radius);
        reposition();
        

    }
    private void OnEnable() {
        reposition();
    }

    private void reposition() {
        int mode = Random.Range(0, 2);
        if (mode == 0) {
            mode--;
        }
        treeLog.localPosition = new Vector3(
            mode * PlatformPool.inst.platform.transform.localScale.x / 2,
            Radius / 2, treeLog.localPosition.z);
        treeLog.eulerAngles = new Vector3(treeLog.localRotation.x,
            Random.Range(-15, 15), mode * TO_HORIZONTAL);

        stump.localScale = new Vector3(2 * Radius,
            stump.localScale.y, 2 * Radius);

        stump.localPosition = new Vector3(
             mode * (PlatformPool.inst.platform.transform.localScale.x / 2 + 3),
             stump.localScale.y / 2, treeLog.localPosition.z);


    }
}
