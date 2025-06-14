using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenMonkeyController : MonoBehaviour
{
    [Header("이동 속도")]
    public float moveSpeed = 2f;

    [Header("초기화 범위 및 삭제 조건")]
    public float destroyX = -10f;  // 왼쪽 바깥으로 나가면 삭제

    [Header("점프 설정")]
    public bool canJump = false;       // 빠른 monkey만 true
    public float jumpForce = 5f;
    public string jumpTriggerTag = "Jump";

    private Rigidbody rb;
    private bool hasJumped = false;

    void Start()
    {
        // monkey를 왼쪽 바라보게 회전
        transform.rotation = Quaternion.Euler(0f, 270f, 0f);

        rb = GetComponent<Rigidbody>();
        if (rb == null && canJump)
            Debug.LogWarning($"{name}: Rigidbody가 없습니다. 점프가 동작하지 않습니다.");

        if (rb != null)
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        // 월드 좌표 기준 왼쪽(-x)으로 이동
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);

        // 화면 왼쪽 바깥으로 나가면 제거 또는 재활용
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
            // 또는 풀링 시스템에 넣는 것도 가능
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(jumpTriggerTag) && canJump && rb != null && !hasJumped)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            hasJumped = true;
        }
    }
}