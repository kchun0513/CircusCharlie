using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [Header("Movement Speed (positive values)")]
    public int minSpeed = 2;
    public int maxSpeed = 5;

    [Header("Jump Settings (only for jump monkey)")]
    public bool canJump = false;       // Jump Monkey prefab만 체크!
    public float jumpForce = 5f;       // 튜닝 가능

    private int speed;
    private Rigidbody rb;

    void Start()
    {
        // Rigidbody 캐싱
        rb = GetComponent<Rigidbody>();
        if (rb == null && canJump)
            Debug.LogWarning($"{name}: Rigidbody가 없습니다. 점프가 동작하지 않습니다.");

        // 충돌 터널링 방지
        if (rb != null)
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // (선택) 물리 시뮬만 쓰려면 Kinematic 모드로
        // if (!canJump && rb != null)
        //     rb.isKinematic = true;

        // 속도를 뒤(음수) 방향으로 랜덤 설정
        speed = Random.Range(-maxSpeed, -minSpeed);
        Debug.Log($"[{name}] speed = {speed}");
    }

    // 물리 이동은 FixedUpdate에서
    void FixedUpdate()
    {
        if (!GameManager.Instance.CheckPaused())
        {
            if (rb != null)
            {
                // MovePosition으로 부드럽게 이동
                Vector3 nextPos = rb.position + transform.forward * speed * Time.fixedDeltaTime;
                rb.MovePosition(nextPos);
            }
            else
            {
                // Rigidbody가 없으면 기존 Translate fallback
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        // “Jump” 태그 박스에 들어오고, 점프 기능이 있을 때
        if (other.CompareTag("Jump") && canJump && rb != null)
        {
            Debug.Log($"[{name}] Monkey Jump!");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}