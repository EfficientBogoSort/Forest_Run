using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    public void SetHomeDistance(int distance) {
        slider.maxValue = distance;
        slider.value = 0;
    }
    public void SetProgress(float distance) {
        slider.value = distance;
    }
}
