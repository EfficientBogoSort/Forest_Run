using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

public class SaveData : MonoBehaviour
{
    private int maximumEntries = 5;

    private string savePath => $"{Application.persistentDataPath}/highsocres.json";

    public void AddEntry(ScoreboardEntryData scoreboardEntryData) {
        scoreboardEntryData.seed = UnityEngine.Random.value;

        ScoreBoardSaveData savedScores = GetSaveScores();

        bool scoreAdded = false;
        for (int i = 0; i < savedScores.highscores.Count; i++) {
            if (scoreboardEntryData.score > savedScores.highscores[i].score) {
                scoreboardEntryData.rank = i;
                savedScores.highscores.Insert(i, scoreboardEntryData);
                scoreAdded = true;
                break;
            }
        }
        if (!scoreAdded && savedScores.highscores.Count < maximumEntries) {
            scoreboardEntryData.rank = savedScores.highscores.Count;
            savedScores.highscores.Add(scoreboardEntryData);
        }

        if (savedScores.highscores.Count > maximumEntries) {
            savedScores.highscores.RemoveRange(maximumEntries, savedScores.highscores.Count - maximumEntries);
        }

        SavedScores(savedScores);
    }

    private ScoreBoardSaveData GetSaveScores() {
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

    private void SavedScores(ScoreBoardSaveData scoreboardSaveData) {
        using(StreamWriter stream = new StreamWriter(savePath)) {
            string json = JsonUtility.ToJson(scoreboardSaveData, true);
            stream.Write(json);
        }
    }
}