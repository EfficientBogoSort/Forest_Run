using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static Movement player;
    private CharacterController controller;
    private const float SideSpeed = 40f;
    private const float BaseSpeed = 30;
    public float forwardSpeed;
    private const float ForwSpeedInc = 5;  
    public static float jumpHeight = 6f;
    private const float Gravity = -70f;
    private int currLane;
    private Vector3 targetLane;
    private float laneWidth;
    private int _phase;
    public bool isCrouching;
    private const float CroucHeight = 1;
    private float StandHeight;
    private const float CrouchDur = .5f;
    private Transform platform;


    private Vector3 move = new Vector3(0, 0, 1);
    private Vector3 velocity;
    public bool isGrounded;
    public LayerMask mask;
    private Transform groundCheck;
    private const float GroundDist = 0.2f;

    public AudioSource jumpSound;


    private void Awake() {
        // reset attributes
        if (player != null && player != this) {
            Destroy(this.gameObject);
        } else {
            player = this;
        }
        isCrouching = false;
        _phase = Constants.PHASE[0];
        forwardSpeed = BaseSpeed;
        _phase = Constants.PHASE[0];
        currLane = 1;
        
    }
    private void Start() {
        // initialize all attributes
        controller = gameObject.GetComponent<CharacterController>();
        StandHeight = controller.height;
        platform = PlatformPool.inst.platform.transform;
        laneWidth = platform.localScale.x / Constants.LANES;
        groundCheck = transform.GetChild(0);

    }

    // Update is called once per frame
    void Update()
    {
        checkTransition();
        isGrounded = Physics.CheckSphere(groundCheck.position, GroundDist, mask);
        handleMove();
        handleJump();

        handleCrouch();
    }

    public static Movement Instance { get { return player; } }

    private void switchLane() {
        // SLide to the left
        if (Input.GetKeyDown(KeyCode.A)) {
            currLane--;
        }
        // Slide to the right
        else if (Input.GetKeyDown(KeyCode.D)) {
            currLane++;
        }
        currLane = Mathf.Clamp(currLane, 0, 2);
        // Criss cross
    }

    private IEnumerator crouch() {
        isCrouching = true;
        controller.height = CroucHeight;
        controller.center = new Vector3(0, CroucHeight / 2, 0);

        yield return new WaitForSeconds(CrouchDur);
        controller.height = StandHeight;
        controller.center = new Vector3(0, StandHeight / 2, 0);
        isCrouching = false;
    }

    // handles player sliding under objects
    private void handleCrouch() {
        if (!isCrouching && isGrounded && Input.GetKeyDown(KeyCode.S)) {
            StartCoroutine(crouch());
        }
    }

    private void handleMove() {
        // handles side movements
        switchLane();
        targetLane = transform.position.z * Vector3.forward;
        if (currLane == 0) {
            targetLane += Vector3.left * laneWidth;
        } else if (currLane == 2) {
            targetLane += Vector3.right * laneWidth;
        }        

        move = Vector3.zero;
        move.x = (targetLane - transform.position).normalized.x * SideSpeed;
        move.z = forwardSpeed;

        controller.Move(move * Time.deltaTime);
    }

    private void handleJump() {

        if (isGrounded && velocity.y < 0) {
            velocity.y = -1;
        }
        // only jump if the player is on the ground and not crouching
        if (isGrounded && !isCrouching && Input.GetButtonDown("Jump")) {
            jumpSound.Play();
            velocity.y = Mathf.Sqrt(Gravity * -2 * jumpHeight);
        }
        velocity.y += Gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    // applies the changes for each phase
    private void checkTransition() {
        if (_phase == Constants.PHASE[0] && Time.timeSinceLevelLoad > Constants.PHASE_LIMIT[0]) {
            forwardSpeed += ForwSpeedInc;
            _phase = Constants.PHASE[1];

        } else if (_phase == Constants.PHASE[1] && Time.timeSinceLevelLoad > Constants.PHASE_LIMIT[1]) {
            forwardSpeed += ForwSpeedInc;
            _phase = Constants.PHASE[2];
        }
    }
}
