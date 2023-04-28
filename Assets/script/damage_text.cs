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
        // 기본 값 설정
        moveSpeed = 3f; //위로 움직이는 속도값
        destroyTime = 3f; //몇초 후 삭제 될건지
        alphaSpeed = 2.0f; //서서히 사라지는 속도

        //텍스트 받기
        text = gameObject.GetComponent<Text>();
        color = text.color;
        text.text = damage.ToString();
        Invoke("DestroyObject", destroyTime);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // 텍스트 위치

        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
        text.color = color;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
