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

    public bool is_attack_motioning = false;    //현재 공격 모션중인지 감지

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
        #region 평타

        // 좌클릭 == 평타
        if (Input.GetMouseButton(0) && GameObject.Find("Knight").GetComponent<characterMove>().mouse_can_input)
        {
            // 현재 이동중이면 멈춰서 떄림
            animator.SetBool("running", false);
            // 이동을 정지시키기위해 movePos를 초기화 ( 현 좌표에서 움직임이 멈춤)
            gameObject.GetComponent<characterMove>().init_pos_zero();

            // 공격
            animator.SetBool("attack_R", true);
            //마우스 입력 강제 제어
            gameObject.GetComponent<characterMove>().mouse_can_input = false;
            //공격 모션중 전환
            is_attack_motioning = true;
            //공격 타입 지정
            gameObject.GetComponent<status>().attackType = 1;
        }

        #endregion

        #region 스킬

        // q :: 방패 올려치기
        // 사용 마나 : 20
        if (Input.GetKeyUp(KeyCode.Q))
        {
            // 마나 계산
            // 마나를 소비하기에 충분하면
            if (gameObject.GetComponent<status>().curMP >= 20 && cooldown_controller.skill2_cooldown_text.text == "")
            {
                gameObject.GetComponent<status>().curMP -= 20;
                //Debug.Log(gameObject.GetComponent<status>().curMP);
                //현재 이동중이면 멈춰서 때림
                animator.SetBool("running", false);
                gameObject.GetComponent<characterMove>().init_pos_zero();

                //공격
                animator.SetBool("attack_L", true);

                // 스킬 쿨타임 on
                cooldown_controller.setCoolTime(cooldown_controller.skill2_cooldown_text, 5);

                //마우스 입력 강제 제어
                gameObject.GetComponent<characterMove>().mouse_can_input = false;
                //공격 모션중 전환
                is_attack_motioning = true;
                //공격 타입 지정
                gameObject.GetComponent<status>().attackType = 2;
            }
                
            else
            {
                // 마나가 부족합니다 알림 메세지 출력 
                // 공격 중지
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
            //공격 타입 지정
            gameObject.GetComponent<status>().attackType = 4;

            // 이동중이면 이동 종료
            if (animator.GetBool("running"))
            {
                animator.SetBool("running", false);
                gameObject.GetComponent<characterMove>().init_pos_zero();
            }

        }


        //궁극기
        // 사용 마나 : 100
        if (Input.GetKeyUp(KeyCode.R))
        {
            //마나 계산
            if (gameObject.GetComponent<status>().curMP >= 100 && cooldown_controller.skill4_cooldown_text.text == "")
            {
                //마나 깎
                gameObject.GetComponent<status>().curMP -= 100;

                gameObject.GetComponent<characterMove>().mouse_can_input = false;
                animator.SetBool("ulti_skill", true);
                is_attack_motioning = true;

                // 스킬 쿨타임 on
                cooldown_controller.setCoolTime(cooldown_controller.skill4_cooldown_text, 20);

                //공격 타입 지정
                gameObject.GetComponent<status>().attackType = 3;

                // 이동중이면 이동 종료
                if (animator.GetBool("running"))
                {
                    animator.SetBool("running", false);
                    gameObject.GetComponent<characterMove>().init_pos_zero();
                }
            }
            else
            {
                // 마나가 부족합니다 알림 메세지 출력 
                // 공격 중지
                return;
            }
        }

        #endregion
    }
}
