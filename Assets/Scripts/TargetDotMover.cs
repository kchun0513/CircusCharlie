using UnityEngine;

public class TargetDotMover : MonoBehaviour
{
    public RectTransform panel;
    public RectTransform dot;
    public float maxSpeed = 50f;
    public float minSpeed = 30f;
    public float acceleration = 10f;

    private float currentSpeed;
    private float targetY;
    private float moveRange;
    private Vector2 initialPosition;

    void Start()
    {
        if (panel == null || dot == null)
        {
            Debug.LogError("panel �Ǵ� dot�� �Ҵ���� �ʾҽ��ϴ�!");
            enabled = false;
            return;
        }

        currentSpeed = Random.Range(minSpeed, maxSpeed);
        float padding = 20f;
        moveRange = (panel.rect.height * 0.5f) - padding;

        initialPosition = dot.anchoredPosition;
        SetNewTargetY();
    }

    void Update()
    {
        Vector2 currentPos = dot.anchoredPosition;
        float step = currentSpeed * Time.deltaTime;

        // ���� �ڵ� ���
        float dir = Mathf.Sign(targetY - currentPos.y);
        currentPos.y += step * dir;

        dot.anchoredPosition = currentPos;

        // ���� �� �� ��ǥ ����
        if (Mathf.Abs(currentPos.y - targetY) < 1f)
        {
            currentSpeed = Random.Range(minSpeed, maxSpeed);
            SetNewTargetY();
        }

        // �г� �� ��ġ ����
        float clampedY = Mathf.Clamp(dot.anchoredPosition.y, -moveRange, moveRange);
        dot.anchoredPosition = new Vector2(initialPosition.x, clampedY);
    }

    void SetNewTargetY()
    {
        targetY = Random.Range(-moveRange, moveRange);
    }
}
