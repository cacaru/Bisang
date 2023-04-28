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
    public Image hpimage;//하위의 hp Image
    public Image mpimage;// 하위의 mp Image
    public Text playerLevel;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Knight").GetComponent<status>();
        hpbar = gameObject.transform.Find("Bars").Find("Healthbar").gameObject;
        mpbar = gameObject.transform.Find("Bars").Find("Manabar").gameObject;

        player.InitStatus();//정보를 가져오기 위해 한번 초기화

        playerName = gameObject.transform.Find("Name").GetComponent<Text>();
        playerLevel = gameObject.transform.Find("Level").Find("Text").GetComponent<Text>();

        playerName.text = player.name;
    }

    // Update is called once per frame
    void Update()
    {
        //Image의 fillAmount를 실시간으로 변경
        hpimage.fillAmount = (float)player.curHP / player.maxHP;
        mpimage.fillAmount = (float)player.curMP / player.maxMP;

        playerLevel.text = player.level.ToString();
    }
}
