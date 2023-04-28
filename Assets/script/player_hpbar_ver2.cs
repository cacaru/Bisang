using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_hpbar_ver2 : MonoBehaviour
{
    public status player;
    public Text playerName;
    public GameObject hpbar;//hp obj
    public GameObject mpbar;//mp obj
    public Image hpimage;//������ hp Image
    public Image mpimage;// ������ mp Image
    public Text playerLevel;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Knight").GetComponent<status>();
        hpbar = gameObject.transform.Find("Bars").Find("Healthbar").gameObject;
        mpbar = gameObject.transform.Find("Bars").Find("Manabar").gameObject;

        player.InitStatus();//������ �������� ���� �ѹ� �ʱ�ȭ

        playerName = gameObject.transform.Find("Name").GetComponent<Text>();
        playerLevel = gameObject.transform.Find("Level").Find("Text").GetComponent<Text>();

        playerName.text = player.name;
    }

    // Update is called once per frame
    void Update()
    {
        //Image�� fillAmount�� �ǽð����� ����
        hpimage.fillAmount = (float)player.curHP / player.maxHP;
        mpimage.fillAmount = (float)player.curMP / player.maxMP;

        playerLevel.text = player.level.ToString();
    }
}
