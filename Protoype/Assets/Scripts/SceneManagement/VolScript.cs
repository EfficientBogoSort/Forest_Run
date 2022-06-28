using UnityEngine;
using UnityEngine.UI;
public class VolScript : MonoBehaviour
{
    private Slider slider;
    private const float DefaultVol = .8f;
    private void OnEnable() {
        if (slider == null) {
            slider = gameObject.GetComponent<Slider>();
        }
        slider.value = PlayerPrefs.GetFloat("GameVol", DefaultVol);
    }

    public void ChangeVol() {
        AudioListener.volume = slider.value;
    }

}
