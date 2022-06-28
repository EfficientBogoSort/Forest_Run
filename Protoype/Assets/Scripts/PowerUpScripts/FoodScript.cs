using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : PowerUpScript
{
    public float energyAmount;
    public int baseEnergy;
    private void Awake() {
        SetScale();
    }
    
    private void OnEnable() {
        energyAmount = baseEnergy * (1.5f - Mathf.Abs(zPos));
    }
    
    private void OnTriggerEnter(Collider other) {
        DisablePowerUp(other);
    }
    private void Update() {
        Rotate();
    }
    private void OnBecameInvisible() {
        gameObject.SetActive(false);
    }
}
