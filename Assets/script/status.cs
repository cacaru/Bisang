using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class status : MonoBehaviour
{
    public string name { get; set; }
    public int level { get; set; }
    public int maxHP { get; set; }
    public int curHP { get; set; }
    public int maxMP { get; set; }
    public int curMP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public bool isDead { get; set; }
    public bool isPass { get; private set; }
    public int attackType { get; set; }
    public int room { get; set; }
    public int stage { get; set; }

    public void Start()
    {
        InitStatus();    
    }

    public virtual void InitStatus() {}

    public void enemy_damaged(int attack, status target) {
        // ������ = ���ݷ� * 100 / (100 + target.����);
        int damage = attack * 100 / (100 + target.Defense);
        target.gameObject.GetComponent<show_damage>().TakeDamage(damage);
        target.curHP -= damage;
        /*
        // ������ �Դ� ��ü�� ü���� 0�� �Ǹ� �״� ��� ���
        if (target.curHP <= 0)
        {
            target.isDead = true;
            target.gameObject.GetComponent<Animator>().SetBool("die", true);
            if (target.gameObject.CompareTag("enemy"))
            {
                target.gameObject.GetComponent<Animator>().SetBool("can_move", false);
                target.gameObject.GetComponent<Animator>().SetBool("can_attack", false);
                target.gameObject.GetComponent<Animator>().SetBool("hit", false);

               
            }
            else if (target.gameObject.CompareTag("Player"))
            {
                target.gameObject.GetComponent<Animator>().SetBool("running", false);
                target.gameObject.GetComponent<Animator>().SetBool("attack_L", false);
                target.gameObject.GetComponent<Animator>().SetBool("attack_R", false);
                target.gameObject.GetComponent<Animator>().SetBool("ulti_skill", false);
                target.gameObject.GetComponent<Animator>().SetBool("hit", false);
            }
            
        }
        */
    }

    // ���� ���� ȿ���� ���� true ������
    public void true_damaged(int attack, status target) {
        target.curHP -= attack;
    }

    public void passSet(bool passOn)
    {
        this.isPass = passOn;
    }

    public void forced_Move(Vector3 point)
    {
        // �����̵�
        Debug.Log(point);
        transform.position = Vector3.MoveTowards(transform.position, point, 5.0f * Time.deltaTime);
    }

    public void give_reward(int reward, int exp, status target) {
        //���� ����ġ
        int curEx;

        target.GetComponent<playerStatus>().Money += reward;
        target.GetComponent<playerStatus>().curEXP += exp;

        //������ ���� ����ġ ����
        curEx = target.GetComponent<playerStatus>().curEXP;

        //������ ���� �޼�
        if (target.GetComponent<playerStatus>().curEXP >= target.GetComponent<playerStatus>().maxEXP)
        {
            target.GetComponent<playerStatus>().level += 1;
            target.GetComponent<playerStatus>().Attack += 5;
            target.GetComponent<playerStatus>().Defense += 3;
            target.GetComponent<playerStatus>().curHP = target.GetComponent<playerStatus>().maxHP;
            target.GetComponent<playerStatus>().curMP = target.GetComponent<playerStatus>().maxMP;
            target.GetComponent<playerStatus>().curEXP = 0 + (curEx - target.GetComponent<playerStatus>().maxEXP);
            target.GetComponent<playerStatus>().maxEXP += 80;
            //������ �̺�Ʈ ����
            target.GetComponent<event_handler>().levelup_event();
        }

    }

}
