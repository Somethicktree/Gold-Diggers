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
    public bool readyToLeave = false;
    private bool canMove = true;
    public GameObject dedUI;
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
    public GameObject hellFire;

    private void Awake()
    {
        PlayerControls = new Player_Controls();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        GameManager.Instance.players.Add(this);

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void OnMove(InputValue ctx)
    {
        if (canMove)
        {
            horizontalInput = ctx.Get<Vector2>().x;
            verticalInput = ctx.Get<Vector2>().y;
        }
    }

    public void OnCamMove(InputValue ctx)
    {
        if (canMove) 
        { 
            camLook.moveCam(ctx.Get<Vector2>());
        }
    }

    public void OnLeaveMine()
    {
        readyToLeave = true;
        GameManager.Instance.Leave();
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

            if (hit.collider.CompareTag("Death"))
            {
                Instantiate(hellFire,hit.collider.gameObject.transform.position, Quaternion.identity);
                dead();
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

    public void dead()
    {
        canMove = false;
        dedUI.SetActive(true);
        GameManager.Instance.players.Remove(this);
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
