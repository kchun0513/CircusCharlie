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
            cameraTransform = Camera.main.transform;
        }

        transform.position = cameraTransform.position + cameraTransform.forward * distance;
        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }

    void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distance;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        Quaternion targetRotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * followSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PointCheck"))
        {
            point += 100;
            Debug.Log("���� : " + point);
            uiText.text = "Point: " + point.ToString();
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
