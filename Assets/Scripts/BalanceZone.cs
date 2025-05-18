using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BalanceZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image targetImage;
    public float holdTimeRequired = 1.5f;
    public bool isHovering = false;

    private float holdTimer = 0f;

    void Update()
    {
        if (isHovering)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdTimeRequired)
            {
                targetImage.color = Color.green; // 성공
            }
            else
            {
                targetImage.color = Color.yellow; // 유지 중
            }
        }
        else
        {
            holdTimer = 0f;
            targetImage.color = Color.red; // Hover 중 아님
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
