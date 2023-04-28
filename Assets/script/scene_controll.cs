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
    // 스테이지 선택버튼으로 이동
    public void start_action() 
    {
        SceneManager.LoadScene("stage_select");
    }

    // 게임 소개로 이동
    public void introduce_action()
    {

    }

    // 게임 종료
    public void exit_action()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 시작화면으로 이동
    public void move_start_scene()
    {
        SceneManager.LoadScene("start_scene");
    }

    // 각 층으로 이동
    public void move_stage(string name)
    {
        switch (name)
        {
            case "stage1":
                SceneManager.LoadScene("stage1");
                break;
        }
    }

    // 다음 스테이지로 이동
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
