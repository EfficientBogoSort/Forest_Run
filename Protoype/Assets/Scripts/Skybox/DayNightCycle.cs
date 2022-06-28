using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    public Clock clock;
    public Vector3 noon;

    [Header("Skybox")]
    public Material Skybox;
    public Gradient color;
    public AnimationCurve skyExposure;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Environment")]
    public AnimationCurve lightingIntensity;
    public AnimationCurve reflectionIntensity;

    [Header("UI")]
    public GameObject[] UserInterfaces;
    public GameObject[] UserInterfaceTexts;
    public Gradient UserInterfaceColor;
    

    // Update is called once per frame
    void Update()
    {
        float normalizedTime = clock.currentTime / clock.secPerDay;

        // skybox
        RenderSettings.skybox.SetFloat("_Exposure", skyExposure.Evaluate(normalizedTime));
        RenderSettings.skybox.SetColor("_Tint", color.Evaluate(normalizedTime));

        // rotation
        sun.transform.eulerAngles = (normalizedTime - 0.25f) * noon * 4.0f;
        moon.transform.eulerAngles = (normalizedTime - 0.75f) * noon * 4.0f;
        
        // intensity
        sun.intensity = sunIntensity.Evaluate(normalizedTime);
        moon.intensity = moonIntensity.Evaluate(normalizedTime);

        // change colors
        sun.color = sunColor.Evaluate(normalizedTime);
        moon.color = moonColor.Evaluate(normalizedTime);

        if (sun.intensity == 0 && sun.gameObject.activeInHierarchy) {
            sun.gameObject.SetActive(false);
        } else if (sun.intensity > 0 && !sun.gameObject.activeInHierarchy) {
            sun.gameObject.SetActive(true);
        }
        if (moon.intensity == 0 && moon.gameObject.activeInHierarchy) {
            moon.gameObject.SetActive(false);
        } else if (moon.intensity > 0 && !moon.gameObject.activeInHierarchy) {
            moon.gameObject.SetActive(true);
        }

        // other lights intensity
        RenderSettings.ambientIntensity = lightingIntensity.Evaluate(normalizedTime);
        RenderSettings.reflectionIntensity = reflectionIntensity.Evaluate(normalizedTime);

        // UI
        Color UIColor = UserInterfaceColor.Evaluate(normalizedTime);
        foreach (GameObject UI in UserInterfaces) {
                UI.GetComponent<Image>().color = UIColor;
        }
        foreach (GameObject UI in UserInterfaceTexts) {
            Text text = UI.GetComponent<Text>();
            text.color = UIColor;
        }
    }
}
