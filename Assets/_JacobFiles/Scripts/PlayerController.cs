using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerAttributes))]
[RequireComponent(typeof(InteractionManager))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]

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

    [Header("Crouching")]
    public float crouchSpeed = 2f; 
    private bool isCrouching = false;
    public AudioClip[] crouchStepSounds;
    public float crouchHeightOffset = 0.5f;


    private CapsuleCollider capsuleCollider;
    private float originalCapsuleHeight;
    private Vector3 originalCameraLocalPosition;

    [Header("Movement")]
    public float moveSpeed = 4f;  
    private Vector2 currentMovementInput;

    [Header("Camera Look")]
    public Transform cameraContainer; 
    public float minXLook, maxXLook;
    private float camCurrentXRotation; 
    public float lookSensitivity; 
    private Vector2 mouseDelta;
    private Rigidbody playerRig;

    [Header("Bobbing Movement")] 
    public float bobbingSpeed = 1.5f; 
    public float bobbingAmount = 0.05f;
    private Vector3 cameraInitialPosition;

    private float timer;


    [HideInInspector]
    public bool canLook = true;

    private void Awake()
    {
        playerRig = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cameraInitialPosition = cameraContainer.localPosition;

        capsuleCollider = GetComponent<CapsuleCollider>();
        originalCapsuleHeight = capsuleCollider.height;
        originalCameraLocalPosition = cameraContainer.localPosition;
    }

    private void LateUpdate()
    {
        if (canLook == true)
        {
            CameraLook();
        }

        HandleBobbingMovement();
    }

    private void FixedUpdate()
    {
        HandleMove();
        HandleCrouch();
    }

    private void HandleMove()
    {
        Vector3 moveDirection = transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x;

        float speed = moveSpeed;
       
        if (isCrouching)
        {
            speed = crouchSpeed;
        }

        moveDirection *= speed;
        moveDirection.y = playerRig.velocity.y;
        playerRig.velocity = moveDirection;
        if (moveDirection.magnitude > footStepThreshHold && IsGrounded())
        {
            if (Time.time - lastStepTime > footStepRate)
            {
                lastStepTime = Time.time;
                if (isCrouching)
                {
                    Debug.Log("Crouch Footstep");
                    audioSource.PlayOneShot(crouchStepSounds[Random.Range(0, crouchStepSounds.Length)]);
                }
                else
                {
                    Debug.Log("Normal Footstep");
                    audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
                }
            }
        }
    }
    private void HandleCrouch()
    {
        if (isCrouching)
        {
            capsuleCollider.height = originalCapsuleHeight * 0.5f; 
            cameraContainer.localPosition = originalCameraLocalPosition + Vector3.down * 0.5f; 
        }
        else
        {
            capsuleCollider.height = originalCapsuleHeight;
            cameraContainer.localPosition = originalCameraLocalPosition; 
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
    private void HandleBobbingMovement()
    {
        float horizontalInput = currentMovementInput.x;
        float verticalInput = currentMovementInput.y;

        float bobbingOffset = Mathf.Sin(timer) * bobbingAmount;

        Vector3 cameraLocalPosition = cameraContainer.localPosition;
        cameraLocalPosition.y = cameraInitialPosition.y + bobbingOffset;
        cameraContainer.localPosition = cameraLocalPosition;

        float speedFactor = Mathf.Clamp01(new Vector2(horizontalInput, verticalInput).magnitude);
        timer += speedFactor * bobbingSpeed * Time.deltaTime;
    }

    public void HandleJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (IsGrounded())
            {
                playerRig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    public void HandleCrouchInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isCrouching = !isCrouching;
        }
    }


    bool IsGrounded()
    {
        Ray[] rays = new Ray[4];

        Vector3 raycastOrigin = transform.position + (Vector3.up * 0.02f);
        if (isCrouching)
        {
            raycastOrigin += Vector3.up * crouchHeightOffset; // Adjust this value as needed
        }

        rays[0] = new Ray(raycastOrigin + (transform.forward * 0.2f), Vector3.down);
        rays[1] = new Ray(raycastOrigin - (transform.forward * 0.2f), Vector3.down);
        rays[2] = new Ray(raycastOrigin + (transform.right * 0.2f), Vector3.down);
        rays[3] = new Ray(raycastOrigin - (transform.right * 0.2f), Vector3.down);

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
