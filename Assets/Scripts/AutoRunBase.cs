using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AutoRunBase : MonoBehaviour
{
    [Header("이동 설정")]
    [Tooltip("초당 기본 이동 속도(걷기)")]
    public float speed = 5f;
    [Tooltip("Shift 누르고 있을 때의 이동 속도(달리기)")]
    public float runSpeed = 10f;
    [Header("플레이어")]
    public GameObject Player;

    private Rigidbody rb;
    public float currentSpeed;
    private ThirdPersonController playerController;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = speed;
        if (Player != null)
        {
            playerController = Player.GetComponent<ThirdPersonController>();
        }
    }

    void Update()
    {
        // Shift 키 입력 체크 → currentSpeed 설정
        bool runInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ||
                        (playerController != null && playerController.movementState == 1);

        currentSpeed = runInput ? runSpeed : speed;
    }

    void FixedUpdate()
    {
        // 항상 +Z 방향으로 자동 이동
        Vector3 forwardDelta = Vector3.forward * (currentSpeed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + forwardDelta);
    }

    public bool GetJumpValue()
    {
        return playerController.GetJumpValueForStage3();
    }

    public void SetJumpFalse()
    {
        playerController.SetJumpfalseForStage3();
    }
}
