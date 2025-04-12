using UnityEngine;

public class Stage2_CatMove : MonoBehaviour
{
    public float speed = 5f;  // 고양이 이동 속도

    void Update()
    {
        // z축 음의 방향으로 (즉, z 값이 감소하는 방향으로) 이동
        transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
        
        // 만약 고양이의 z 좌표가 0 이하가 되면 고양이 삭제
        if (transform.position.z <= 0f)
        {
            Destroy(gameObject);
        }
    }
}