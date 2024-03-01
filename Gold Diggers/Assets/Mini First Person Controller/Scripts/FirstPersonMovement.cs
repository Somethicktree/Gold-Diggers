using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    [Header("Interaction")]
    public Camera mainCamera;

    public float interactionLength = 5f;

    public LayerMask breakable;

    public ScoreKeep score;

    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.
        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);

        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Input gotten");
            Dig();
        }
    }

    private void Dig()
    {
        Debug.Log("Digging");
        Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(playerRay, out hit, interactionLength, breakable))
        {
            Debug.Log("Destroy block");

            //Get the ScoreKeep component before destroyed so we know the value of object
            GameManager.Instance.changeScore(hit.collider.GetComponent<ScoreKeep>().ScoreValue);

            Destroy(hit.collider.gameObject);

            //DestroyWithTag("Block");
        }
    }

    
}