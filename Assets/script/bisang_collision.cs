using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bisang_collision : MonoBehaviour
{
    // ��� �� �浹�� 1���� �߻���Ű���� �ϴ� ����
    // characterMove ��ũ��Ʈ����
    // �� ������ ù��° �浹���� false�� ��ȯ�� ����
    // �� ������ ������ �̺�Ʈ���� true�� ��ȯ����
    // == �� ���� ����� ����Ǵ� ���� ù �浹���� false ���� 
    //public bool is_first = true;
    public GameObject character;
    private Collider target;

    private bool is_enter_poison = false;

    private void OnTriggerEnter(Collider other)
    {
        this.target = other;
        switch (character.tag)
        {
            case "Player":
                player_trigger();
                break;
            case "enemy":
                enemy_trigger();
                break;
            case "debuff":
                // �ڷ�ƾ���� 1�� ��ٸ��� ���� target�� ���� ��� ���Ͽ� �������� ������ �Ұ��� �������Ƿ�
                // other�� �״�� �������� ��� ����
                debuff_trigger(other);
                break;
        }
    }

    // ĳ���� �з� Ʈ���� 
    private void player_trigger()
    {
        Debug.Log("ĳ���� ���� ����");
        // ĳ���Ͱ� ������ ���
        if (target.CompareTag("enemy") && character.GetComponent<player_skill>().is_attack_motioning)
        {
            //Debug.Log("��� �ν�");
            // ��밡 ���� �� hit ȿ���� ���
            GameObject.Find("audio").transform.Find("hit").GetComponent<AudioSource>().Play();

            this.target.GetComponent<Animator>().SetBool("hit", true);
            GameObject.Find("Gmanager").GetComponent<hit_calculator>().calc_status(character, this.target.GetComponent<status>());
        }

    }
   
    // ���� �з� Ʈ���� 
    private void enemy_trigger()
    {
        if (target.CompareTag("Player") && character.GetComponent<enemy_AI>().enemy_attacking && !this.target.GetComponent<status>().isPass)
        {
            this.target.GetComponent<Animator>().SetBool("hit", true);
            // ��밡 ���� �� hit ȿ���� ���
            GameObject.Find("audio").transform.Find("hit").GetComponent<AudioSource>().Play();
            GameObject.Find("Gmanager").GetComponent<hit_calculator>().calc_status(character, this.target.GetComponent<status>());
        }
    }

    //����� �з� Ʈ����
    private void debuff_trigger(Collider other)
    {
        // ������ �� ������ �´� ��� �߰� - ���� ������ 
        // ���� ���� ���� ��� 1�ʸ��� ���������� �������� �԰� �ۼ���
        if (target.CompareTag("Player") && target.gameObject.name == "Knight")
        {
            if (character.name == "Poison")
            {
                is_enter_poison = true;
                StartCoroutine("wait_time", other);
            }
        }
    }


    #region �� ������ �浹 ��� ����

    public void OnTriggerExit(Collider other)
    {
        if (gameObject.tag != "debuff") return;
        // ĳ���Ͱ� �� ������ ����� ������ false�� �����Ͽ� ������ ������ �ߴ�
        if (other.CompareTag("Player") && character.gameObject.name == "Poison")
        {
            is_enter_poison = false;
            StopCoroutine("wait_time");
        }

    }

    // �������� ������ 1�� �ں��� �������� �������� ���ϱ� ���� IEnumerator
    private IEnumerator wait_time(Collider other)
    {
        // �� ���� �ȿ� �ְ�, ���� ����Ʈ�� ���ӵǰ� ���� ��
        while (is_enter_poison && character.gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(1);
            other.GetComponent<Animator>().SetBool("hit", true);
            GameObject.Find("Gmanager").GetComponent<hit_calculator>().calc_status(character, other.GetComponent<status>() );
        }
    }
    #endregion
}
