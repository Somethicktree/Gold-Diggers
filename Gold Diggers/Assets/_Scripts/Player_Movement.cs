using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

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

    public Transform cam;

    [Header("Visual Effects")]
    public VisualEffect blockBreak;
    public GameObject blockBreakObject;

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

    public void OnMine()
    {
        Debug.Log("Mining");
        Ray playerRay = new Ray(cam.position, cam.forward);

        RaycastHit hit;
        if (Physics.Raycast(playerRay, out hit, interactionLength, breakable))
        {
            Debug.Log("Mine block");

            //Get the ScoreKeep component before destroyed so we know the value of object
            GameManager.Instance.changeScore(hit.collider.GetComponent<ScoreKeep>().ScoreValue);

            Destroy(hit.collider.gameObject);

            //Make a local variable of Gameobject and instantiate it at the location of the cube
            GameObject VFX = Instantiate(blockBreakObject, hit.point, Quaternion.identity);

            blockBreak = VFX.GetComponent<VisualEffect>();

            //Gets the tag and checks block to send right VFX
            if (hit.collider.CompareTag("Block"))
            {
                blockBreak.SendEvent("Block Break");
            }
            if (hit.collider.CompareTag("Iron"))
            {
                blockBreak.SendEvent("Iron Break");
            }
            if (hit.collider.CompareTag("Gold"))
            {
                blockBreak.SendEvent("Gold Break");
            }
            if (hit.collider.CompareTag("Adam"))
            {
                blockBreak.SendEvent("Adam Break");
            }

            //You can use a timer in a regular destroy function
            Destroy(VFX, 1.5f);
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
