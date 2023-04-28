using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class event_handler : MonoBehaviour
{
    private Animator animator;
    private GameObject magic_circle;
    private GameObject element_arr;
    private GameObject vortex;
    public GameObject levelupEffect;

    // ���� ���� ��ü
    public GameObject audio;

    BoxCollider coliderWepon;
    private GameObject objWeapon;
    BoxCollider coliderShield;
    private GameObject objShield;
    CapsuleCollider coliderMagic;
    private GameObject objMagic;
    CapsuleCollider coliderVortex;
    private GameObject objVortex;

    public GameObject hit_effect;         // �÷��̾� ��Ʈ ����Ʈ (�־����� ������ ã�� ����)

    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.Find("Knight").GetComponent<characterMove>().animator;
        magic_circle = GameObject.Find("Knight").GetComponent<player_skill>().magic_circle;
        element_arr = GameObject.Find("Knight").GetComponent<player_skill>().element_arr;
        vortex = GameObject.Find("Knight").GetComponent<player_skill>().vortex;
        

        // ��Ʈ ���� �ִ� ������Ʈ ����
        objWeapon = GameObject.Find("Sword");
        coliderWepon = objWeapon.GetComponent<BoxCollider>();

        objShield = GameObject.Find("Shield");
        coliderShield = objShield.GetComponent<BoxCollider>();

        objMagic = GameObject.Find("Magic").transform.Find("ElementalArrow2").transform.Find("Light").gameObject;
        coliderMagic = objMagic.GetComponent<CapsuleCollider>();

        //objVortex = GameObject.Find("Magic").transform.Find("Vortex").transform.Find("Ring").gameObject;
        //coliderVortex = objVortex.GetComponent<CapsuleCollider>();

        coliderWepon.enabled = false;//������ colider ��Ȱ��ȭ
        coliderShield.enabled = false;//������ colider ��Ȱ��ȭ
        coliderMagic.enabled = false;//������ colider ��Ȱ��ȭ
        //coliderVortex.enabled = false;//skill2�� colider ��Ȱ��ȭ

        if (hit_effect)
            hit_effect.SetActive(false);
    }

    // ĳ���� �´� ���
    public void player_hit_start(int val)
    {
        if (val >= 1)
        {
            Vector3 effect_pos;
            effect_pos.x = gameObject.transform.position.x + 0.1f;
            effect_pos.y = gameObject.transform.position.y + 0.3f;
            effect_pos.z = gameObject.transform.position.z + 0.1f;
            hit_effect.transform.position = effect_pos;
            hit_effect.SetActive(true);
        }
    }

    public void player_hit_end(int val)
    {
        if (val >= 1)
        {
            animator.SetBool("hit", false);
            hit_effect.SetActive(false);
        }
    }

    public void attack_l_start_event(int val) {
        if (val >= 1) {
            // ���� ���ݽ� ���� �Ҹ� ���
            audio.transform.Find("shield_attack").GetComponent<AudioSource>().Play();

            coliderShield.enabled = true;//������ colider Ȱ��ȭ
            GameObject.Find("Knight").GetComponent<player_skill>().is_attack_motioning = true;
        }
    }

    // ���� L ���(���а���) ������ �̺�Ʈ
    public void attack_l_end_event(int val)
    {
        if (val >= 1)
        {
            // ���� �߿� ���� hit �� ���� ����Ʈ ����
            animator.SetBool("hit", false);

            animator.SetBool("attack_L", false);
            GameObject.Find("Knight").GetComponent<characterMove>().mouse_can_input = true;
            //���� ����� ��ȯ
            GameObject.Find("Knight").GetComponent<player_skill>().is_attack_motioning = false;
            //���� ����� �浹 1ȸ�� �����ϱ� ���� collision.is_first�� true�� ��ȯ
            //GameObject.Find("Shield").GetComponent<bisang_collision>().is_first = true;

            coliderShield.enabled = false;//������ colider ��Ȱ��ȭ
        }
    }

    public void attack_r_start_event(int val) {
        if( val >= 1)
        {
            // �� �ֵθ��� ���� ���
            audio.transform.Find("mouse_left").GetComponent<AudioSource>().Play();

            coliderWepon.enabled = true;//������ colider Ȱ��ȭ
            GameObject.Find("Knight").GetComponent<player_skill>().is_attack_motioning = true;
        }
    }

    //���� R ���(��Ÿ) ������ �̺�Ʈ
    public void attack_r_end_event(int val)
    {
        if (val >= 1)
        {
            // ���� �߿� ���� hit �� ���� ����Ʈ ����
            animator.SetBool("hit", false);

            animator.SetBool("attack_R", false);
            GameObject.Find("Knight").GetComponent<characterMove>().mouse_can_input = true;
            //���� ����� ��ȯ
            GameObject.Find("Knight").GetComponent<player_skill>().is_attack_motioning = false;
            //���� ����� �浹 1ȸ�� �����ϱ� ���� collision.is_first�� true�� ��ȯ
            //GameObject.Find("Sword").GetComponent<bisang_collision>().is_first = true;

            coliderWepon.enabled = false;//������ colider ��Ȱ��ȭ
        }
    }

    //Ulti_skill ��� �̺�Ʈ

    // ������ ����Ʈ
    public void ulti_skill_magic_event(int val)
    {
        if (val >= 1)
        {
            // ������ ����Ʈ �� ���� �ֵθ��� ������ �������� ����
            // GameObject.Find("Sword").GetComponent<bisang_collision>().is_first = false;
            magic_circle.transform.position = transform.position;
            magic_circle.SetActive(true);

            // ������ ����Ʈ �� ���� �ֵθ� �� ���� �Ҹ� ���
            audio.transform.Find("skill_R").GetComponent<AudioSource>().Play();
        }

    }

    // ���� ����Ʈ
    public void ulti_skill_ele_event(int val)
    {
        if (val >= 1)
        {
            coliderMagic.enabled = true;//������ colider Ȱ��ȭ
            // ���� �߻� �Ҹ� ���
            audio.transform.Find("skill_R_end").GetComponent<AudioSource>().Play();

            magic_circle.SetActive(false);
            element_arr.transform.position = transform.position;
            element_arr.transform.rotation = transform.rotation;
            element_arr.SetActive(true);
            GameObject.Find("Knight").GetComponent<player_skill>().is_attack_motioning = true;
        }

    }

    // ������
    public void ulti_skill_end_event(int val)
    {
        if (val >= 1)
        {
            // ���� �߿� ���� hit �� ���� ����Ʈ ����
            animator.SetBool("hit", false);

            //���� ����� �浹 1ȸ�� �����ϱ� ���� collision.is_first�� true�� ��ȯ
            //GameObject.Find("Light").GetComponent<bisang_collision>().is_first = true;

            element_arr.SetActive(false);
            GameObject.Find("Knight").GetComponent<characterMove>().mouse_can_input = true;
            animator.SetBool("ulti_skill", false);
            GameObject.Find("Knight").GetComponent<player_skill>().is_attack_motioning = false;

            coliderMagic.enabled = false;//������ colider ��Ȱ��ȭ

            //��ų �� ���Ƴ��� �� �浹�̺�Ʈ Ǯ��
            //GameObject.Find("Sword").GetComponent<bisang_collision>().is_first = true;
        }
    }


    // skill2 ����Ʈ
    public void skill2_start_event(int val)
    {
        if (val >= 1)
        {
            vortex.transform.position = gameObject.GetComponent<player_skill>().vortexPoint;
            vortex.SetActive(true);
            StartCoroutine(WaitMove());
        }
    }

    public IEnumerator WaitMove()
    {
        yield return new WaitForSeconds(4.0f);
        vortex.SetActive(false);
    }

    // ������
    public void skill2_end_event(int val)
    {
        if (val >= 1)
        {
            animator.SetBool("skill_2", false);
            GameObject.Find("Knight").GetComponent<characterMove>().mouse_can_input = true;
            GameObject.Find("Knight").GetComponent<player_skill>().is_attack_motioning = false;

            coliderVortex.enabled = false;
        }
    }


    // ������ �ϴ� ĳ���� ���� �صα�
    public void player_die_event(int val)
    {
        if (val >= 1)
        {
            gameObject.SetActive(false);
        }
    }

    //������ �̺�Ʈ
    public void levelup_event() {
        //��ġ ����
        Debug.Log(levelupEffect);
        Debug.Log(gameObject);
        levelupEffect.transform.position = gameObject.transform.position;
        levelupEffect.SetActive(true);
        Debug.Log("������ �߻�");

        Invoke("levelup_end_event", 2.0f);
    }

    public void levelup_end_event() {
        levelupEffect.SetActive(false);
    }
}
