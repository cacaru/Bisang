using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_status : status
{
    public int rewardEXP { get; set; }
    public int rewardMoney { get; set; }

    public override void InitStatus()
    {
        name = "Skeleton";
        level = 1;
        maxHP = 200;
        curHP = maxHP;
        maxMP = 150;
        curMP = maxMP;
        Attack = Random.Range(20, 25);
        Defense = 25;
        isDead = false;
        this.passSet(false);
        attackType = 0;
        room = 0;
        stage = 0;

        rewardEXP = 25;
        rewardMoney = 10;
    }
}
