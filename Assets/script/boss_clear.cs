using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_clear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeInHierarchy)
        {
            Time.timeScale = 0f;
            GameObject.Find("clear_UI").SetActive(true);
        }
    }
}
