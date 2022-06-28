using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
public class SceneSwitcher : MonoBehaviour
{
    public Slider volSlider;
    public Button backButton;
    public Clock clock;
    public Transform holderTransform = null;
    private string savePath => $"{Application.persistentDataPath}/highsocres.json";

    private void Start() {
        AudioListener.volume = PlayerPrefs.GetFloat("GameVol", 0.8f);
    }
    public void PlayStoryMode() {
        SceneManager.LoadScene("StoryModeOpening");
    }

    public void PlayEndlessMode() {
        SceneManager.LoadScene("EndlessMode");
    }

    public void ExitGame() {
        Application.Quit();
    }
    public void SaveGameVol() {
        PlayerPrefs.SetFloat("GameVol", volSlider.value);
        backButton.onClick.Invoke();
    }

    public void SetVol() {
        AudioListener.volume = PlayerPrefs.GetFloat("GameVol", 0.8f);
    }
    public void LoadMainMenu() {
        GameObject music = GameObject.Find("InGameMusic");
        if (music != null) {
            Destroy(music);
        }
        SceneManager.LoadScene("Start");
        if (clock != null) {
            clock.firstFrameGame = true;
        }
        Time.timeScale = 1;
    }
    public void reset() {
        foreach (Transform child in holderTransform) {
            Destroy(child.gameObject);
        }
        using(StreamWriter stream = new StreamWriter(savePath, false)) {
            stream.Write(string.Empty);
        }
    }
}