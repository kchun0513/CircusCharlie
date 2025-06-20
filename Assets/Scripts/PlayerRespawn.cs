using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    public Transform ropeTransform;                   // 줄 위치
    public TextMeshProUGUI countdownText;             // 카운트다운 UI

    private StarterAssets.ThirdPersonController playerController;
    private StarterAssets.StarterAssetsInputs input;

    private bool isRespawning = false;                // 리스폰 중 여부

    void Start()
    {
        playerController = GetComponent<StarterAssets.ThirdPersonController>();
        input = GetComponent<StarterAssets.StarterAssetsInputs>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 바닥에 닿으면 리스폰 시작
        if (!isRespawning && hit.gameObject.CompareTag("DeathZone"))
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.fallingClip); // falling 효과음 재생
            Debug.Log("떨어졌습니다. 리스폰 위치로 이동 + 카운트다운 시작!");
            StartCoroutine(RespawnWithCountdown());
        }

        // 고양이(또는 장애물, Obstacle)에 닿으면 리스폰 시작 (DeathZone과 동일한 방식으로 처리)
        if (!isRespawning && hit.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("고양이(장애물)와 충돌했습니다. 리스폰 위치로 이동 + 카운트다운 시작!");
            StartCoroutine(RespawnWithCountdown());
        }
    }

    IEnumerator RespawnWithCountdown()
    {
        isRespawning = true;

        // 1. 캐릭터 움직임 정지
        if (playerController != null)
            playerController.enabled = false;

        // 2. 입력 강제 차단
        if (input != null)
            input.move = Vector2.zero;

        // 3. 줄 위 리스폰 위치로 이동 (x=0, z 유지, y=ropeTransform.y + 10)
        // CharacterController 충돌 처리를 피하기 위해 비활성화 후 위치 변경
        var cc = GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false;

        float currentZ = transform.position.z;
        float respawnY = ropeTransform.position.y + 10f;
        Vector3 spawnPosition = new Vector3(0f, respawnY, currentZ);
        Debug.Log($"Respawning at {spawnPosition}");
        transform.position = spawnPosition;

        // Removed re-enable of CharacterController here

        // 4. 카운트다운 표시
        countdownText.gameObject.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownText.gameObject.SetActive(false);

        // 5a. CharacterController 다시 활성화
        cc = GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = true;

        // 5. 캐릭터 다시 활성화
        if (playerController != null)
            playerController.enabled = true;

        // 6. 리스폰 완료 → 입력 허용
        isRespawning = false;
    }

    void Update()
    {
        // 리스폰 중에는 입력 차단 유지
        if (isRespawning && input != null)
        {
            input.move = Vector2.zero;
        }
    }
}