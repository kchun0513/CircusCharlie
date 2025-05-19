using System.Collections;
using System.Collections.Generic;
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
    public Slider BalanceSlider; //±ÕÇü
    private float timer = 0f;

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
    }
}
