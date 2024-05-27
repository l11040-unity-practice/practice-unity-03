using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed;
    public float JumpPower;
    private Vector2 _curMovementInput;
    public LayerMask GroundLayerMask;

    [Header("Look")]
    public Transform CameraContainer;
    public float MinXLook;
    public float MaxXLook;
    public float LookSensitivity;
    private float _camCurXRot;
    private Vector2 _mouseDelta;

    private Rigidbody _rigidbody;
    private bool isSetting = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void LateUpdate()
    {
        if (!isSetting)
        {
            CameraLook();
        }
    }
    private void FixedUpdate()
    {
        Move();
    }
    void CameraLook()
    {
        _camCurXRot += _mouseDelta.y * LookSensitivity;
        _camCurXRot = Mathf.Clamp(_camCurXRot, MinXLook, MaxXLook);
        CameraContainer.localEulerAngles = new Vector3(-_camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, _mouseDelta.x * LookSensitivity, 0);
    }

    void Move()
    {
        Vector3 dir = transform.forward * _curMovementInput.y + transform.right * _curMovementInput.x;
        dir *= MoveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * JumpPower, ForceMode.Impulse);
        }
    }
    public void OnSetting(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (isSetting)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Time.timeScale = 0;
            }
            isSetting = !isSetting;
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
                    new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
                    new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
                    new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
                    new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, GroundLayerMask))
            {
                return true;
            }
        }
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3[] rayOrigins = new Vector3[4]
        {
            transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f),
            transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f),
            transform.position + (transform.right * 0.2f) + (transform.up * 0.01f),
            transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f)
        };

        foreach (Vector3 origin in rayOrigins)
        {
            Gizmos.DrawLine(origin, origin + Vector3.down * 0.1f);
        }
    }
}
