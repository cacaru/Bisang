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
        player.InitStatus();//������ �������� ���� �ѹ� �ʱ�ȭ

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

        //���� ����
        playerNameInfo.text = "�̸� : " + player.name;
        playerLevelInfo.text = "���� : " + player.level.ToString();
        playerMaxHPInfo.text = "�ִ� HP / ���� HP : " + player.maxHP.ToString() + "/" + player.curHP.ToString();
        playerMaxMPInfo.text = "�ִ� MP / ���� MP: " + player.maxMP.ToString() + "/" + player.curMP.ToString();
        playerAttackInfo.text = "���ݷ� : " + player.Attack.ToString();
        playerDefenseInfo.text = "���� : " + player.Defense.ToString();
        playerMaxEXPInfo.text = "�ִ� ����ġ / ���� ����ġ : " + player.GetComponent<playerStatus>().maxEXP.ToString() + "/" + player.GetComponent<playerStatus>().curEXP.ToString();
        playerMoneyInfo.text = "�� : " + player.GetComponent<playerStatus>().Money.ToString();

    }

    public void closeplayerInfo() {
        statMenuCanvas.SetActive(false);
        statisOpen = false;
    }
}
