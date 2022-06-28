using UnityEngine;

public class OpeningCameraFollow : MonoBehaviour
{
    public GameObject openingPlayer;

    private float distance = -6f;
    private float elapsedTime = 0;
    private bool followActivate = false;
    private bool ascendActivate = false;
    private bool descendActivate = false;
    private bool spinActivate = false;
    private bool rotateActivate = false;
    private float spinSpeed = 40;
    private void Update() {
        if (elapsedTime > 3 && elapsedTime < 5) {
            ascendActivate = true;
            spinActivate = true;
        }
        if (elapsedTime > 5 && elapsedTime < 7) {
            ascendActivate = false;
            descendActivate = true;
        }
        if (elapsedTime > 7) {
            spinActivate = false;
            rotateActivate = true;
        }
        if (elapsedTime > 9.5) {
            rotateActivate = false;
        }
        if (elapsedTime > 7 && elapsedTime < 8) {
            descendActivate = false;
            followActivate = true;
        }

        // ascend camera
        if (ascendActivate) {
            transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 2, transform.position.z);
        }

        // spin view
        if (spinActivate) {
            transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * spinSpeed, Vector3.right);
        }

        // rotate to up right
        if (rotateActivate) {
            transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * 72, Vector3.back);
        }


        // descend camera
        if (descendActivate) {
            transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * 2, transform.position.z);
        }

        // follow player
        if (followActivate) {
            transform.position = new Vector3(transform.position.x, transform.position.y, openingPlayer.transform.position.z + distance);
        }

        elapsedTime += Time.deltaTime;
    }

}