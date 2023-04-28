using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_AI : MonoBehaviour
{
    public float attackDelay = 3f;         //���� ������
    public bool enemy_attacking = false;   // ���Ͱ� ���������� �˸��� ����
    public GameObject hit_effect;         // ���� ��Ʈ ����Ʈ (�־����� ������ ã�� ����)

    // AI agent
    private NavMeshAgent path_finder;
    // ���� ��� == ĳ����
    private GameObject character;
    
    private Animator animator;

    Collider enemycollider;
    GameObject enemyhitbox;

    //���� ��ų ����Ʈ
    private GameObject swamp;           // ��ȿ�� ���� ������

    private bool can_move;
    private bool can_attack;
    private int attack_type;

    private float pos; //���������� �Ÿ�
    private float lastAttackTime; //������ ���� ����
    private float attackRange; // ���� �ݰ�  -> awake���� ���� ������Ʈ�� �θ��� ���� ����
    private int[] attack_rand = new int[10]; // ���� ���� �迭 -> �迭�� �� ������ ���� ��ŭ�� Ȯ�� �ο� 
                               // ex { 1,1,1,2,2,3} :: 1 = 1/2 | 2 = 1/3 | 3 = 1/6 Ȯ���� ����
    
    public Transform tr;
    private bool hasTarget
    {
        get
        {
            //������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true
            if (character != null && !character.GetComponent<status>().isDead)
            {
                return true;
            }

            //�׷��� �ʴٸ� false
            return false;
        }
    }

    // ���Ͱ� �����Ǹ� �ڵ带 ����
    public void Awake()
    {
        //character = GameObject.Find("Knight");

        // ������ ���� collider�� �����ϴ� �������� ���� Ű�� ���� ���ǹ���
        // �⺻ ���
        // 1.�� ������ ���� ���� ����
        // 2.�� ���� collider�� ã�� ��� �� ���� false�� ����
        // 3.�� ���� �� �ִ� enemycollider�� ������ ���� �� �ֵ��� gameObject�� ����

        // �������� ��쿡 boxcollider�� ã��
        if (gameObject.GetComponent<Slime_Status>() != null)
        {
            enemyhitbox = gameObject.transform.Find("Body").Find("Hit_Box").gameObject;
            enemycollider = enemyhitbox.GetComponent<BoxCollider>();

            //���� ���� ����
            attackRange = 2.3f;
            //���� ���� ���� ����
            enemycollider.enabled = false;
        }
        // ���̷����� ��� ����  boxcollider�� ã��
        else if (gameObject.GetComponent<Skeleton_status>() != null)
        {
            enemyhitbox = gameObject.transform.Find("Bip01").Find("Bip01 Pelvis").Find("Bip01 Spine").Find("Bip01 Spine1").Find("Bip01 Spine2").Find("Bip01 R Clavicle")
                                              .Find("Bip01 R UpperArm").Find("Bip01 R Forearm").Find("Bip01 R Hand").Find("Skeleton_sword").gameObject;
            enemycollider = enemyhitbox.GetComponent<BoxCollider>();

            //���� ���� ����
            attackRange = 2.5f;
            //���� ���������� �̸� ����
            enemycollider.enabled = false;
        }
        // ������ ��� 
        else if (gameObject.GetComponent<GroundDragon_status>() != null)
        {
            //���� �� �� �Ӹ��� hit_box �ҷ�����
            enemyhitbox = gameObject.transform.Find("Root").Find("Spine01").Find("Spine02").Find("Chest").Find("Neck01").Find("Neck02")
                                              .Find("Head").gameObject;
            enemycollider = enemyhitbox.GetComponent<BoxCollider>();
            //�Ӹ��� ���������� �̸� ����
            enemycollider.enabled = false;
            //��ȿ�� �� ������ �� �� ���� �ҷ�����
            swamp = gameObject.transform.Find("Swamp").gameObject;
            //���� ���� ����
            attackRange = 5f;
            //������ Ȯ�� ����
            //������ ���� ��ŭ�� Ȯ�� ���� - 1�������� �ڵ��̹Ƿ� ���� ���� ���� ������ ����
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
            //���� ����� ������ ��� �Ÿ� ����� �ǽð����� �ؾ��ϴ� Update()
            pos = Vector3.Distance(tr.position, character.transform.position);
            //Debug.Log(pos);
        }
    }

    //������ ����� ��ġ�� �ֱ������� ã�� ��� ����
    private IEnumerator UpdatePath()
    {
        //��� �ִ� ���� ���� ����
        while (!gameObject.GetComponent<status>().isDead)
        {
            if (hasTarget)
            {
                Attack();
            }
            else
            {
                //���� ����� ���� ���, AI �̵� ����
                path_finder.isStopped = true;
                can_attack = false;
                can_move = false;

                //������ 20f�� �ݶ��̴��� player ���̾ ���� �ݶ��̴� �����ϱ�
                Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, 1 << 3);
                
                //��� �ݶ��̴��� ��ȸ�ϸ鼭 ��� �ִ� LivingEntity ã��
                for (int i = 0; i < colliders.Length; i++)
                {
                    //�ݶ��̴��κ��� status ��������
                    status character_tmp = colliders[i].GetComponent<status>();

                    //status�� �����ϰ� �±װ� Player �̸� �ش� status�� ����ִٸ�
                    if (character_tmp != null && !character_tmp.isDead && character_tmp.CompareTag("Player") )
                    {
                        //Debug.Log(character_tmp);
                        //���� ����� �ش� character�� ����
                        character = character_tmp.gameObject;

                        //for�� ���� ��� ����
                        break;
                    }
                }
            }

            //0.25�� �ֱ�� ó�� �ݺ�
            yield return new WaitForSeconds(0.25f);
        }
    }

    //���� ������ �Ÿ��� ���� ���� ����
    public void Attack()
    {
        //�ڽ��� ���X, ���� ������ �Ÿ��� ���� ��Ÿ� �ȿ� �ִٸ�
        if (!gameObject.GetComponent<status>().isDead && pos < attackRange)
        {
            //���� �ݰ� �ȿ� ������ �������� �����.
            can_move = false;
            path_finder.isStopped = true;

            //���� ��� �ٶ󺸱�
            if (!enemy_attacking)
                gameObject.transform.LookAt(character.transform);

            //�ֱ� ���� �������� attackDelay �̻� �ð��� ������ ���� ����
            if (lastAttackTime + attackDelay <= Time.time)
            {
                can_attack = true;

                // ���� Ư�� ������ �����ϱ� ������ ����ó��
                if (gameObject.GetComponent<status>().name == "GroundDragon" )
                {
                    gameObject.GetComponent<status>().attackType = attack_rand[Random.Range(0, 10)];
                    attack_type = gameObject.GetComponent<status>().attackType;

                    lastAttackTime = Time.time;

                }
                else
                {    
                    //���� Ÿ�� ���� 
                    //1�� �⺻ ����
                    gameObject.GetComponent<status>().attackType = 1;
                }
            }
            
            else
            {
                can_attack = false;
            }
        }
        //���� �ݰ� �ۿ� ������ ���� ���� ��� ���� ��Ǹ� ����
        else if (!gameObject.GetComponent<status>().isDead && enemy_attacking)
        {
            can_move = false;
            path_finder.isStopped = true;
        }
        //���� �ݰ� �ۿ� �ְ� ���� ������ ���� ��� �����ϱ�
        else
        {
            can_move = true;
            can_attack = false;
            //��� ����
            path_finder.isStopped = false; //��� �̵�
            path_finder.SetDestination(character.transform.position);
            // ���� �� �̹Ƿ� ������ ���� false
            enemy_attacking = false;
        }
    }

    // ----------------------------------------------------------------------------
    // enemy �̺�Ʈ �ڵ� �κ�
    // ----------------------------------------------------------------------------

    #region enemy ���� �ִϸ��̼�

    // val �� �ѱ�� ��ȣ�� ���͸� �����Ͽ� �ش� ���͸� ����ϴ� ���� ������ ����
    // ��� ��ȣ�� 0���� ũ��
    // �� ��ȣ�� �߰� ������ �����Ѵ�.
    // 1 : slime
    // 2 : skeleton
    // 3 : ground dragon 

    // hit effect animation�� ��� 
    // ����Ʈ������ ��ġ�� �����ϰ� Ÿ�̹��� ��� �κ��� �ٸ� ���� �����Ƿ� ���� �Լ����� ����(�ӽ�)
    // ���� Ÿ�ְ̹� ��ġ�� �����ص� �ɰͰ��ٰ� �ǴܵǸ� ���� 

    // �⺻ ���� ����
    public void attack_start(int val)
    {
        if( val >= 1)
        {
            // hit effect
            enemy_attacking = true;
            //�ֱ� ���� �ð� ����
            lastAttackTime = Time.time;

            if (val == 1 || val == 2 || val == 3) {
                enemycollider.enabled = true;
            }
        }
    }

    // �⺻ ���� ����
    public void attack_end(int val) 
    {
        Debug.Log("���� ����");
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

    // �ڽ��� �´� ��� ����
    public void hit_end_event(int val)
    {
        if( val >= 1)
        {
            animator.SetBool("hit", false);
        }
    }

    // �ڽ��� �״� �̺�Ʈ
    public void die_event(int val)
    {
        if ( val >= 1)
        {
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region ������ ���� �ִϸ��̼�


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

    #region ���̷��� ���� �ִϸ��̼�

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

    #region �巡�� ���� �ִϸ��̼�

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

    // ��ȿ �� �� ���� �� ���� ����Ʈ
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
