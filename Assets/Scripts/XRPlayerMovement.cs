using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using Unity.XR.CoreUtils;

public class XRPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float jumpForce = 2.0f;
    public float gravity = -9.81f;

    public InputActionProperty moveInput;  // ���̽�ƽ �̵� �Է�
    public InputActionProperty jumpInput;  // ���� ��ư �Է�

    private CharacterController characterController;
    private XROrigin xrOrigin;
    private Vector3 velocity;
    private bool isActive;
    private bool isGrounded;
    void Start()
    {
        isActive = XRSettings.isDeviceActive;
        if (isActive)
        {
            characterController = GetComponent<CharacterController>();
            xrOrigin = GetComponent<XROrigin>();
        }
    }

    void Update()
    {
        if (isActive)
        {
            isGrounded = characterController.isGrounded;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = 0f;
            }

            // ���̽�ƽ �̵�
            Vector2 input = moveInput.action.ReadValue<Vector2>();
            Vector3 move = new Vector3(input.x, 0, input.y);
            move = xrOrigin.Camera.transform.TransformDirection(move);
            move.y = 0;

            characterController.Move(move * moveSpeed * Time.deltaTime);

            // ���� ó��
            if (jumpInput.action.WasPressedThisFrame() && isGrounded)
            {
                velocity.y += Mathf.Sqrt(jumpForce * -2f * gravity);
            }

            /// �߷� ����
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}
