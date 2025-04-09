using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_PlayerRespawn : MonoBehaviour
{
    // 줄 Object를 inspector에서 드래그하여 연결
    public Transform ropeTransform;

    // 바닥과 충돌했는지 확인
    void OnControllerColliderHit(ControllerColliderHit hit)
{
    if (hit.gameObject.CompareTag("DeathZone"))
    {
        Debug.Log("떨어졌습니다. 줄 위로 리스폰합니다.");
        Respawn();
    }
}

    void Respawn()
    {
        // 플레이어의 z 위치만 유지
        float currentZ = transform.position.z;

        // 줄 x,y값을 사용 (현재 위치)
        float ropeX = ropeTransform.position.x;
        float ropeY = ropeTransform.position.y + 1.5f;  // 줄보다 위로

        // 리스폰 위치 설정
        transform.position = new Vector3(ropeX, ropeY, currentZ);
    }
}