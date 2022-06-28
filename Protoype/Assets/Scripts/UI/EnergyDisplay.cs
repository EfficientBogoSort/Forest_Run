using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyDisplay : MonoBehaviour
{
    private CollisionCheck player;
    private Text text;
    private const int LowEnergyLimit = 50;
    private bool lowEnergy = false;
    private void Start() {
        player = GameObject.Find(Constants.PLAYER_NAME).GetComponent<CollisionCheck>();
    }
    void Update()
    {

        text = GetComponent<Text>();
        text.text =  player.energy.ToString("0");
        if (!lowEnergy && player.energy < LowEnergyLimit) {
            text.color = Color.red;
            lowEnergy = true;
        } else if (lowEnergy && player.energy > LowEnergyLimit) {
            text.color = Color.white;
            lowEnergy = false;
        }
    }
}
