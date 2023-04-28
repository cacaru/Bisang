using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class characterMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotateSpeed = 5.0f;
    public float stop_distance = 1.1f;
    public Animator animator;

    public bool mouse_can_input = true;

    private Vector3 movePos = Vector3.zero;
    private Vector3 moveDir = Vector3.zero;
    
    
    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("rolling", false);
        animator.SetBool("running", false);
    }

    // Update is called once per frame
    void Update()
    {
        #region 마우스 클릭에 따른 이동

        if (Input.GetMouseButtonDown(1) && mouse_can_input )
        {
            animator.SetBool("running", true);

            //ray cast 이용한 좌표 이동 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // RaycastHit raycastHit;
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                // 이동 지점
                movePos = raycastHit.point;
                movePos.y = -0.5f;
            }
            //Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);
        }

        if (movePos != Vector3.zero && mouse_can_input )
        {
            // 방향을 구한다. 
            Vector3 dir = movePos - transform.position;

            // 방향을 이용해 회전각을 구한다.
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

            // 회전 및 이동 
            transform.rotation = Quaternion.Euler(0, angle, 0);
            transform.position = Vector3.MoveTowards(transform.position, movePos, moveSpeed * Time.deltaTime);
        }
        // 현재위치와 목표위치 사이의 거리를 구한다.
        //float dis = Vector3.Distance(transform.position, movePos);
        float dis = (float)(Math.Truncate(Vector3.Distance(transform.position, movePos)*10) / 10);

        if ( dis<= 0.8f)
            animator.SetBool("running", false);

        // 목표지점 도달시 이동지점을 초기화해 추가적인 움직임을 제한한다. 
        if (dis <= 0.3f)
        {
            init_pos_zero();
        }

        #endregion

        #region 구르기

        if( Input.GetKeyDown(KeyCode.Space))
        {
            // 구르는 동안 무적 on
            gameObject.GetComponent<status>().passSet(true);

            // 구르는 동안 우 클릭(이동) 방지
            mouse_can_input = false;
            animator.SetBool("rolling", true);
            //animator.SetBool("running", false);
        }

        #endregion
    }

    // 구르는 모션 마지막 이벤트 받기
    public void dive_roll_end_event(int val)
    {
        if (val >= 1)
        {
            animator.SetBool("rolling", false);
            mouse_can_input = true;

            // 구르기가 끝나면 무적 off
            gameObject.GetComponent<status>().passSet(false);
        }
    }

    // move_pos 0으로 초기화
    public void init_pos_zero()
    {
        movePos = Vector3.zero;
    }

}