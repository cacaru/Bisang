using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameisPaused = false;
    public GameObject pauseMenuCanvas;
    public GameObject clear_UI;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameisPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume() {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
        //���콺 �Է� ����
        GameObject.Find("Knight").GetComponent<characterMove>().mouse_can_input = true;
    }

    public void Pause() {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
        GameObject.Find("Knight").GetComponent<characterMove>().mouse_can_input = false;
    }

    public void ToMain() {
        //����ȭ������ �̵�
        SceneManager.LoadScene("stage_select");
    }

    public void Quit() {
        // ����
        Application.Quit();
    }

    public void stage_clear()
    {
        Time.timeScale = 0f;
        //���콺 �Է� ����
        GameObject.Find("Knight").GetComponent<characterMove>().mouse_can_input = false;
        clear_UI.SetActive(true);
    }
}
