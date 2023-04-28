using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{
    // 모든 room을 저장할 최상위 room 객체
    public Transform room_obj;
    // 모든 room을 각각 저장하고 있을 list
    public List<GameObject> room_arr;
    // 모든 포탈을 저장하고 있는 최상위 객체
    public GameObject potal_obj;

    // 각 room을 이어줄 포탈을 한 쌍으로 묶어서 저장하기 위한 Dictionary
    // 들어간 포탈 이름 -> 나올 포탈의 transform 형식으로 저장
    public Dictionary<string, Transform> potal_dic;

    // 캐릭터를 포탈로 이동시킬 때 함께 보낼 카메라
    public Camera camera;

    private void Awake()
    {
        // 저장 공간 할당
        room_arr = new List<GameObject>();
        potal_dic = new Dictionary<string, Transform>(); //내 포탈 이름, 연결될 포탈 위치

        // 각각의 room을 list에 저장
        for(int i = 0; i < room_obj.childCount; i++)
        {
            room_arr.Add(room_obj.GetChild(i).gameObject);
        }

        // 포탈 쌍 추가
        for(int i = 0; i < potal_obj.transform.childCount; i++)
        {
            Transform temp = potal_obj.transform.GetChild(i);
            // 이름 , tramsform으로 주기위해
            // potal_saver 밑에 번호로 한 쌍씩 추가
            // 하나의 번호 아래에는 반드시 2개의 포탈만 들어가 있어야함
            // 아닐시 에러
            potal_dic.Add(temp.GetChild(0).name, temp.GetChild(1).transform);
            potal_dic.Add(temp.GetChild(1).name, temp.GetChild(0).transform);
        }
    }

    public void move_room(GameObject user, string name)
    {
        Transform temp = potal_dic[name];
        // 바로 같은곳으로 이동하면 위치가 겹쳐서 무한히 룸을 이동하게 됨
        Vector3 pos = temp.position;
        //pos.x += 2f;
        user.transform.position = pos;        

        // 카메라도 따라 이동하게 해야함
        pos.x = user.transform.position.x;
        pos.y = camera.transform.position.y;
        pos.z = user.transform.position.z + 10f;

        camera.transform.position = pos;

        // 이동이 완료되면 캐릭터의 움직임을 멈춤
        user.GetComponent<Animator>().SetBool("running", false);
        user.GetComponent<characterMove>().init_pos_zero();

        StartCoroutine("wait_3_second", name);
    }

    IEnumerator  wait_3_second(string name)
    {
        yield return new WaitForSeconds(3);
        potal_dic[name].gameObject.GetComponent<move_room_collision>().can_use = false;
    }
}
