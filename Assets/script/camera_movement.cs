using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class camera_movement : MonoBehaviour
{
    // ī�޶� ĳ���͸� ����ٴϰ� �ϴ� ��ũ��Ʈ
    // ĳ���� �ޱ�
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
        
        //ī�޶�� ĳ���Ͱ� �Ÿ��� ���� ��� - ���� ����
        move_pos.y = camera_y;
        move_pos.z += 10f;
        
        // ī�޶� ��ġ �̵�( ȸ�� x )
        transform.position = Vector3.MoveTowards(transform.position, move_pos, moveSpeed * Time.deltaTime);
    }
}
