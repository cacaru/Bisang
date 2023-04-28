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

    // 사운드 저장 객체
    public GameObject audio;

    BoxCollider coliderWepon;
    private GameObject objWeapon;
    BoxCollider coliderShield;
    private GameObject objShield;
    CapsuleCollider coliderMagic;
    private GameObject objMagic;
    CapsuleCollider coliderVortex;
    private GameObject objVortex;

    public GameObject hit_effect;         // 플레이어 히트 이펙트 (넣어주지 않으면 찾지 못함)

    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.Find("Knight").GetComponent<characterMove>().animator;
        magic_circle = GameObject.Find("Knight").GetComponent<player_skill>().magic_circle;
        element_arr = GameObject.Find("Knight").GetComponent<player_skill>().element_arr;
        vortex = GameObject.Find("Knight").GetComponent<player_skill>().vortex;
        

        // 히트 판정 있는 오브젝트 선언
        objWeapon = GameObject.Find("Sword");
        coliderWepon = objWeapon.GetComponent<BoxCollider>();

        objShield = GameObject.Find("Shield");
        coliderShield = objShield.GetComponent<BoxCollider>();

        objMagic = GameObject.Find("Magic").transform.Find("ElementalArrow2").transform.Find("Light").gameObject;
        coliderMagic = objMagic.GetComponent<CapsuleCollider>();

        //objVortex = GameObject.Find("Magic").transform.Find("Vortex").transform.Find("Ring").gameObject;
        //coliderVortex = objVortex.GetComponent<CapsuleCollider>();

        coliderWepon.enabled = false;//무기의 colider 비활성화
        coliderShield.enabled = false;//방패의 colider 비활성화
        coliderMagic.enabled = false;//마법의 colider 비활성화
        //coliderVortex.enabled = false;//skill2의 colider 비활성화

        if (hit_effect)
            hit_effect.SetActive(false);
    }

    // 캐릭터 맞는 모션
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
            // 방패 공격시 나는 소리 재생
            audio.transform.Find("shield_attack").GetComponent<AudioSource>().Play();

            coliderShield.enabled = true;//방패의 colider 활성화
            GameObject.Find("Knight").GetComponent<player_skill>().is_attack_motioning = true;
        }
    }

    // 공격 L 모션(방패공격) 마지막 이벤트
    public void attack_l_end_event(int val)
    {
        if (val >= 1)
        {
            // 공격 중에 맞은 hit 에 따른 이펙트 끄기
            animator.SetBool("hit", false);

            animator.SetBool("attack_L", false);
            GameObject.Find("Knight").GetComponent<characterMove>().mouse_can_input = true;
            //공격 모션중 전환
            GameObject.Find("Knight").GetComponent<player_skill>().is_attack_motioning = false;
            //공격 모션의 충돌 1회만 감지하기 위해 collision.is_first를 true로 변환
            //GameObject.Find("Shield").GetComponent<bisang_collision>().is_first = true;

            coliderShield.enabled = false;//방패의 colider 비활성화
        }
    }

    public void attack_r_start_event(int val) {
        if( val >= 1)
        {
            // 검 휘두르는 사운드 재생
            audio.transform.Find("mouse_left").GetComponent<AudioSource>().Play();

            coliderWepon.enabled = true;//무기의 colider 활성화
            GameObject.Find("Knight").GetComponent<player_skill>().is_attack_motioning = true;
        }
    }

    //공격 R 모션(평타) 마지막 이벤트
    public void attack_r_end_event(int val)
    {
        if (val >= 1)
        {
            // 공격 중에 맞은 hit 에 따른 이펙트 끄기
            animator.SetBool("hit", false);

            animator.SetBool("attack_R", false);
            GameObject.Find("Knight").GetComponent<characterMove>().mouse_can_input = true;
            //공격 모션중 전환
            GameObject.Find("Knight").GetComponent<player_skill>().is_attack_motioning = false;
            //공격 모션의 충돌 1회만 감지하기 위해 collision.is_first를 true로 변환
            //GameObject.Find("Sword").GetComponent<bisang_collision>().is_first = true;

            coliderWepon.enabled = false;//무기의 colider 비활성화
        }
    }

    //Ulti_skill 모션 이벤트

    // 마법진 이펙트
    public void ulti_skill_magic_event(int val)
    {
        if (val >= 1)
        {
            // 마법진 이펙트 중 검을 휘두르는 동작을 막기위한 한줄
            // GameObject.Find("Sword").GetComponent<bisang_collision>().is_first = false;
            magic_circle.transform.position = transform.position;
            magic_circle.SetActive(true);

            // 마법진 이펙트 중 검을 휘두를 때 나는 소리 재생
            audio.transform.Find("skill_R").GetComponent<AudioSource>().Play();
        }

    }

    // 전방 이펙트
    public void ulti_skill_ele_event(int val)
    {
        if (val >= 1)
        {
            coliderMagic.enabled = true;//마법의 colider 활성화
            // 마법 발사 소리 재생
            audio.transform.Find("skill_R_end").GetComponent<AudioSource>().Play();

            magic_circle.SetActive(false);
            element_arr.transform.position = transform.position;
            element_arr.transform.rotation = transform.rotation;
            element_arr.SetActive(true);
            GameObject.Find("Knight").GetComponent<player_skill>().is_attack_motioning = true;
        }

    }

    // 마지막
    public void ulti_skill_end_event(int val)
    {
        if (val >= 1)
        {
            // 공격 중에 맞은 hit 에 따른 이펙트 끄기
            animator.SetBool("hit", false);

            //공격 모션의 충돌 1회만 감지하기 위해 collision.is_first를 true로 변환
            //GameObject.Find("Light").GetComponent<bisang_collision>().is_first = true;

            element_arr.SetActive(false);
            GameObject.Find("Knight").GetComponent<characterMove>().mouse_can_input = true;
            animator.SetBool("ulti_skill", false);
            GameObject.Find("Knight").GetComponent<player_skill>().is_attack_motioning = false;

            coliderMagic.enabled = false;//마법의 colider 비활성화

            //스킬 중 막아놓은 검 충돌이벤트 풀기
            //GameObject.Find("Sword").GetComponent<bisang_collision>().is_first = true;
        }
    }


    // skill2 이펙트
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

    // 마지막
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


    // 죽으면 일단 캐릭터 삭제 해두기
    public void player_die_event(int val)
    {
        if (val >= 1)
        {
            gameObject.SetActive(false);
        }
    }

    //레벨업 이벤트
    public void levelup_event() {
        //위치 조정
        Debug.Log(levelupEffect);
        Debug.Log(gameObject);
        levelupEffect.transform.position = gameObject.transform.position;
        levelupEffect.SetActive(true);
        Debug.Log("레이저 발사");

        Invoke("levelup_end_event", 2.0f);
    }

    public void levelup_end_event() {
        levelupEffect.SetActive(false);
    }
}
