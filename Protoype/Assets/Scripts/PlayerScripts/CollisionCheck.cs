using UnityEngine;
using System.Collections;
public class CollisionCheck : MonoBehaviour
{
    public float energy;
    private float energyLoss;
    private int phase;
    private const float deltaEnergy = .5f;
    public GameObject gameOverMenu;
    public bool slowed;

    private void Awake() {
        slowed = false;
        energyLoss = 2f;
        energy = 100;
        phase = Constants.PHASE[0];
    }
    private void Update() {
        // energy loss rate increases in each phase
        if (phase == Constants.PHASE[0] && 
            Time.timeSinceLevelLoad > Constants.PHASE_LIMIT[0]) {
            phase = Constants.PHASE[1];
            energyLoss += deltaEnergy;
        } else if (phase == Constants.PHASE_LIMIT[1] && 
            Time.timeSinceLevelLoad > Constants.PHASE_LIMIT[1]) {
            phase = Constants.PHASE[2];
            energyLoss += deltaEnergy;
        }
        // lose if player runs out of energy
        if (energy <= 0) {
            Lose();
        }
        energy -= energyLoss * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        string tag = other.tag;
        if (tag == "Food") {
            energy += other.gameObject.GetComponent<FoodScript>().energyAmount;
        } else if (!slowed && tag == "SpeedModifier") {
            SpeedModifier slowDown = other.gameObject.GetComponent<SpeedModifier>();
            StartCoroutine(SlowDown(slowDown.Duration, slowDown.SpeedDec));
        }

        
    }
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        // make the player lose the game if hits an obstacle
        if (hit.gameObject.CompareTag("Obstacle")) {
            Lose();
        }
    }

    private void Lose() {
        gameOverMenu.GetComponent<MenuScript>().openLoseScreen();
    }

    private IEnumerator SlowDown(float duration, float slow) {
        slowed = true;
        gameObject.GetComponent<Movement>().forwardSpeed += slow;
        yield return new WaitForSeconds(duration);
        slowed = false;
        gameObject.GetComponent<Movement>().forwardSpeed -= slow;

    }
}
