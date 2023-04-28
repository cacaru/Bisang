using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_hpbar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public status player;
    public Text playerHPText;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        player = GameObject.Find("Knight").GetComponent<playerStatus>();
    }

    public void Update()
    {
        slider.maxValue = player.maxHP;
        slider.value = player.curHP;
        if (player.curHP > 0)
            playerHPText.text = player.curHP.ToString();
        else if (player.curHP <= 0)
            playerHPText.text = "0";
    }

    
}
