using UnityEngine;
using UnityEngine.InputSystem;  // Send Messages 모드에서도 InputValue 쓰려면 필요합니다.

[RequireComponent(typeof(CharacterController))]
public class PCPlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -9.81f;

    CharacterController controller;
    Vector2 moveInput;
    float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Send Messages 모드가 호출하는 콜백
    // Move 액션이 발생하면 InputValue에 담겨 온 Vector2 값을 꺼내 씁니다.
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Jump 액션(버튼)도 같은 방식으로 받습니다.
    void OnJump(InputValue value)
    {
        // performed만 걸러내고 싶으면 value.isPressed 사용
        if (value.isPressed && controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Update()
    {
        // 수평 이동
        Vector3 horiz = (transform.right * moveInput.x + transform.forward * moveInput.y)
                        * moveSpeed * Time.deltaTime;
        controller.Move(horiz);

        // 점프/중력 처리
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = 0f;
    }
}