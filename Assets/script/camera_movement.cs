using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class camera_movement : MonoBehaviour
{
    // 카메라가 캐릭터를 따라다니게 하는 스크립트
    // 캐릭터 받기
    public GameObject character;
    public float moveSpeed = 6.3f;
    public float camera_y = 7.8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move_pos = character.transform.position;
        
        //카메라와 캐릭터간 거리를 위한 계산 - 임의 조정
        move_pos.y = camera_y;
        move_pos.z += 10f;
        
        // 카메라 위치 이동( 회전 x )
        transform.position = Vector3.MoveTowards(transform.position, move_pos, moveSpeed * Time.deltaTime);
    }
}
