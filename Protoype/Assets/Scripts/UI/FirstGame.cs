using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("firstGame", 1);
        PlayerPrefs.Save();
    }
}
