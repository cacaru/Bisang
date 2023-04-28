using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_room_collision : MonoBehaviour
{
    public bool can_use = false;
    private void OnTriggerEnter(Collider other)
    {
        // �浹 ���� "��"(��Ż)�� clear ��Ż�� ��
        if (gameObject.name == "clear" && other.CompareTag("Player") )
        {
            GameObject.Find("Canvas").GetComponent<PauseMenu>().stage_clear();
        }
        // �浹�� Ÿ���� �÷��̾��� ���� �۵��ؾ��� --> �����϶� �۵��ϸ� �ȵ�
        if (other.CompareTag("Player") && !can_use)
        {
            can_use = true;
            GameObject.Find("Gmanager").GetComponent<Map_Manager>().move_room(other.gameObject, gameObject.name);
        }
    }
}
