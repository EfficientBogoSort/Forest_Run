using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{
    public float secPerDay = 5;
	public float initialTime;
	public bool firstFrameGame = true;
	public float currentTime;

    private float degreesPerSecond;
    private float elapsedTime;

	void Start () {
		initialise();
	}
    void Update () {
		Scene scene = SceneManager.GetActiveScene();
		if (SceneManager.GetActiveScene().name == "StoryModeOpening" && firstFrameGame) {
			firstFrameGame = false;
			initialise();
		} else if (SceneManager.GetActiveScene().name == "StoryMode" && firstFrameGame) {
			firstFrameGame = false;
			initialise();
		}
		elapsedTime += Time.deltaTime;
		currentTime = elapsedTime % secPerDay;
	}

	public void initialise () {
		elapsedTime = initialTime;
		currentTime = elapsedTime % secPerDay;
	}
}
