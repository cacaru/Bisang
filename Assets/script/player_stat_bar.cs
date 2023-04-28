using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_stat_bar : MonoBehaviour
{
    public status player;
    public Text playerNameInfo;
    public Text playerLevelInfo;
    public Text playerMaxHPInfo;
    public Text playerMaxMPInfo;
    public Text playerAttackInfo;
    public Text playerDefenseInfo;
    public Text playerMaxEXPInfo;
    public Text playerMoneyInfo;

    public static bool statisOpen = false;
    public GameObject statMenuCanvas;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Knight").GetComponent<status>();
        player.InitStatus();//정보를 가져오기 위해 한번 초기화

        playerNameInfo = gameObject.transform.Find("Panel").Find("Name").GetComponent<Text>();
        playerLevelInfo = gameObject.transform.Find("Panel").Find("Level").GetComponent<Text>();
        playerMaxHPInfo = gameObject.transform.Find("Panel").Find("MaxHP").GetComponent<Text>();
        playerMaxMPInfo = gameObject.transform.Find("Panel").Find("MaxMP").GetComponent<Text>();
        playerAttackInfo = gameObject.transform.Find("Panel").Find("Attack").GetComponent<Text>();
        playerDefenseInfo = gameObject.transform.Find("Panel").Find("Defense").GetComponent<Text>();
        playerMaxEXPInfo = gameObject.transform.Find("Panel").Find("MaxEXP").GetComponent<Text>();
        playerMoneyInfo = gameObject.transform.Find("Panel").Find("Money").GetComponent<Text>();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (statisOpen)
                closeplayerInfo();
            else
                playerInfo();
        }
    }

    public void playerInfo() {
        statMenuCanvas.SetActive(true);
        statisOpen = true;

        //정보 갱신
        playerNameInfo.text = "이름 : " + player.name;
        playerLevelInfo.text = "레벨 : " + player.level.ToString();
        playerMaxHPInfo.text = "최대 HP / 현재 HP : " + player.maxHP.ToString() + "/" + player.curHP.ToString();
        playerMaxMPInfo.text = "최대 MP / 현재 MP: " + player.maxMP.ToString() + "/" + player.curMP.ToString();
        playerAttackInfo.text = "공격력 : " + player.Attack.ToString();
        playerDefenseInfo.text = "방어력 : " + player.Defense.ToString();
        playerMaxEXPInfo.text = "최대 경험치 / 현재 경험치 : " + player.GetComponent<playerStatus>().maxEXP.ToString() + "/" + player.GetComponent<playerStatus>().curEXP.ToString();
        playerMoneyInfo.text = "돈 : " + player.GetComponent<playerStatus>().Money.ToString();

    }

    public void closeplayerInfo() {
        statMenuCanvas.SetActive(false);
        statisOpen = false;
    }
}
