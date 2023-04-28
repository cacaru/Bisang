using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit_calculator : MonoBehaviour
{
    // 데미지 계산식
    // 데미지 = 공격력 * 100 / (100 + target.방어력) ;
    private GameObject attacker;
    private int attackType;
    private status victim;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("왓쇼이!");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void calc_status(GameObject attacker, status victim)
    {
        //hit calculator 
        this.attacker = attacker;
        this.attackType = attacker.GetComponent<status>().attackType;
        this.victim = victim;

        switch (attacker.tag)
        {
            // 플레이어 to 몬스터
            case "Player":
                this.player_hit_enemy();
                break;
            // 몬스터 to 플레이어
            case "enemy":
                this.enemy_hit_player();
                break;
            // 디버프 to 플레이어
            case "debuff":
                this.debuff_hit_player();
                break;
        }
    }

    // 캐릭터가 몬스터를 떄릴 때. case 삽입
    private void player_hit_enemy()
    {
        switch (this.attacker.name)
        {
            case "Knight":
                this.knight_attack();
                break;
        }
    }

    private void knight_attack()
    {
        // 1 :: 평타로 들어가는 딜
        // 2 :: 방패치기로 들어가는 딜
        // 3 :: 궁으로 들어가는 딜

        status player = this.attacker.GetComponent<status>();
        status target = this.victim;


        int damage = player.Attack;
        switch (attackType)
        {
            case 1:
                target.enemy_damaged(damage, target);
                Debug.Log(this.attacker.name + " Attack! HP = " + target.curHP);
                break;
            case 2:
                damage = (int)(player.Attack * 1.5);
                target.enemy_damaged(damage, target);
                Debug.Log(this.attacker.name + " Attack! HP = " + target.curHP);
                break;
            case 3:
                damage = (int)(player.Attack * 3);
                target.enemy_damaged(damage, target);
                Debug.Log(this.attacker.name + " Attack! HP = " + target.curHP);
                break;
            case 4:
                damage = (int)(player.Attack * 0.1);
                target.enemy_damaged(damage, target);
                target.forced_Move(attacker.GetComponent<player_skill>().vortexPoint);
                Debug.Log(this.attacker.name + " Attack! HP = " + target.curHP);
                break;
            case 0:
                Debug.Log("버그");
                break;
            default:
                break;
        }
        //죽었는지 확인
        isDead();
    }



    // 몬스터가 캐릭터를 떄릴 때. case 삽입
    private void enemy_hit_player()
    {
        // 상대가 무적이면 무시
        if (this.victim.isPass)
            return;

        switch (this.attacker.GetComponent<status>().name)
        {
            case "Slime":
                slime_attack();
                break;
            case "Skeleton":
                skeleton_attack();
                break;
            case "GroundDragon":
                ground_dragon_attack();
                break;
        }
        //죽었는지 확인
        isDead();
    }

    private void slime_attack()
    {
        status enemy = this.attacker.GetComponent<status>();
        status target = this.victim;

        int damage = enemy.Attack;
        switch (attackType)
        {
            case 1:
                target.enemy_damaged(damage, target);
                Debug.Log(this.attacker.name + " Attack! HP = " + target.curHP);
                break;
            case 0:
                Debug.Log("어택 타입지정 해주세요.");
                break;
            default:
                break;
        }
        //죽었는지 확인
        isDead();
    }

    private void skeleton_attack()
    {
        status enemy = this.attacker.GetComponent<status>();
        status target = this.victim;

        int damage = enemy.Attack;
        switch (attackType)
        {
            case 1:
                target.enemy_damaged(damage, target);
                Debug.Log(this.attacker.name + " Attack! HP = " + target.curHP);
                break;
            case 0:
                Debug.Log("어택 타입지정 해주세요.");
                break;
            default:
                break;
        }
        //죽었는지 확인
        isDead();
    }

    private void ground_dragon_attack() 
    {
        status enemy = this.attacker.GetComponent<status>();
        status target = this.victim;

        int damage = enemy.Attack;
        switch (attackType)
        {
            case 1:
                target.enemy_damaged(damage, target);
                Debug.Log(this.attacker.name + " Attack! claw 1 HP = " + target.curHP);
                break;
            case 2:
                target.enemy_damaged(damage, target);
                Debug.Log(this.attacker.name + " Attack! hom 2 HP = " + target.curHP);
                break;
            case 3:
                target.enemy_damaged(damage, target);
                Debug.Log(this.attacker.name + " Attack! poison 3 HP = " + target.curHP);
                break;
            case 0:
                Debug.Log("어택 타입지정 해주세요.");
                break;
            default:
                break;
        }
        //죽었는지 확인
        isDead();
    }

    //캐릭터가 디버프에 데미지를 받을때. case 삽입
    private void debuff_hit_player()
    {
        switch (this.attacker.name)
        {
            case "Poison":
                this.poison_attack();
                break;
        }
        //죽었는지 확인
        isDead();
    }

    //독에 의한 데미지
    private void poison_attack()
    {
        status enemy = this.attacker.GetComponent<status>();
        status target = this.victim;

        target.true_damaged(enemy.Attack, target);
        Debug.Log(this.attacker.name + " Attack! HP = " + target.curHP);
        //죽었는지 확인
        isDead();
    }

    public void isDead() {
        //string pname = victim.gameObject.transform.parent.name;
        // 데미지 입는 주체의 체력이 0이 되면 죽는 모션 출력
        if (victim.curHP <= 0 && victim.isDead == false)
        {
            victim.isDead = true;
            victim.gameObject.GetComponent<Animator>().SetBool("die", true);
            if (victim.gameObject.CompareTag("enemy"))
            {
                victim.gameObject.GetComponent<Animator>().SetBool("can_move", false);
                victim.gameObject.GetComponent<Animator>().SetBool("can_attack", false);
                victim.gameObject.GetComponent<Animator>().SetBool("hit", false);

                //경험치와 돈 제공
                switch (victim.name)
                {
                    case "Slime":
                        attacker.GetComponent<status>().give_reward(victim.GetComponent<Slime_Status>().rewardMoney, 
                            victim.GetComponent<Slime_Status>().rewardEXP, 
                            attacker.GetComponent<status>());
                        break;
                    case "Skeleton":
                        attacker.GetComponent<status>().give_reward(victim.GetComponent<Skeleton_status>().rewardMoney,
                           victim.GetComponent<Skeleton_status>().rewardEXP,
                           attacker.GetComponent<status>());
                        break;
                    case "GroundDragon":
                        attacker.GetComponent<status>().give_reward(victim.GetComponent<GroundDragon_status>().rewardMoney,
                           victim.GetComponent<GroundDragon_status>().rewardEXP,
                           attacker.GetComponent<status>());
                        break;
                }
                //적의 숫자를 줄이고 방이 클리어 되었는 지 확인
                GameObject.Find("map").transform.Find(victim.gameObject.transform.parent.name).GetComponent<Room_status>().check_clear();
            }
            else if (victim.gameObject.CompareTag("Player"))
            {
                victim.gameObject.GetComponent<Animator>().SetBool("running", false);
                victim.gameObject.GetComponent<Animator>().SetBool("attack_L", false);
                victim.gameObject.GetComponent<Animator>().SetBool("attack_R", false);
                victim.gameObject.GetComponent<Animator>().SetBool("ulti_skill", false);
                victim.gameObject.GetComponent<Animator>().SetBool("hit", false);
            }

        }
    }
}
