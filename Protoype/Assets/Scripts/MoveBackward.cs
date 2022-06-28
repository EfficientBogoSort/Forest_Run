using UnityEngine;
using System.Collections;

public class MoveBackward : MonoBehaviour {

    public float thresholdTime;
    public float speed;
	private float elapsedTime = 0;
    public GameObject WinScreen;
    public AudioSource winSound;
    private bool ended = false;

    // Update is called once per frame
    void Update () {
        this.transform.localPosition -= Vector3.forward * Time.deltaTime * speed;
		if (elapsedTime > thresholdTime && !ended) {
			speed = 0;
            WinScreen.SetActive(true);
            winSound.Play();
            ended = true;
		}
		elapsedTime += Time.deltaTime; 
    }
}
