using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text uiText;
    private int point = 0;
    public int life = 3;
    public Transform cameraTransform;
    public float distance = 5f;
    public float followSpeed = 100f;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // XR 카메라
        }

        transform.position = cameraTransform.position + cameraTransform.forward * distance;
        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }

    void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        // 목표 위치
        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distance;

        // 부드럽게 위치 이동
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        // 회전도 부드럽게 맞추기
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * followSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PointCheck")) //장애물 충돌
        {
            point += 100; // 현재 단계 재 로드
            Debug.Log("점수 : " + point);
            uiText.text = "Point: " + point.ToString(); // 변수 값을 UI에 표시
        }

        if (other.CompareTag("Obstacle"))
        {
            life -= 1;
            if (life < 0)
            {
                life = 3;
            }
        }
    }
}
