using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DistanceDisplay : MonoBehaviour
{
    public int HOME_DISTANCE = 2000;
    public float startPositionY;
    public GameObject progressBar;
    private Text text;
    void Start()
    {
        progressBar = GameObject.Find("ProgressBar");
        progressBar.GetComponent<Progress>().SetHomeDistance(HOME_DISTANCE);
        startPositionY = Movement.Instance.transform.position.z;
        progressBar.GetComponent<Progress>().SetProgress(0);
    }

    void Update()
    {
        text = GetComponent<Text>();
        // how many distance the player has run 
        float distance = Movement.Instance.transform.position.z - startPositionY;
        progressBar.GetComponent<Progress>().SetProgress(distance);
        text.text = (int)(HOME_DISTANCE -  distance) + "";
        if (HOME_DISTANCE - distance <= 0) {
            SceneManager.LoadScene("StoryModeEnding");
        }
    }
}
