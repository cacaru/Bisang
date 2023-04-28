using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_status : MonoBehaviour
{
    public bool is_clear = false;
    public int enemy_cnt;
    public List<GameObject> portals;

    private void Awake()
    {
        enemy_counter();
        search_portal();
    }

    private void enemy_counter()
    {
        enemy_cnt = GameObject.Find("enemy").transform.Find(gameObject.name).childCount;
    }

    public void check_clear() {
        enemy_cnt--;
        if (enemy_cnt == 0) {
            is_clear = true;
            foreach (var item in portals)
            {
                item.SetActive(true);
            }
        }
        
    }

    private void search_portal() {
        //반지름 20f의 콜라이더로 player 레이어를 가진 콜라이더 검출하기
        Collider[] colliders = Physics.OverlapSphere(transform.position, 100f, 1 << 6);
        for (int i = 0; i < colliders.Length; i++) {
            portals.Add(colliders[i].gameObject);
            colliders[i].gameObject.SetActive(false);
        }
    }


}
