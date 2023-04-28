using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_room_collision : MonoBehaviour
{
    public bool can_use = false;
    private void OnTriggerEnter(Collider other)
    {
        // 충돌 당한 "나"(포탈)가 clear 포탈일 때
        if (gameObject.name == "clear" && other.CompareTag("Player") )
        {
            GameObject.Find("Canvas").GetComponent<PauseMenu>().stage_clear();
        }
        // 충돌한 타겟이 플레이어일 때만 작동해야함 --> 몬스터일때 작동하면 안됨
        if (other.CompareTag("Player") && !can_use)
        {
            can_use = true;
            GameObject.Find("Gmanager").GetComponent<Map_Manager>().move_room(other.gameObject, gameObject.name);
        }
    }
}
