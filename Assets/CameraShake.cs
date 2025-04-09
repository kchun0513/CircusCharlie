using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float frequency = 5f;

    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        float shake = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = originalPos + new Vector3(0, shake, 0);
    }
}
