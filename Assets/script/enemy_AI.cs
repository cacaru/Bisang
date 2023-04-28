using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_AI : MonoBehaviour
{
    public float attackDelay = 3f;         //공격 딜레이
    public bool enemy_attacking = false;   // 몬스터가 공격중임을 알리는 변수
    public GameObject hit_effect;         // 몬스터 히트 이펙트 (넣어주지 않으면 찾지 못함)

    // AI agent
    private NavMeshAgent path_finder;
    // 추적 대상 == 캐릭터
    private GameObject character;
    
    private Animator animator;

    Collider enemycollider;
    GameObject enemyhitbox;

    //지룡 스킬 이펙트
    private GameObject swamp;           // 포효시 나올 독지형

    private bool can_move;
    private bool can_attack;
    private int attack_type;

    private float pos; //추적대상과의 거리
    private float lastAttackTime; //마지막 공격 시점
    private float attackRange; // 공격 반경  -> awake에서 공격 컴포넌트를 부르며 같이 지정
    private int[] attack_rand = new int[10]; // 랜덤 공격 배열 -> 배열에 들어간 변수의 갯수 만큼의 확률 부여 
                               // ex { 1,1,1,2,2,3} :: 1 = 1/2 | 2 = 1/3 | 3 = 1/6 확률을 가짐
    
    public Transform tr;
    private bool hasTarget
    {
        get
        {
            //추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (character != null && !character.GetComponent<status>().isDead)
            {
                return true;
            }

            //그렇지 않다면 false
            return false;
        }
    }

    // 몬스터가 스폰되면 코드를 적용
    public void Awake()
    {
        //character = GameObject.Find("Knight");

        // 몬스터의 공격 collider를 공격하는 순간에만 껏다 키기 위한 조건문들
        // 기본 기능
        // 1.각 몹마다 공격 범위 수정
        // 2.각 몹의 collider를 찾아 등록 후 최초 false로 지정
        // 3.각 몹에 들어가 있는 enemycollider가 본인의 것일 수 있도록 gameObject로 시작

        // 슬라임일 경우에 boxcollider를 찾음
        if (gameObject.GetComponent<Slime_Status>() != null)
        {
            enemyhitbox = gameObject.transform.Find("Body").Find("Hit_Box").gameObject;
            enemycollider = enemyhitbox.GetComponent<BoxCollider>();

            //공격 범위 설정
            attackRange = 2.3f;
            //적의 공격 판정 끄기
            enemycollider.enabled = false;
        }
        // 스켈레톤의 경우 검의  boxcollider를 찾음
        else if (gameObject.GetComponent<Skeleton_status>() != null)
        {
            enemyhitbox = gameObject.transform.Find("Bip01").Find("Bip01 Pelvis").Find("Bip01 Spine").Find("Bip01 Spine1").Find("Bip01 Spine2").Find("Bip01 R Clavicle")
                                              .Find("Bip01 R UpperArm").Find("Bip01 R Forearm").Find("Bip01 R Hand").Find("Skeleton_sword").gameObject;
            enemycollider = enemyhitbox.GetComponent<BoxCollider>();

            //공격 범위 설정
            attackRange = 2.5f;
            //적의 공격판정을 미리 꺼둠
            enemycollider.enabled = false;
        }
        // 지룡의 경우 
        else if (gameObject.GetComponent<GroundDragon_status>() != null)
        {
            //돌진 할 때 머리의 hit_box 불러오기
            enemyhitbox = gameObject.transform.Find("Root").Find("Spine01").Find("Spine02").Find("Chest").Find("Neck01").Find("Neck02")
                                              .Find("Head").gameObject;
            enemycollider = enemyhitbox.GetComponent<BoxCollider>();
            //머리의 공격판정을 미리 꺼둠
            enemycollider.enabled = false;
            //포효할 때 나오게 될 독 지형 불러오기
            swamp = gameObject.transform.Find("Swamp").gameObject;
            //공격 범위 설정
            attackRange = 5f;
            //공격의 확률 설정
            //변수의 갯수 만큼의 확률 지정 - 1차원적인 코드이므로 추후 개선 사항 있으면 수정
            int[] dragon_attack_rand = {1,1,1,1,1, 2,2,2, 3,3 };
            attack_rand = dragon_attack_rand;
        }

        if (hit_effect)
            hit_effect.SetActive(false);

        animator = GetComponent<Animator>();
        path_finder = GetComponent<NavMeshAgent>();

        tr = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    { 
        StartCoroutine(UpdatePath());
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("can_move", can_move);
        animator.SetBool("can_attack", can_attack);
        if (gameObject.GetComponent<status>().name == "GroundDragon") {
            animator.SetInteger("attack_type", attack_type);
        }

        if (hasTarget)
        {
            //추적 대상이 존재할 경우 거리 계산은 실시간으로 해야하니 Update()
            pos = Vector3.Distance(tr.position, character.transform.position);
            //Debug.Log(pos);
        }
    }

    //추적할 대상의 위치를 주기적으로 찾아 경로 갱신
    private IEnumerator UpdatePath()
    {
        //살아 있는 동안 무한 루프
        while (!gameObject.GetComponent<status>().isDead)
        {
            if (hasTarget)
            {
                Attack();
            }
            else
            {
                //추적 대상이 없을 경우, AI 이동 정지
                path_finder.isStopped = true;
                can_attack = false;
                can_move = false;

                //반지름 20f의 콜라이더로 player 레이어를 가진 콜라이더 검출하기
                Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, 1 << 3);
                
                //모든 콜라이더를 순회하면서 살아 있는 LivingEntity 찾기
                for (int i = 0; i < colliders.Length; i++)
                {
                    //콜라이더로부터 status 가져오기
                    status character_tmp = colliders[i].GetComponent<status>();

                    //status가 존재하고 태그가 Player 이며 해당 status가 살아있다면
                    if (character_tmp != null && !character_tmp.isDead && character_tmp.CompareTag("Player") )
                    {
                        //Debug.Log(character_tmp);
                        //추적 대상을 해당 character로 지정
                        character = character_tmp.gameObject;

                        //for문 루프 즉시 정지
                        break;
                    }
                }
            }

            //0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    //추적 대상과의 거리에 따라 공격 실행
    public void Attack()
    {
        //자신이 사망X, 추적 대상과의 거리이 공격 사거리 안에 있다면
        if (!gameObject.GetComponent<status>().isDead && pos < attackRange)
        {
            //공격 반경 안에 있으면 움직임을 멈춘다.
            can_move = false;
            path_finder.isStopped = true;

            //추적 대상 바라보기
            if (!enemy_attacking)
                gameObject.transform.LookAt(character.transform);

            //최근 공격 시점에서 attackDelay 이상 시간이 지나면 공격 가능
            if (lastAttackTime + attackDelay <= Time.time)
            {
                can_attack = true;

                // 용은 특수 공격이 존재하기 때문에 예외처리
                if (gameObject.GetComponent<status>().name == "GroundDragon" )
                {
                    gameObject.GetComponent<status>().attackType = attack_rand[Random.Range(0, 10)];
                    attack_type = gameObject.GetComponent<status>().attackType;

                    lastAttackTime = Time.time;

                }
                else
                {    
                    //공격 타입 지정 
                    //1은 기본 공격
                    gameObject.GetComponent<status>().attackType = 1;
                }
            }
            
            else
            {
                can_attack = false;
            }
        }
        //공격 반경 밖에 있지만 공격 중일 경우 공격 모션만 지속
        else if (!gameObject.GetComponent<status>().isDead && enemy_attacking)
        {
            can_move = false;
            path_finder.isStopped = true;
        }
        //공격 반경 밖에 있고 공격 중이지 않을 경우 추적하기
        else
        {
            can_move = true;
            can_attack = false;
            //계속 추적
            path_finder.isStopped = false; //계속 이동
            path_finder.SetDestination(character.transform.position);
            // 추적 중 이므로 공격중 변수 false
            enemy_attacking = false;
        }
    }

    // ----------------------------------------------------------------------------
    // enemy 이벤트 핸들 부분
    // ----------------------------------------------------------------------------

    #region enemy 공용 애니메이션

    // val 로 넘기는 번호로 몬스터를 구분하여 해당 몬스터만 사용하는 변수 조작을 실행
    // 모든 번호는 0보다 크다
    // 각 번호는 추가 순으로 기입한다.
    // 1 : slime
    // 2 : skeleton
    // 3 : ground dragon 

    // hit effect animation의 경우 
    // 이펙트마다의 위치를 조절하고 타이밍을 잡는 부분이 다를 수도 있으므로 공용 함수에서 제외(임시)
    // 이후 타이밍과 위치가 동일해도 될것같다고 판단되면 통합 

    // 기본 공격 시작
    public void attack_start(int val)
    {
        if( val >= 1)
        {
            // hit effect
            enemy_attacking = true;
            //최근 공격 시간 갱신
            lastAttackTime = Time.time;

            if (val == 1 || val == 2 || val == 3) {
                enemycollider.enabled = true;
            }
        }
    }

    // 기본 공격 종료
    public void attack_end(int val) 
    {
        Debug.Log("공격 꺼짐");
        if (val >= 1)
        {
            enemy_attacking = false;

            if (val == 1 || val == 2 ) {
                enemycollider.enabled = false;
            }
            else if (val == 3) {
                enemycollider.enabled = false;
                animator.SetBool("hit", false);
            }
        }
    }

    // 자신이 맞는 모션 끄기
    public void hit_end_event(int val)
    {
        if( val >= 1)
        {
            animator.SetBool("hit", false);
        }
    }

    // 자신이 죽는 이벤트
    public void die_event(int val)
    {
        if ( val >= 1)
        {
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region 슬라임 전용 애니메이션


    public void slime_hit_effect(int val)
    {
        if ( val >= 1 )
        {
            Vector3 effect_pos;
            effect_pos.x = gameObject.transform.position.x + 0.1f;
            effect_pos.y = gameObject.transform.position.y + 0.3f;
            effect_pos.z = gameObject.transform.position.z + 0.1f;
            hit_effect.transform.position = effect_pos;
            hit_effect.SetActive(true);
        }
    }

    public void slime_hit_effect_end(int val)
    {
        if( val>=1 )
        {
            hit_effect.SetActive(false);
        }
    }

    #endregion

    #region 스켈레톤 전용 애니메이션

    public void skeleton_hit_effect(int val)
    {
        if (val >= 1)
        {
            //Vector3 effect_pos;
            //effect_pos.x = gameObject.transform.position.x + 0.1f;
            //effect_pos.y = gameObject.transform.position.y + 0.3f;
            //effect_pos.z = gameObject.transform.position.z + 0.1f;
            hit_effect.transform.position = gameObject.transform.position;
            hit_effect.SetActive(true);
        }
    }

    public void skeleton_hit_effect_end(int val)
    {
        if (val >= 1)
        {
            hit_effect.SetActive(false);
        }
    }

    #endregion

    #region 드래곤 전용 애니메이션

    public void dragon_hit_effect(int val)
    {
        if (val >= 1 )
        {
            Vector3 effect_pos = gameObject.transform.position;
            Vector3 effect_scale = new Vector3( 1, 1, 1 );
            hit_effect.transform.position = effect_pos;
            hit_effect.transform.localScale = effect_scale;
            
            hit_effect.SetActive(true);
        }
    }

    public void dragon_hit_effect_end(int val)
    {
        if (val >= 1)
        {
            Vector3 effect_scale = new Vector3(0.5f, 0.5f, 0.5f);
            hit_effect.transform.localScale = effect_scale;
            hit_effect.SetActive(false);
        }
    }

    // 포효 할 때 나올 독 지형 이펙트
    public void poison_area_start(int val)
    {
        if (val >= 1) 
        {
            //swamp.SetActive(true);
            GameObject clone_swamp = Instantiate(swamp, swamp.transform.position, swamp.transform.rotation);
            clone_swamp.transform.parent = GameObject.Find("Gmanager").transform;
            clone_swamp.SetActive(true);
        }
    }

    #endregion

}
