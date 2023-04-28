using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit_calculator : MonoBehaviour
{
    // ������ ����
    // ������ = ���ݷ� * 100 / (100 + target.����) ;
    private GameObject attacker;
    private int attackType;
    private status victim;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("�Ӽ���!");
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
            // �÷��̾� to ����
            case "Player":
                this.player_hit_enemy();
                break;
            // ���� to �÷��̾�
            case "enemy":
                this.enemy_hit_player();
                break;
            // ����� to �÷��̾�
            case "debuff":
                this.debuff_hit_player();
                break;
        }
    }

    // ĳ���Ͱ� ���͸� ���� ��. case ����
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
        // 1 :: ��Ÿ�� ���� ��
        // 2 :: ����ġ��� ���� ��
        // 3 :: ������ ���� ��

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
                Debug.Log("����");
                break;
            default:
                break;
        }
        //�׾����� Ȯ��
        isDead();
    }



    // ���Ͱ� ĳ���͸� ���� ��. case ����
    private void enemy_hit_player()
    {
        // ��밡 �����̸� ����
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
        //�׾����� Ȯ��
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
                Debug.Log("���� Ÿ������ ���ּ���.");
                break;
            default:
                break;
        }
        //�׾����� Ȯ��
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
                Debug.Log("���� Ÿ������ ���ּ���.");
                break;
            default:
                break;
        }
        //�׾����� Ȯ��
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
                Debug.Log("���� Ÿ������ ���ּ���.");
                break;
            default:
                break;
        }
        //�׾����� Ȯ��
        isDead();
    }

    //ĳ���Ͱ� ������� �������� ������. case ����
    private void debuff_hit_player()
    {
        switch (this.attacker.name)
        {
            case "Poison":
                this.poison_attack();
                break;
        }
        //�׾����� Ȯ��
        isDead();
    }

    //���� ���� ������
    private void poison_attack()
    {
        status enemy = this.attacker.GetComponent<status>();
        status target = this.victim;

        target.true_damaged(enemy.Attack, target);
        Debug.Log(this.attacker.name + " Attack! HP = " + target.curHP);
        //�׾����� Ȯ��
        isDead();
    }

    public void isDead() {
        //string pname = victim.gameObject.transform.parent.name;
        // ������ �Դ� ��ü�� ü���� 0�� �Ǹ� �״� ��� ���
        if (victim.curHP <= 0 && victim.isDead == false)
        {
            victim.isDead = true;
            victim.gameObject.GetComponent<Animator>().SetBool("die", true);
            if (victim.gameObject.CompareTag("enemy"))
            {
                victim.gameObject.GetComponent<Animator>().SetBool("can_move", false);
                victim.gameObject.GetComponent<Animator>().SetBool("can_attack", false);
                victim.gameObject.GetComponent<Animator>().SetBool("hit", false);

                //����ġ�� �� ����
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
                //���� ���ڸ� ���̰� ���� Ŭ���� �Ǿ��� �� Ȯ��
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
