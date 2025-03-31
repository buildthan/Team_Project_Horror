using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpPower;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody _rigidbody;

    [Header("Headbob")]
    public float bobFrequency = 10f;
    public float bobAmplitude = 0.05f;
    private float bobTimer;
    private Vector3 initialCamLocalPos;

    [Header("Sprint")]
    public float sprintMultiplier = 1.5f;
    private bool isSprinting;

    /// <summary>
    /// 임시로 추가
    /// </summary>
    [Header("UI")]
    // UI 관련
    public Action inventory;    // 인벤토리 열고 닫을때 Toggle 메서드를 담아서 실행


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        /// fps 하니까 가운데에 고정하고 보여준다
        Cursor.lockState = CursorLockMode.Locked;
        initialCamLocalPos = cameraContainer.localPosition;

        //인벤토리 접근용
        inventory += UIManager.Instance.gameUI.Toggle;
        UIManager.Instance.gameUI.dropPosition = CharacterManager.Instance.Player.dropPosition;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
            HeadBob();
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnSprintInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            PlayerCondition condition = GetComponent<PlayerCondition>();
            if (condition != null && condition.IsSprintingAllowed)
            {
                isSprinting = true;
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isSprinting = false;
        }
    }

    #region 인벤토리 연결(임시)
    // tab키 누르면 열린다
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            /// UIInventory의 Toggle 메서드를 사용하기 위해 delegate를 사용
            inventory?.Invoke();    // delegate에 Toggle 메서드가 있으면 호출
            //ToggleCursor();
        }
    }
    /// <summary>
    /// 내부적으로 Cursor를 Toggle해주는 기능
    /// 인벤토리를 껐을 때는 커서가 화면 중앙에 고정되며, 보이지 않는다
    /// 인벤토리를 켰을때는 화면을 고정하고, 인벤토리를 클릭해줄 커서가 나와서 화면 전체를 움직일 수 있다
    /// </summary>
    //void ToggleCursor()
    //{
    //    bool toggle = Cursor.lockState == CursorLockMode.Locked;    /// Locked: 인벤토리창이 아직 열리지 않은 상태(커서가 화면 중앙에 고정되어있다)
    //    Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
    //    // toggle이 true: 인벤토리창이 열리지 않아서 커서가 화면 중앙에 고정 -> None으로 만들어서, 화면에서 움직일 수 있게 만든다
    //    // toggle이 false: 인벤토리창이 열려있다면 커서가 화면에서 움직일 수 있다 -> Locked로 만들어서 화면 중앙에 커서를 고정한다

        
    //    canLook = !toggle;
    //    // toggle이 true: 위에서 커서를 화면에서 움직일 수 있게 만들었으므로 canLock은 false
    //    // toggle이 false: 위에서 화면 중앙에 커서를 고정했으므로 canLock은 true
    //}
    #endregion





    public bool IsSprinting()
    {
        return isSprinting && curMovementInput.magnitude > 0.1f && IsGrounded();
    }

    public void ForceStopSprint()
    {
        isSprinting = false;
    }

    private void Move()
    {
        float speed = moveSpeed;
        if (isSprinting)
        {
            speed *= sprintMultiplier;
        }

        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= speed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    void HeadBob()
    {
        float frequency = isSprinting ? bobFrequency * 1.5f : bobFrequency;
        float amplitude = isSprinting ? bobAmplitude * 1.5f : bobAmplitude;

        if (curMovementInput.magnitude > 0.1f && IsGrounded())
        {
            bobTimer += Time.deltaTime * frequency;

            float bobOffsetY = Mathf.Sin(bobTimer) * amplitude;
            Vector3 newPos = new Vector3(
                initialCamLocalPos.x,
                initialCamLocalPos.y + bobOffsetY,
                initialCamLocalPos.z
            );

            cameraContainer.localPosition = newPos;
        }
        else
        {
            cameraContainer.localPosition = Vector3.Lerp(
                cameraContainer.localPosition,
                initialCamLocalPos,
                Time.deltaTime * frequency
            );

            bobTimer = 0f;
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
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
