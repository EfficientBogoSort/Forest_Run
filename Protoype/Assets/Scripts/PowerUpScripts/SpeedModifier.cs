using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier : PowerUpScript
{
    public float SpeedDec = -20;
    public float Duration = 5;
    private void Awake() {
        SetScale();
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
