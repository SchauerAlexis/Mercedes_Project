using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerActions;
    private CharacterController characterController;

    [Header("Momentary Speed of the Player")]
    [SerializeField] private float movementY;
    [SerializeField] private float movementX;
    [SerializeField] private float movementZ;

    [Header("Momentary Rotation of the Player")]
    [SerializeField] private float lookX;
    [SerializeField] private float lookY;

    private bool turnable;


    [Header("Physics Parameters")]
    public float walkSpeed = 20f;
    public float sprintSpeed = 45f;
    private float speed;
    public float lookSpeed = 1f;
    public float jumpHight = 3f;
    public float gravity = -9.81f;


    [Header("Gravity Check Parameters")]
    
    [Tooltip("Where do we meassure contact with the ground?")]
    public Transform groundCheck;
    
    [Tooltip("How big is our meassuring Range?")]
    public float groundDistance = 0.1f;
    
    [Tooltip("What counts as ground?")]
    public LayerMask groundMask;

    [Tooltip("Are we standing on the ground right now?")]
    [SerializeField] private bool isGrounded;

    private float rotateY = 0f;
    private Transform cam;
    private Transform body;
    public string controlscheme;

    void Start()
    {
        playerActions = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        body = GetComponent<Transform>();
        cam = GetComponentInChildren<Camera>().transform;
        speed = walkSpeed;

    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 LookVector = context.ReadValue<Vector2>();
        lookX = LookVector.x * lookSpeed * 0.1f;
        lookY = LookVector.y * lookSpeed * 0.1f;
    }

    public void OnHover(InputAction.CallbackContext context)
    {
        float hoverAxis = context.ReadValue<float>();
        movementY = hoverAxis * speed * 0.1f;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveVector = context.ReadValue<Vector2>();
        movementX = moveVector.x;
        movementZ = moveVector.y;
    }

    /*
    public void OnMoveUpSpecial(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            characterController.Move(transform.forward * speed * 0.05f);
        }
    }
    */

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            speed = sprintSpeed; 
        }

        if (context.canceled)
        {
            speed = walkSpeed;
        }
    }    
    
    /* public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
            movementY = jumpHight; 

    } */
    
    public void OnInteract(InputAction.CallbackContext context)
    {

    }

    public void OnExit(InputAction.CallbackContext context)
    {

    }

    public void OnModify(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Cursor.lockState = CursorLockMode.Locked;
            turnable = true;
        }

        if (context.canceled)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            turnable = false;
        }

        // Debug.Log("Modify" + context.phase.ToString());
    }

    void Update()
    {
        controlscheme = playerActions.currentControlScheme;

        if ((turnable) || (playerActions.currentControlScheme != "Keyboard&Mouse"))
        {
            rotateY -= lookY;
            rotateY = Mathf.Clamp(rotateY, -80f, 80f);

            cam.transform.localRotation = Quaternion.Euler(rotateY, 0f, 0f);
            body.Rotate(Vector3.up, lookX);
        }

        // movementY += gravity * Time.deltaTime;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && movementY < 0)
            movementY = -0.01f;

        Vector3 movement = (transform.right * movementX + transform.forward * movementZ + transform.up * movementY);
        characterController.Move(movement * speed * 0.1f * Time.deltaTime);
    }
}
