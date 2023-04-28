using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal_status : MonoBehaviour
{
    public Transform portal_transform;//포탈 위치
    public string room_name;           //포탈이 위치한 방 이름

    public portal_status(Transform _portal_transform, string _room_name) {
        this.portal_transform = _portal_transform;
        this.room_name = _room_name;
    }

}
