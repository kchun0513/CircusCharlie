using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalanceManager : MonoBehaviour
{
    // Start is called before the first frame update
    public BalanceZone LeftBalanceZone;
    public BalanceZone RightBalanceZone;
    public float timeRequired = 0.1f;
    public float BalanceLoss = 0.5f;
    public PlayerManager player;
    public ThirdPersonController thpCon;
    public Slider BalanceSlider; //균형
    public Image SliderFill;
    public TMP_Text BalanceText;
    public Image NoticePanel;
    private float timer = 0f;
    private bool BalanceInitialized = false;
    private bool _isFalling = false;


    void Start()
    {
        BalanceSlider.value = 100.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.clear)
        {
            _CheckBalance();
        }  
        if (BalanceSlider.value <= 0)
        {
            if (!_isFalling) // 중복 방지
            {
                StartCoroutine(FallSequence());
            }
        }
    }

    void _CheckBalance()
    {
        if (!GameManager.Instance.CheckPaused())
        {
            timer += Time.deltaTime;
            //Debug.Log(timer);
            float t = BalanceSlider.value / 100f;
            SliderFill.color = new Color(1f - t, t, 0f, 1f);
            if (LeftBalanceZone.isHovering && RightBalanceZone.isHovering)
            {
                if (BalanceSlider.value < 100 && timer >= timeRequired)
                {
                    BalanceSlider.value += 1f;
                    timer = 0f;
                }
            }
            else
            {
                if (timer >= timeRequired)
                {
                    BalanceSlider.value -= 1f;
                    timer = 0f;
                }
            }
        } else if (GameManager.Instance.CheckPaused() && !BalanceInitialized)
        {
            if (LeftBalanceZone.isHovering && RightBalanceZone.isHovering)
            {
                BalanceInitialized = true;
                BalanceText.text = "";
                NoticePanel.enabled = false;
                GameManager.Instance.GameRestart();
            }
        }
    }

    // 밸런스 값이 0일 때 떨어지는 효과
    IEnumerator FallSequence()
    {
        _isFalling = true;
        SoundManager.Instance.PlayCrowdReaction(false);
        GameObject rope = GameObject.Find("Rope");
        if (rope != null)
        {
            Collider col = rope.GetComponent<MeshCollider>();
            if (col != null) col.enabled = false;
        }

        yield return new WaitForSeconds(1.0f);

        if (GameManager.Instance.life <= 0)
        {
            player.GameOver();
        }
        else
        {
            player.playerDead();
        }
    }

}
