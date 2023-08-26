using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Sound")]
    private AudioSource audioSource;
    public AudioClip[] stepSounds;
    public float footStepRate, footStepThreshHold;
    private float lastStepTime;

    [Header("Jumping")]
    public float jumpForce;
    public LayerMask groundLayer;

    [Header("Movement")]
    public float moveSpeed = 4f;  
    private Vector2 currentMovementInput;

    [Header("Camera Look")]
    public Transform cameraContainer; 
    public float minXLook, maxXLook; 
    private float camCurrentXRotation; 
    public float lookSensitivity; 
    private Vector2 mouseDelta;
    private Rigidbody myRig;

    [HideInInspector]
    public bool canLook = true;

    private void Awake()
    {
        myRig = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (canLook == true)
        {
            CameraLook();
        }

    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        Vector3 moveDirection = transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x;
        moveDirection *= moveSpeed;
        moveDirection.y = myRig.velocity.y;
        myRig.velocity = moveDirection;
        if (moveDirection.magnitude > footStepThreshHold && IsGrounded())
        {
            if (Time.time - lastStepTime > footStepRate)
            {
                lastStepTime = Time.time;
                audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
            }
        }
    }

    public void HandleLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void HandleMovementInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            currentMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            currentMovementInput = Vector2.zero;
        }
    }



    private void CameraLook()
    {
        camCurrentXRotation += mouseDelta.y * lookSensitivity;
        camCurrentXRotation = Mathf.Clamp(camCurrentXRotation, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurrentXRotation, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void HandleJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (IsGrounded())
            {
                myRig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward* 0.2f)+(Vector3.up * 0.02f),Vector3.down),
            new Ray(transform.position + (-transform.forward* 0.2f)+(Vector3.up * 0.02f),Vector3.down),
            new Ray(transform.position + (transform.right* 0.2f)+(Vector3.up * 0.02f),Vector3.down),
            new Ray(transform.position + (-transform.right* 0.2f)+(Vector3.up * 0.02f),Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayer))
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.2f), Vector3.down);
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
