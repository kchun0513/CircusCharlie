using StarterAssets;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float frequency = 5f;

    private Vector3 originalPos;

    [Header("플레이어")]
    public GameObject Player;
    private ThirdPersonController playerController;

    private void Awake()
    {
        if (Player != null)
        {
            playerController = Player.GetComponent<ThirdPersonController>();
        }
    }

    void Start()
    {
        originalPos = transform.localPosition;

    }

    void Update()
    {
        bool runInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ||
                (playerController != null && playerController.movementState == 1);
        float shake = Mathf.Sin(Time.time * frequency * (runInput ? 1.5f : 1f)) * amplitude * (runInput ? 2f : 1f);
        transform.localPosition = originalPos + new Vector3(0, shake, 0);
    }
}
