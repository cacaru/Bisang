using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy_hpbar : MonoBehaviour
{
    private Transform cam;
    private Slider hpbar;

    // 적 정보 받아오기
    private GameObject enemy;
    private status enemy_status;
    private Vector3 pos;
    private float upper_y_pos;
    // Start is called before the first frame update
    void Start()
    {
        // 카메라의 위치를 받아 hpbar가 카메라 방향으로 항상 보이게 하기 위한 변수 받기
        cam = Camera.main.transform;

        //몹 정보 읽기
        enemy = gameObject.transform.parent.parent.gameObject;
        enemy_status = enemy.GetComponent<status>();

        //몹 정보 초기화
        enemy_status.InitStatus();

        //몹 하위에 존재하는 슬라이더 읽기
        hpbar = gameObject.GetComponent<Slider>();

        // start 단에서 initstatus가 실행되지 않는 것으로 보이기 때문에 오브젝트의 이름으로 임시로 사용
        // -> 임의로 초기화 시켜서 작성함 ㅎ
        switch (enemy_status.name)
        {
            case "Slime":
                upper_y_pos = 1.5f;
                break;
            case "Skeleton":
                upper_y_pos = 2.3f;
                break;
            case "GroundDragon":
                upper_y_pos = 3.5f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // hpbar 설정
        hpbar.maxValue = enemy_status.maxHP;
        hpbar.value = enemy_status.curHP;

        // hpbar가 항상 카메라 방향을 바라고보고 있게 하기
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        // hpbar가 항상 몹의 위에 따라다니게 하기
        move_pos();
    }

    private void move_pos()
    {
        pos = enemy.transform.position;
        pos.y += upper_y_pos;
        hpbar.transform.position = pos;
    }
}
