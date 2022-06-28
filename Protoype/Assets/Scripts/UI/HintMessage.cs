using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintMessage : MonoBehaviour
{
    public GameObject[] UserInterfaces;
    public GameObject[] UserInterfaceTexts;
    public Color highlightColor;
    public Color DarkerColor;
    public float twinkleSpeed;
    public float startFadeTime;
    [Range(0,0.5f)]public float fadeSpeed;
    public float remainingRenderTime;

    private float elapsedTime = 0;

    // Update is called once per frame
    void Update()
    {
        if ((PlayerPrefs.HasKey("firstGame") && PlayerPrefs.GetInt("firstGame") == 1) || !PlayerPrefs.HasKey("firstGame")) {
            Color UIColor = Color.Lerp(DarkerColor, highlightColor, (1 + Mathf.Cos(elapsedTime / twinkleSpeed * 2 * Mathf.PI)) / 2);
            foreach (GameObject UI in UserInterfaces) {
                    UI.GetComponent<Image>().color = UIColor;
            }
            foreach (GameObject UI in UserInterfaceTexts) {
                Text text = UI.GetComponent<Text>();
                text.color = UIColor;
            }
            if (remainingRenderTime > 0) {
                if (elapsedTime > twinkleSpeed) {
                    elapsedTime -= twinkleSpeed;
                    remainingRenderTime -= twinkleSpeed;
                    if (remainingRenderTime < startFadeTime) {
                        highlightColor.a -= fadeSpeed;
                        DarkerColor.a -= fadeSpeed;
                    }
                }
            } else {
                foreach (GameObject UI in UserInterfaces) {
                    UI.SetActive(false);
                }
                foreach (GameObject UI in UserInterfaceTexts) {
                    UI.SetActive(false);
                }
                PlayerPrefs.SetInt("firstGame", 0);
                PlayerPrefs.Save();
            }
            elapsedTime += Time.deltaTime;
        } else {
            foreach (GameObject UI in UserInterfaces) {
                UI.GetComponent<Image>().enabled = false;
            }
            foreach (GameObject UI in UserInterfaceTexts) {
                UI.GetComponent<Text>().enabled = false;
            }
            GetComponent<HintMessage>().enabled = false;
        }
    }
}
