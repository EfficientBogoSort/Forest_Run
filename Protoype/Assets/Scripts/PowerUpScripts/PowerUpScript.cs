using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    private const float RotationSpeed = 60;
    private const float RelativeScale = 1.0f / 12;
    protected float zPos;
    public float spawnChance;
    protected void SetScale() {
        // set the dimensions of the power up according to the 
        // platform dimensions
        Vector3 platScale = PlatformPool.inst.platform.transform.localScale;
        transform.localScale = new Vector3(
            RelativeScale * platScale.x, 1.5f,
            RelativeScale * platScale.x);
    }

    protected void DisablePowerUp(Collider other) {
        if (other.CompareTag("Player")) {
            // disable the object when the player
            // comes in contact with it
            gameObject.SetActive(false);
        }
    }

    protected void Rotate() {
        transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
    }

    public void SetPos(Vector3 platform, List<Vector2> points) {
        // Generate a position and check that it's not within a certain distance
        // to another power up
        float x = 0;
        float z = 0;
        Vector2 chosenPos = new Vector2();
        while (!inPowerUpArea(points, chosenPos)) {
            x = Random.Range(-1, 2);
            z = Random.Range(-0.8f, 0.8f);
            chosenPos = new Vector2(x, z);
        }

        transform.position = platform + new Vector3(
                    PlatformPool.inst.platform.transform.localScale.x / 3 * x,
                    transform.localScale.y + 1,
                    PlatformPool.inst.platform.transform.localScale.z * z / 2);
        points.Add(chosenPos);
        zPos = z;
    }
    private bool inPowerUpArea(List<Vector2> occupied, Vector2 pos) {
        for (int i = 0; i < occupied.Count; i++) {
            if (Vector2.Distance(pos, occupied[i]) < 0.2667f) {
                return false;
            }
        }
        return !(pos.y * PlatformPool.inst.platform.transform.localScale.z > -6 &&
                    pos.y * PlatformPool.inst.platform.transform.localScale.z < 6);
    }
}
