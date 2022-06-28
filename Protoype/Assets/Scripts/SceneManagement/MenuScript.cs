using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuScript : MonoBehaviour
{
    private static bool paused = false;
    public GameObject gameUI;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public Text initialName;
    public Text inputtedName;
    public Clock clock;
    private bool lost = false;
    private const float NormalVol = 0.2f;
    private const float LoweredVol = 0.05f;

    public AudioSource loseSound;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (paused) {
                Resume();
            } else if (!lost){
                Pause();
            }
        }
    }

    public void Resume() {
        SetVol(NormalVol);
        paused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        gameUI.SetActive(true);

    }
    public void Pause() {
        SetVol(LoweredVol);
        paused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        gameUI.SetActive(false);
    }

    public void LoadMainMenu() {
        GameObject music = GameObject.Find("InGameMusic");
        if (music != null) {
            Destroy(music);
        }
        SceneManager.LoadScene("Start");
        clock.firstFrameGame = true;
        Time.timeScale = 1;
    }

    public void LoadScoreboard() {
        PlayerPrefs.SetInt("openScoreboard", 1);
        GameObject music = GameObject.Find("InGameMusic");
        if (music != null) {
            Destroy(music);
        }
        SceneManager.LoadScene("Start");
        clock.firstFrameGame = true;
        Time.timeScale = 1;
    }

    public void RestartGame() {
        GameObject music = GameObject.Find("InGameMusic");
        if (music != null) {
            music.GetComponent<AudioSource>().Stop();
            music.GetComponent<AudioSource>().Play();
        }
        Scene scene = SceneManager.GetActiveScene();
        clock.firstFrameGame = true;
        SceneManager.LoadScene(scene.name);
        Time.timeScale = 1;
    }

    public void openLoseScreen() {
        // Not sure how initialName is supposed to work here.
        /*
        if (PlayerPrefs.HasKey("PlayerName")) {
            initialName.text = PlayerPrefs.GetString("PlayerName");
        }
        */
        GameObject music = GameObject.Find("InGameMusic");
        if (music != null) {
            music.GetComponent<AudioSource>().Stop();
        }
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
        lost = true;
        gameUI.SetActive(false);
        loseSound.Play();
    }

    private void SetVol(float vol) {
        AudioSource bgMusic = GameObject.Find("InGameMusic")?.GetComponent<AudioSource>();
        if (bgMusic != null) {
            bgMusic.volume = vol;
        }
        
    }
    public void saveScore() {
        ScoreboardEntryData entry = new ScoreboardEntryData();

        if (inputtedName.text != "") {
            PlayerPrefs.SetString("PlayerName", inputtedName.text);
        }
        entry.name = PlayerPrefs.GetString("PlayerName");
        entry.score = (int)Movement.Instance.transform.position.z;
        GameObject.Find("DataSaver").GetComponent<SaveData>().AddEntry(entry);
    }
}
