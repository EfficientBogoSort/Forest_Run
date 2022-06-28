using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;

public class Scoreboard : MonoBehaviour
{
    public Transform holderTransform = null;
    public GameObject scoreboardEntryObject = null;

    private string savePath => $"{Application.persistentDataPath}/highsocres.json";

    private void Start() {
        ScoreBoardSaveData savedScores = GetSaveScores();
        UpdateUI(savedScores);
    }

    private void UpdateUI(ScoreBoardSaveData savedScores) {
        foreach (Transform child in holderTransform) {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < savedScores.highscores.Count; i++) {
           ScoreboardEntryData entry = savedScores.highscores[i];
            entry.rank = i;
            Instantiate(scoreboardEntryObject, holderTransform).
            GetComponent<ScoreboardEntryUI>().Initialise(entry);
        }
    }

    public ScoreBoardSaveData GetSaveScores() {
        if (!File.Exists(savePath)) {
            File.Create(savePath).Dispose();
            return new ScoreBoardSaveData();
        }

        using(StreamReader stream = new StreamReader(savePath)) {
            string json = stream.ReadToEnd();
            ScoreBoardSaveData saveSocres = JsonUtility.FromJson<ScoreBoardSaveData>(json);
            if (saveSocres == null) {
                return new ScoreBoardSaveData();
            } else {
                return saveSocres;
            }
        }
    }
}
