using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    private Player_Controls PlayerControls;
    public CameraLook camLook;
    public float moveSpeed = 5f;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    [Header("Dig")]
    public float interactionLength;
    public LayerMask breakable;

    private void Awake()
    {
        PlayerControls = new Player_Controls();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        GetComponent<PlayerInput>().enabled = false;
        GetComponent<PlayerInput>().enabled = true;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void OnMove(InputValue ctx)
    {
        horizontalInput = ctx.Get<Vector2>().x;
        verticalInput = ctx.Get<Vector2>().y;
    }

    public void OnCamMove(InputValue ctx)
    {
        camLook.moveCam(ctx.Get<Vector2>());
    }

    public void OnDig()
    {
        Debug.Log("Digging");
        Ray playerRay = new Ray(transform.position, transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(playerRay, out hit, interactionLength, breakable))
        {
            Debug.Log("Destroy block");

            //Get the ScoreKeep component before destroyed so we know the value of object
            //GameManager.Instance.changeScore(hit.collider.GetComponent<ScoreKeep>().ScoreValue);

            Destroy(hit.collider.gameObject);

            //DestroyWithTag("Block");
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10, ForceMode.Force);
    }

    private void OnEnable()
    {
        PlayerControls.Enable();
    }

    private void OnDisable()
    {
        PlayerControls.Disable();
    }
}
