using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStatus : status
{
    public int maxEXP { get; set; }
    public int curEXP { get; set; }
    public int Money { get; set; }

    public override void InitStatus()
    {
        name = "Warrior";
        level = 1;
        maxHP = 700;
        curHP = maxHP;
        maxMP = 200;
        curMP = maxMP;
        Attack = 60;
        Defense = 50;
        isDead = false;
        this.passSet(false);
        attackType = 0;
        room = 0;
        stage = 0;

        curEXP = 0;
        maxEXP = 100;
        Money = 0;
    }
}
