using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class check_skill : MonoBehaviour
{
    public playerStatus player;
    public Image Skill_q;
    public Image Skill_r;

    private Color qcolor;
    private Color rcolor;

    private void Start()
    {
        player = GameObject.Find("Knight").GetComponent<playerStatus>();
        //Skill_q = gameObject.transform.Find("Skill_Q").GetComponent<Image>();
        //Skill_r = gameObject.transform.Find("Skill_R").GetComponent<Image>();

        //원래 색상 저장
        qcolor = Skill_q.material.color;
        rcolor = Skill_r.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        check_q();
        check_r();
    }

    public void check_q() {
        if (player.curMP < 20)
            Skill_q.color = Color.gray;
        else
            Skill_q.color = qcolor;
    }

    public void check_r() {
        if (player.curMP < 100)
            Skill_r.color = Color.grey;
        else
            Skill_r.color = rcolor;
    }
}
