using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class damage_text : MonoBehaviour
{
    private float moveSpeed;
    private float destroyTime;
    private float alphaSpeed;

    private Text text;
    private Color color;

    public int damage;

    void Start()
    {
        // �⺻ �� ����
        moveSpeed = 3f; //���� �����̴� �ӵ���
        destroyTime = 3f; //���� �� ���� �ɰ���
        alphaSpeed = 2.0f; //������ ������� �ӵ�

        //�ؽ�Ʈ �ޱ�
        text = gameObject.GetComponent<Text>();
        color = text.color;
        text.text = damage.ToString();
        Invoke("DestroyObject", destroyTime);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // �ؽ�Ʈ ��ġ

        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * alphaSpeed); // �ؽ�Ʈ ���İ�
        text.color = color;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
