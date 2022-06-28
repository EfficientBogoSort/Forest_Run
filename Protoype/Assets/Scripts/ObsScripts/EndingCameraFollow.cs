using UnityEngine;

public class EndingCameraFollow : MonoBehaviour
{
    public GameObject endingPlayer;

    private float distance = 6f;
    private float elapsedTime = 0;
    private bool followActivate = false;
    private bool ascendActivate = false;
    private bool descendActivate = false;
    private bool spinActivate = false;
    private bool rotateDownActivate = false;
    private bool forwardActivate = false;
    private float spinSpeed = 40;
    private void Update() {

        if (elapsedTime < 7) {
            followActivate = true;
        } else {
            followActivate = false;
        }

        if (elapsedTime > 7 && elapsedTime < 9) {
            ascendActivate = true;
            rotateDownActivate = true;
            forwardActivate = true;
        } else {
            ascendActivate = false;
            rotateDownActivate = false;
            forwardActivate = false;
        }

        // ascend camera
        if (ascendActivate) {
            transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 2, transform.position.z);
        }

        // spin view
        if (spinActivate) {
            transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * spinSpeed, Vector3.right);
        }

        // descend camera
        if (descendActivate) {
            transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * 2, transform.position.z);
        }

        // follow player
        if (followActivate) {
            transform.position = new Vector3(transform.position.x, transform.position.y, endingPlayer.transform.position.z + distance);
        }

        // rotate down
        if (rotateDownActivate) {
            transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * 12, Vector3.right);
        }

        // move forward
        if (forwardActivate) {
            this.transform.localPosition -= Vector3.forward * Time.deltaTime * 3;
            this.transform.localPosition -= Vector3.left * Time.deltaTime * 2.3f;
            transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * 20, Vector3.up);
        }


        elapsedTime += Time.deltaTime;
    }

}