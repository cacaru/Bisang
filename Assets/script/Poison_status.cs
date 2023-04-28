using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_status : status
{
    public int rewardEXP { get; set; }
    public int rewardMoney { get; set; }

    public override void InitStatus()
    {
        name = "Poison";
        level = 1;
        maxHP = 0;
        curHP = maxHP;
        maxMP = 0;
        curMP = maxMP;
        Attack = 10;
        Defense = 50;
        isDead = false;
        this.passSet(false);
        attackType = 0;
        room = 0;
        stage = 0;

        rewardEXP = 50;
        rewardMoney = 1000;
    }
}
