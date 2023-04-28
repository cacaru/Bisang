using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDragon_status : status
{
    public int rewardEXP { get; set; }
    public int rewardMoney { get; set; }

    public override void InitStatus()
    {
        name = "GroundDragon";
        level = 1;
        maxHP = 1500;
        curHP = maxHP;
        maxMP = 150;
        curMP = maxMP;
        Attack = Random.Range(60, 75);
        Defense = 50;
        isDead = false;
        this.passSet(false);
        attackType = 0;
        room = 0;
        stage = 0;

        rewardEXP = 100;
        rewardMoney = 1000;
    }
}
