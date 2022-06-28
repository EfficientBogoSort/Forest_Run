using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveScoreboard : MonoBehaviour
{
    public GameObject scoreboard;
    public Button ToScoreboard;
    void Start() {
        if (!PlayerPrefs.HasKey("PlayerName")) {
            PlayerPrefs.SetString("PlayerName", "Anonymous");
        }
        if (scoreboard.GetComponent<Scoreboard>().GetSaveScores() == null) {
            PlayerPrefs.SetInt("openScoreboard", 0);
        }

    }
    void Update()
    {
        if (PlayerPrefs.HasKey("openScoreboard") && PlayerPrefs.GetInt("openScoreboard") == 1 && !scoreboard.activeSelf) {
            ToScoreboard.onClick.Invoke();
            PlayerPrefs.SetInt("openScoreboard", 0);
        }
    }
}
