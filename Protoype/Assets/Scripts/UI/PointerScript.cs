using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerScript : MonoBehaviour
{
    public float rotationSpeed = 250;
    private GameObject[] buttons;
    private GameObject canvas;
    void Start() {
        buttons = GameObject.FindGameObjectsWithTag("Button");
        canvas = GameObject.Find("Canvas");
    }
    // Update is called once per frame
    void Update()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        transform.Rotate(0, rotationSpeed * Time.deltaTime,0);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);
        if (results.Count != 0 && results[0].gameObject.tag == "Button")
        {
            transform.position = new Vector3 (results[0].gameObject.transform.position.x - 2.5f, results[0].gameObject.transform.position.y, 0);
            GetComponent<Renderer>().enabled = true;
        } else {
            GetComponent<Renderer>().enabled = false;
        }
    }
}
