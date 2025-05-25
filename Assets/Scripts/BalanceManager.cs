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
    public Slider BalanceSlider; //����
    public TMP_Text BalanceText;
    private float timer = 0f;
    private bool BalanceInitialized = false;
    

    void Start()
    {
        BalanceSlider.value = 100.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _CheckBalance();
        if (BalanceSlider.value <= 0)
        {
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

    void _CheckBalance()
    {
        if (!GameManager.Instance.CheckPaused())
        {
            timer += Time.deltaTime;
            //Debug.Log(timer);
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
                GameManager.Instance.GameRestart();
            }
        }
    }
}
