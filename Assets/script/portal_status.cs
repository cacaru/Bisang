using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal_status : MonoBehaviour
{
    public Transform portal_transform;//��Ż ��ġ
    public string room_name;           //��Ż�� ��ġ�� �� �̸�

    public portal_status(Transform _portal_transform, string _room_name) {
        this.portal_transform = _portal_transform;
        this.room_name = _room_name;
    }

}
