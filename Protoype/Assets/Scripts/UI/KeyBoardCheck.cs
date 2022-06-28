using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoardCheck : MonoBehaviour
{
    public Button[] backButton;
    public Button saveButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            foreach (Button button in backButton) {
                if (button.gameObject.activeInHierarchy) {
                    button.onClick.Invoke();
                    return;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && saveButton.gameObject.activeInHierarchy)
        {
            saveButton.onClick.Invoke();
        }
    }
}
