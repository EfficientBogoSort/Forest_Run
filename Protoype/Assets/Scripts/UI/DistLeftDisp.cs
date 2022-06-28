using UnityEngine;
using UnityEngine.UI;

public class DistLeftDisp : MonoBehaviour
{
    private Text text;
    public GameObject display;
    private float maxDist;
    private void Awake() {
        maxDist = display.GetComponent<DistanceDisplay>().HOME_DISTANCE;
        text = GetComponent<Text>();
    }
    private void OnEnable() {
        // calculate the distance left and displays it
        text.text = (int) (maxDist - Movement.Instance.transform.position.z) + "   metres";
    }

}
