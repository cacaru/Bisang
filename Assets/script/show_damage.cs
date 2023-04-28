using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class show_damage : MonoBehaviour
{
    private Transform cam;
    private Vector3 pos;    //데미지가 출력될 위치

    public GameObject damage_text;
    private void Start()
    {
        cam = Camera.main.transform;
    }
    public void TakeDamage(int damage)
    {
        pos = gameObject.transform.position;
        switch (gameObject.GetComponent<status>().name)
        {
            // 캐릭터
            case "Warrior":
                pos.y += 2f;
                break;

            // 몹
            case "Slime":
                pos.y += 1.3f;
                break;
            case "Skeleton":
                pos.y += 2.3f;
                break;
            case "GroundDragon":
                pos.y += 5f;
                break;
        }

        GameObject hudText = Instantiate(damage_text, pos, gameObject.transform.rotation); // 생성할 텍스트 오브젝트

        // 클론이 생성될 위치
        if (gameObject.CompareTag("enemy"))
            hudText.transform.SetParent(gameObject.transform.Find("enemy_hpbar"));
        else if (gameObject.CompareTag("Player"))
            hudText.transform.SetParent(gameObject.transform.Find("player_damaged"));

        // 데미지폰트가 항상 카메라 방향을 바라고보고 있게 하기
        hudText.transform.LookAt(pos + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);

        hudText.GetComponent<damage_text>().damage = damage; // 데미지 전달
        hudText.SetActive(true);
    }
}