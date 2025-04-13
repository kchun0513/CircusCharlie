using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayer : MonoBehaviour
{
    public float speed = 3.0f;      // 이동 속도
    public float jumpForce = 7.5f;  // 점프 힘

    private Vector3 velocity;       // 중력 및 점프를 위한 속도 벡터
    private CharacterController cc;

    void Start()
    {
        // CharacterController 컴포넌트 가져오기
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. VR 컨트롤러 입력 받아서 전후 이동 처리
        // 왼쪽 컨트롤러의 썸스틱 수직축 (앞뒤) 값을 이용
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector3 moveDirection = transform.forward * input.y * speed;
        
        // 2. 점프 처리 및 중력 적용
        if (cc.isGrounded)
        {
            // 바닥에 있을 때 점프 버튼 (A 버튼: OVRInput.Button.One) 입력 확인
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                velocity.y = jumpForce;
            }
            else
            {
                // 바닥에 있으면 수직 속도 초기화
                velocity.y = 0;
            }
        }
        // 매 프레임 중력 적용 (Physics.gravity.y는 음수)
        velocity.y += Physics.gravity.y * Time.deltaTime;

        // 3. 최종 이동 벡터 계산 후 CharacterController로 이동
        Vector3 finalMovement = moveDirection + velocity;
        cc.Move(finalMovement * Time.deltaTime);
    }
}