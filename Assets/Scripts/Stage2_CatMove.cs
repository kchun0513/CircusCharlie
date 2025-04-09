using UnityEngine;

public class Stage2_CatMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform player;         // 플레이어 Transform (Inspector에 연결)
    public float speed = 5f;         // 고양이 이동 속도

    void Start()
    {
        // 생성 시 플레이어를 바라보도록 설정
        if (player != null)
        {
            transform.LookAt(player);
        }
    }

    void Update()
    {
        // 현재 forward 방향으로 이동
        transform.position += transform.forward * speed * Time.deltaTime;
        
        // 플레이어보다 뒤쪽에 있다면 (예: z 좌표가 작다면) 이 고양이는 지나쳤다고 판단해서 삭제
        if(player != null && transform.position.z < player.position.z)
        {
            Destroy(gameObject);
        }
    }
}