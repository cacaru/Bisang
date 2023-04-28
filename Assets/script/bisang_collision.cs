using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bisang_collision : MonoBehaviour
{
    // 모션 중 충돌을 1번만 발생시키도록 하는 변수
    // characterMove 스크립트에서
    // 각 공격의 첫번째 충돌에서 false로 전환한 것을
    // 각 공격의 마지막 이벤트에서 true로 전환해줌
    // == 각 공격 모션이 실행되는 동안 첫 충돌이후 false 유지 
    //public bool is_first = true;
    public GameObject character;
    private Collider target;

    private bool is_enter_poison = false;

    private void OnTriggerEnter(Collider other)
    {
        this.target = other;
        switch (character.tag)
        {
            case "Player":
                player_trigger();
                break;
            case "enemy":
                enemy_trigger();
                break;
            case "debuff":
                // 코루틴으로 1초 기다리는 동안 target의 값이 계속 변하여 정상적인 연산이 불가능 해졌으므로
                // other를 그대로 가져가서 계산 진행
                debuff_trigger(other);
                break;
        }
    }

    // 캐릭터 분류 트리거 
    private void player_trigger()
    {
        Debug.Log("캐릭터 공격 판정");
        // 캐릭터가 때리는 경우
        if (target.CompareTag("enemy") && character.GetComponent<player_skill>().is_attack_motioning)
        {
            //Debug.Log("상대 인식");
            // 상대가 맞을 떄 hit 효과음 재생
            GameObject.Find("audio").transform.Find("hit").GetComponent<AudioSource>().Play();

            this.target.GetComponent<Animator>().SetBool("hit", true);
            GameObject.Find("Gmanager").GetComponent<hit_calculator>().calc_status(character, this.target.GetComponent<status>());
        }

    }
   
    // 몬스터 분류 트리거 
    private void enemy_trigger()
    {
        if (target.CompareTag("Player") && character.GetComponent<enemy_AI>().enemy_attacking && !this.target.GetComponent<status>().isPass)
        {
            this.target.GetComponent<Animator>().SetBool("hit", true);
            // 상대가 맞을 떄 hit 효과음 재생
            GameObject.Find("audio").transform.Find("hit").GetComponent<AudioSource>().Play();
            GameObject.Find("Gmanager").GetComponent<hit_calculator>().calc_status(character, this.target.GetComponent<status>());
        }
    }

    //디버프 분류 트리거
    private void debuff_trigger(Collider other)
    {
        // 지룡의 독 지형에 맞는 경우 추가 - 지형 데미지 
        // 지형 내에 있을 경우 1초마다 지속적으로 데미지를 입게 작성함
        if (target.CompareTag("Player") && target.gameObject.name == "Knight")
        {
            if (character.name == "Poison")
            {
                is_enter_poison = true;
                StartCoroutine("wait_time", other);
            }
        }
    }


    #region 독 데미지 충돌 계산 구역

    public void OnTriggerExit(Collider other)
    {
        if (gameObject.tag != "debuff") return;
        // 캐릭터가 독 지형을 벗어나면 변수를 false로 변경하여 데미지 입히기 중단
        if (other.CompareTag("Player") && character.gameObject.name == "Poison")
        {
            is_enter_poison = false;
            StopCoroutine("wait_time");
        }

    }

    // 독지형에 입장한 1초 뒤부터 지속적인 데미지를 가하기 위한 IEnumerator
    private IEnumerator wait_time(Collider other)
    {
        // 독 지형 안에 있고, 독의 이펙트가 지속되고 있을 때
        while (is_enter_poison && character.gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(1);
            other.GetComponent<Animator>().SetBool("hit", true);
            GameObject.Find("Gmanager").GetComponent<hit_calculator>().calc_status(character, other.GetComponent<status>() );
        }
    }
    #endregion
}
