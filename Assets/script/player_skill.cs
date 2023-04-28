using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_skill : MonoBehaviour
{
    public GameObject magic_circle;
    public GameObject element_arr;
    public GameObject vortex;

    private player_cooldown cooldown_controller;

    public Vector3 vortexPoint = new Vector3(0f, 0.1f, 0f);

    public bool is_attack_motioning = false;    //���� ���� ��������� ����

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.Find("Knight").GetComponent<characterMove>().animator;

        cooldown_controller = gameObject.GetComponent<player_cooldown>();
    }

    // Update is called once per frame
    void Update()
    {
        #region ��Ÿ

        // ��Ŭ�� == ��Ÿ
        if (Input.GetMouseButton(0) && GameObject.Find("Knight").GetComponent<characterMove>().mouse_can_input)
        {
            // ���� �̵����̸� ���缭 ����
            animator.SetBool("running", false);
            // �̵��� ������Ű������ movePos�� �ʱ�ȭ ( �� ��ǥ���� �������� ����)
            gameObject.GetComponent<characterMove>().init_pos_zero();

            // ����
            animator.SetBool("attack_R", true);
            //���콺 �Է� ���� ����
            gameObject.GetComponent<characterMove>().mouse_can_input = false;
            //���� ����� ��ȯ
            is_attack_motioning = true;
            //���� Ÿ�� ����
            gameObject.GetComponent<status>().attackType = 1;
        }

        #endregion

        #region ��ų

        // q :: ���� �÷�ġ��
        // ��� ���� : 20
        if (Input.GetKeyUp(KeyCode.Q))
        {
            // ���� ���
            // ������ �Һ��ϱ⿡ ����ϸ�
            if (gameObject.GetComponent<status>().curMP >= 20 && cooldown_controller.skill2_cooldown_text.text == "")
            {
                gameObject.GetComponent<status>().curMP -= 20;
                //Debug.Log(gameObject.GetComponent<status>().curMP);
                //���� �̵����̸� ���缭 ����
                animator.SetBool("running", false);
                gameObject.GetComponent<characterMove>().init_pos_zero();

                //����
                animator.SetBool("attack_L", true);

                // ��ų ��Ÿ�� on
                cooldown_controller.setCoolTime(cooldown_controller.skill2_cooldown_text, 5);

                //���콺 �Է� ���� ����
                gameObject.GetComponent<characterMove>().mouse_can_input = false;
                //���� ����� ��ȯ
                is_attack_motioning = true;
                //���� Ÿ�� ����
                gameObject.GetComponent<status>().attackType = 2;
            }
                
            else
            {
                // ������ �����մϴ� �˸� �޼��� ��� 
                // ���� ����
                return;
            }

            
        }

        // skill3
        if (Input.GetKeyUp(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                this.vortexPoint.x = raycastHit.point.x;
                this.vortexPoint.z = raycastHit.point.z;
            }

            gameObject.GetComponent<characterMove>().mouse_can_input = false;
            animator.SetBool("skill_2", true);
            is_attack_motioning = true;
            //���� Ÿ�� ����
            gameObject.GetComponent<status>().attackType = 4;

            // �̵����̸� �̵� ����
            if (animator.GetBool("running"))
            {
                animator.SetBool("running", false);
                gameObject.GetComponent<characterMove>().init_pos_zero();
            }

        }


        //�ñر�
        // ��� ���� : 100
        if (Input.GetKeyUp(KeyCode.R))
        {
            //���� ���
            if (gameObject.GetComponent<status>().curMP >= 100 && cooldown_controller.skill4_cooldown_text.text == "")
            {
                //���� ��
                gameObject.GetComponent<status>().curMP -= 100;

                gameObject.GetComponent<characterMove>().mouse_can_input = false;
                animator.SetBool("ulti_skill", true);
                is_attack_motioning = true;

                // ��ų ��Ÿ�� on
                cooldown_controller.setCoolTime(cooldown_controller.skill4_cooldown_text, 20);

                //���� Ÿ�� ����
                gameObject.GetComponent<status>().attackType = 3;

                // �̵����̸� �̵� ����
                if (animator.GetBool("running"))
                {
                    animator.SetBool("running", false);
                    gameObject.GetComponent<characterMove>().init_pos_zero();
                }
            }
            else
            {
                // ������ �����մϴ� �˸� �޼��� ��� 
                // ���� ����
                return;
            }
        }

        #endregion
    }
}
