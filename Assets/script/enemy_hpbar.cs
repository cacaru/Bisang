using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy_hpbar : MonoBehaviour
{
    private Transform cam;
    private Slider hpbar;

    // �� ���� �޾ƿ���
    private GameObject enemy;
    private status enemy_status;
    private Vector3 pos;
    private float upper_y_pos;
    // Start is called before the first frame update
    void Start()
    {
        // ī�޶��� ��ġ�� �޾� hpbar�� ī�޶� �������� �׻� ���̰� �ϱ� ���� ���� �ޱ�
        cam = Camera.main.transform;

        //�� ���� �б�
        enemy = gameObject.transform.parent.parent.gameObject;
        enemy_status = enemy.GetComponent<status>();

        //�� ���� �ʱ�ȭ
        enemy_status.InitStatus();

        //�� ������ �����ϴ� �����̴� �б�
        hpbar = gameObject.GetComponent<Slider>();

        // start �ܿ��� initstatus�� ������� �ʴ� ������ ���̱� ������ ������Ʈ�� �̸����� �ӽ÷� ���
        // -> ���Ƿ� �ʱ�ȭ ���Ѽ� �ۼ��� ��
        switch (enemy_status.name)
        {
            case "Slime":
                upper_y_pos = 1.5f;
                break;
            case "Skeleton":
                upper_y_pos = 2.3f;
                break;
            case "GroundDragon":
                upper_y_pos = 3.5f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // hpbar ����
        hpbar.maxValue = enemy_status.maxHP;
        hpbar.value = enemy_status.curHP;

        // hpbar�� �׻� ī�޶� ������ �ٶ���� �ְ� �ϱ�
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        // hpbar�� �׻� ���� ���� ����ٴϰ� �ϱ�
        move_pos();
    }

    private void move_pos()
    {
        pos = enemy.transform.position;
        pos.y += upper_y_pos;
        hpbar.transform.position = pos;
    }
}
