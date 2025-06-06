using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AutoRun : MonoBehaviour
{
    [Header("이동 설정")]
    [Tooltip("초당 기본 이동 속도(걷기)")]
    public float speed = 5f;
    [Tooltip("Shift 누르고 있을 때의 이동 속도(달리기)")]
    public float runSpeed = 10f; 

    [Header("점프 설정")]
    [Tooltip("점프 시 위로 가하는 힘")]
    public float jumpForce = 5f;

    // Rigidbody 컴포넌트 참조
    private Rigidbody rb;

    // 말과 말 자식의 모든 Renderer 모아두는 배열
    private Renderer[] horseRenderers;

    // 지금 땅에 붙어 있는 상태인지
    private bool isGrounded = true;

    // 현재 실제로 사용할 이동 속도 (FixedUpdate 때마다 갱신)
    private float currentSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // 말 본체와 모든 자식 오브젝트에 붙어 있는 Renderer를 미리 수집
        horseRenderers = GetComponentsInChildren<Renderer>();

        // 시작할 때는 말이 땅 위에 서 있다고 가정 → 모든 Renderer 보여준다
        SetHorseRenderersVisible(true);
        isGrounded = true;

        // 초기 currentSpeed는 기본 speed
        currentSpeed = speed;
    }

    void Update()
    {
        // 1) Jump 키 입력 처리
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // 말을 “숨기기” 모드로 전환
            SetHorseRenderersVisible(false);
            isGrounded = false;

            // Rigidbody를 통해 위로 점프
            Vector3 v = rb.velocity;
            v.y = 0f;            // 혹시 이미 내려가는 속도가 남아 있으면 초기화
            rb.velocity = v;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }

        // 2) Shift 키 입력 체크 → currentSpeed 설정
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // Shift를 누르고 있으면 달리기 속도
            currentSpeed = runSpeed;
        }
        else
        {
            // Shift를 떼면 기본 속도로
            currentSpeed = speed;
        }
    }

    void FixedUpdate()
    {
        // 3) 항상 +Z 방향으로 자동 이동
        // currentSpeed 값이 Update()에서 매 프레임 갱신됨
        Vector3 forwardDelta = Vector3.forward * (currentSpeed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + forwardDelta);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 4) 말이 땅(“Road” 태그)이 붙은 오브젝트와 충돌했을 때
        if (collision.gameObject.CompareTag("Road"))
        {
            // 땅에 닿으면 다시 말 보이게
            SetHorseRenderersVisible(true);
            isGrounded = true;
        }
    }

    /// <summary>
    /// 말 본체 및 자식에 있는 모든 Renderer를 보이거나 숨깁니다.
    /// </summary>
    private void SetHorseRenderersVisible(bool visible)
    {
        foreach (var rend in horseRenderers)
        {
            rend.enabled = visible;
        }
    }
}
