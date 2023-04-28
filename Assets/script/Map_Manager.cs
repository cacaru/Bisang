using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{
    // ��� room�� ������ �ֻ��� room ��ü
    public Transform room_obj;
    // ��� room�� ���� �����ϰ� ���� list
    public List<GameObject> room_arr;
    // ��� ��Ż�� �����ϰ� �ִ� �ֻ��� ��ü
    public GameObject potal_obj;

    // �� room�� �̾��� ��Ż�� �� ������ ��� �����ϱ� ���� Dictionary
    // �� ��Ż �̸� -> ���� ��Ż�� transform �������� ����
    public Dictionary<string, Transform> potal_dic;

    // ĳ���͸� ��Ż�� �̵���ų �� �Բ� ���� ī�޶�
    public Camera camera;

    private void Awake()
    {
        // ���� ���� �Ҵ�
        room_arr = new List<GameObject>();
        potal_dic = new Dictionary<string, Transform>(); //�� ��Ż �̸�, ����� ��Ż ��ġ

        // ������ room�� list�� ����
        for(int i = 0; i < room_obj.childCount; i++)
        {
            room_arr.Add(room_obj.GetChild(i).gameObject);
        }

        // ��Ż �� �߰�
        for(int i = 0; i < potal_obj.transform.childCount; i++)
        {
            Transform temp = potal_obj.transform.GetChild(i);
            // �̸� , tramsform���� �ֱ�����
            // potal_saver �ؿ� ��ȣ�� �� �־� �߰�
            // �ϳ��� ��ȣ �Ʒ����� �ݵ�� 2���� ��Ż�� �� �־����
            // �ƴҽ� ����
            potal_dic.Add(temp.GetChild(0).name, temp.GetChild(1).transform);
            potal_dic.Add(temp.GetChild(1).name, temp.GetChild(0).transform);
        }
    }

    public void move_room(GameObject user, string name)
    {
        Transform temp = potal_dic[name];
        // �ٷ� ���������� �̵��ϸ� ��ġ�� ���ļ� ������ ���� �̵��ϰ� ��
        Vector3 pos = temp.position;
        //pos.x += 2f;
        user.transform.position = pos;        

        // ī�޶� ���� �̵��ϰ� �ؾ���
        pos.x = user.transform.position.x;
        pos.y = camera.transform.position.y;
        pos.z = user.transform.position.z + 10f;

        camera.transform.position = pos;

        // �̵��� �Ϸ�Ǹ� ĳ������ �������� ����
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
