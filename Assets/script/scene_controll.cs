using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scene_controll : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // �������� ���ù�ư���� �̵�
    public void start_action() 
    {
        SceneManager.LoadScene("stage_select");
    }

    // ���� �Ұ��� �̵�
    public void introduce_action()
    {

    }

    // ���� ����
    public void exit_action()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ����ȭ������ �̵�
    public void move_start_scene()
    {
        SceneManager.LoadScene("start_scene");
    }

    // �� ������ �̵�
    public void move_stage(string name)
    {
        switch (name)
        {
            case "stage1":
                SceneManager.LoadScene("stage1");
                break;
        }
    }

    // ���� ���������� �̵�
    public void move_next_stage(string name)
    {
        switch (name)
        {
            case "stage1":
                //SceneManager.LoadScene("stage2");
                break;
        }
    }
}
