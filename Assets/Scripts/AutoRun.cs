using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HorseAutoRun : MonoBehaviour
{
    [Tooltip("초당 앞으로 이동 속도")]
    public float speed = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // +Z 방향으로 일정 속도만큼 이동
        Vector3 delta = Vector3.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + delta);
    }
}