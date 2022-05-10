using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine;


public class SimpleCameraController : MonoBehaviour
{
    private bool turnable;
    public bool invertY;
    public bool invertX;

    private Vector3 movement;
    private Vector2 look;

    public float lookSpeed;
    public float flySpeed;
    private float flySafe;
    public float dashSpeed;
    private float rotateX;
    private float rotateY;

    private void Start()
    {
        movement = Vector3.zero;
        flySafe = flySpeed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveVector = context.ReadValue<Vector2>();
        movement.x = moveVector.x * 0.1f;
        movement.z = moveVector.y * 0.1f;
    }
    public void OnHover(InputAction.CallbackContext context)
    {
        float hoverAxis = context.ReadValue<float>();
        movement.y = hoverAxis * flySpeed * 0.1f;
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
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
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 LookVector = context.ReadValue<Vector2>();
        look.x = LookVector.x * lookSpeed * 0.1f;
        look.y = LookVector.y * lookSpeed * 0.1f;
        // Debug.Log("Look" + LookVector);
    }

    public void OnRun(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            flySpeed = dashSpeed;
        }

        if (context.canceled)
        {
            flySpeed = flySafe;
        }
    }

    public void OnMoveUpSpecial(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movement.z = 1f;
        }
        //   Debug.Log("TouchUp" + context.phase.ToString());
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        //   Debug.Log("Interact" + context.phase.ToString());
    }


    void Update()
    {
        if (turnable)
        {
            look.y = look.y * (invertY ? -1 : 1);
            look.x = look.x * (invertX ? -1 : 1);

            rotateY -= look.y;
            rotateX -= look.x;

            this.transform.localRotation = Quaternion.Euler(rotateY, rotateX, 0f);
        }

        Vector3 translation = (transform.right * movement.x + transform.forward * movement.z + transform.up * movement.y) * flySpeed;
        transform.position += translation;
    }
}
