using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resize : MonoBehaviour
{
    private Transform parent;
    // Start is called before the first frame update
    private void Awake() {
        // resize terrain if it has a parent
        if (transform.parent != null) {
            parent = transform.parent.transform;
            transform.localScale = new Vector3(transform.localScale.x / parent.localScale.x,
                transform.localScale.y / parent.localScale.y,
                transform.localScale.z / parent.localScale.z);

        }
    }
}
