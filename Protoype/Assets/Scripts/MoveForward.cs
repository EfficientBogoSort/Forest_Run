using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveForward : MonoBehaviour {

    public float thresholdTime;
    public float speed;
	private float elapsedTime = 0;

    // Update is called once per frame
    void Update () {
        this.transform.localPosition += Vector3.forward * Time.deltaTime * speed;
		if (elapsedTime > thresholdTime) {
			speed = 0;
            SceneManager.LoadScene("StoryMode");
		}

		elapsedTime += Time.deltaTime; 
    }
}
