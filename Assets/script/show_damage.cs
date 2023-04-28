using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class show_damage : MonoBehaviour
{
    private Transform cam;
    private Vector3 pos;    //�������� ��µ� ��ġ

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
            // ĳ����
            case "Warrior":
                pos.y += 2f;
                break;

            // ��
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

        GameObject hudText = Instantiate(damage_text, pos, gameObject.transform.rotation); // ������ �ؽ�Ʈ ������Ʈ

        // Ŭ���� ������ ��ġ
        if (gameObject.CompareTag("enemy"))
            hudText.transform.SetParent(gameObject.transform.Find("enemy_hpbar"));
        else if (gameObject.CompareTag("Player"))
            hudText.transform.SetParent(gameObject.transform.Find("player_damaged"));

        // ��������Ʈ�� �׻� ī�޶� ������ �ٶ���� �ְ� �ϱ�
        hudText.transform.LookAt(pos + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);

        hudText.GetComponent<damage_text>().damage = damage; // ������ ����
        hudText.SetActive(true);
    }
}