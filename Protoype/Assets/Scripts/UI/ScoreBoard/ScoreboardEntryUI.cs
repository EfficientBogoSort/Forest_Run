using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreboardEntryUI : MonoBehaviour
{
    [SerializeField] private Text entryRankText = null;
    [SerializeField] private Text entryNameText = null;
    [SerializeField] private Text entryScoreText = null;

    public void Initialise(ScoreboardEntryData scoreboardEntryData) {
        entryRankText.text = (scoreboardEntryData.rank + 1).ToString();
        entryNameText.text = scoreboardEntryData.name;
        entryScoreText.text = scoreboardEntryData.score.ToString();
    }
}